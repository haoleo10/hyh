using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System;
using YiSha.Util.Model;
using YiSha.Util;
using System.Collections.Generic;
using System.Net;
using NPOI.POIFS.Crypt.Dsig;
using System.Text.RegularExpressions;
using static NPOI.HSSF.Util.HSSFColor;
using YiSha.Model.IotDBData;
using Quartz;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics;
using YiSha.Business.SystemManage;
using YiSha.Entity.SystemManage;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Linq;
using System.Web;
using System.IO;
using System.IO.Compression;

namespace YiSha.Admin.WebApi.Controllers
{
    /// <summary>
    /// IotDB接口
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    [AuthorizeFilter]
    public class IotDBDataController : ControllerBase
    {

        public static List<SearchCondition> ConvertQueryStringToList(string queryString)
        {
            var conditions = new List<SearchCondition>();

            // 解析查询字符串
            var queryParams = HttpUtility.ParseQueryString(queryString);

            // 找出所有的索引
            var indexes = new HashSet<int>();
            foreach (var key in queryParams.AllKeys)
            {
                if (key.StartsWith("[") && key.Contains("]"))
                {
                    var indexStr = key.Substring(1, key.IndexOf("]") - 1);
                    if (int.TryParse(indexStr, out int index))
                    {
                        indexes.Add(index);
                    }
                }
            }

            // 为每个索引创建一个SearchCondition对象
            foreach (var index in indexes.OrderBy(i => i))
            {
                var condition = new SearchCondition();

                // 提取并转换各个属性
                if (queryParams[$"[{index}].condition"] != null)
                {
                    condition.condition = int.Parse(queryParams[$"[{index}].condition"]);
                }

                condition.doneWorkName = queryParams[$"[{index}].doneWorkName"];
                condition.startTime = queryParams[$"[{index}].startTime"];
                condition.endTime = queryParams[$"[{index}].endTime"];
                condition.valueMin = queryParams[$"[{index}].valueMin"];
                condition.valueMax = queryParams[$"[{index}].valueMax"];

                conditions.Add(condition);
            }

            return conditions;
        }


        private FSSocketBLL socketBLL = new FSSocketBLL();

        /// <summary>
        /// EchartData接口
        /// </summary>
        /// <param name="DoneWorkName"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="State"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<iotData>> GetIOTDBechatrDatas([FromQuery] string DoneWorkName, [FromQuery] string StartTime, [FromQuery] string EndTime, [FromQuery] string State)
        {
            //接受返回数据字符串
            string responseBody = "";

            //接受错误信息
            string error = "";
            using (HttpClient client = new HttpClient())
            {

                // 添加Authorization头root.test_group.test_device
                client.DefaultRequestHeaders.Add("Authorization", "cm9vdDpyb290");
                //定义查询语句 
                var json = "";
                //定义query接口 查询前段语句 条件为空时 默认
                //json = "{\"sql\":\"SELECT * FROM root.reactor.system1.techSystem1.DX.FFS_" + DoneWorkName + " where time >" + DateTime.Parse(StartTime).ToString("yyyy-MM-dd HH:mm:ss") + " and time<" + DateTime.Parse(EndTime).ToString("yyyy-MM-dd HH:mm:ss") + "\" limit 9000}";
                json = "{\"sql\":\"SELECT * FROM root.FSS." + DoneWorkName + " where time >" + DateTime.Parse(StartTime).ToString("yyyy-MM-dd HH:mm:ss") + " and time<" + DateTime.Parse(EndTime).ToString("yyyy-MM-dd HH:mm:ss") + " limit 1000\" }";


                //定义query接口
                var url = GlobalContext.SystemConfig.IotDBURLNew;
                if (string.IsNullOrWhiteSpace(StartTime) && string.IsNullOrWhiteSpace(EndTime) || DateTime.Parse(StartTime) == DateTime.MinValue && DateTime.Parse(EndTime) == DateTime.MinValue)
                {
                    url = GlobalContext.SystemConfig.IotDBURLNew;
                }
                else
                {
                    url = GlobalContext.SystemConfig.IotDBURL;
                }

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                try
                {
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    response.EnsureSuccessStatusCode();
                    responseBody = await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException e)
                {

                    error = e.Message;
                }
            }
            if (!string.IsNullOrWhiteSpace(error))
            {
                List<iotData> iotDatas = new List<iotData>();
                iotData data = new iotData
                {
                    Data_type = error
                };
                iotDatas.Add(data);
                return iotDatas;
            }
            else
            {
                try
                {
                    IotDBinfoLow jsonData = JsonConvert.DeserializeObject<IotDBinfoLow>(responseBody);
                    List<iotData> iotDatas = new List<iotData>();
                    string pattern = @"(?<=\.)([A-Z0-9_]+)(?=\.value)";
                    if (jsonData != null && jsonData.values.Count > 0)
                    {
                        int count = jsonData.values[0].Count;
                        for (int i = 0; i < count; i++)
                        {
                            iotData data = new iotData
                            {
                                Id = jsonData.timestamps[i],
                                Column_name = DoneWorkName,
                                DataValue = float.Parse(jsonData.values[0][i]),
                                Data_type = jsonData.data_types[0] as string ?? jsonData.data_types[0].ToString()
                            };
                            iotDatas.Add(data);
                        }
                        return iotDatas;
                    }
                    else
                    {
                        return null;
                    }

                }
                catch (Exception ex)
                {
                    IotDBinfo jsonData = JsonConvert.DeserializeObject<IotDBinfo>(responseBody);
                    List<iotData> iotDatas = new List<iotData>();
                    string pattern = @"(?<=\.)([A-Z0-9_]+)(?=\.value)";
                    if (jsonData != null)
                    {
                        int count = jsonData.values[0][0].Count;
                        for (int i = 0; i < count; i++)
                        {
                            iotData data = new iotData
                            {
                                Id = jsonData.timestamps[i],
                                Column_name = DoneWorkName,
                                DataValue = float.Parse(jsonData.values[0][0][i]),
                                Data_type = "doublue"
                            };
                            iotDatas.Add(data);
                        }
                        return iotDatas;
                    }
                    else
                    {
                        return null;
                    }
                }

            }
        }

        /// <summary>
        /// echartData接口历史数据
        /// </summary>
        /// <param name="DoneWorkName"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="State"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public async Task<IotDBinfoLow> GetIOTDBDatasLS([FromQuery] string DoneWorkName, [FromQuery] string StartTime, [FromQuery] string EndTime, [FromQuery] string State)
        {
            //接受返回数据字符串
            string responseBody = "";

            //接受错误信息
            string error = "";
            using (HttpClient client = new HttpClient())
            {

                // 添加Authorization头root.test_group.test_device
                client.DefaultRequestHeaders.Add("Authorization", "cm9vdDpyb290");
                //定义查询语句 
                var json = "";
                //定义query接口 查询前段语句 条件为空时 默认
                //json = "{\"sql\":\"SELECT * FROM root.reactor.system1.techSystem1.DX.FFS_" + DoneWorkName + " where time >" + DateTime.Parse(StartTime).ToString("yyyy-MM-dd HH:mm:ss") + " and time<" + DateTime.Parse(EndTime).ToString("yyyy-MM-dd HH:mm:ss") + "\" limit 9000}";
                json = "{\"sql\":\"SELECT * FROM root.FSS.AA5" + DoneWorkName + " where time >" + DateTime.Parse(StartTime).ToString("yyyy-MM-dd HH:mm:ss") + " and time<" + DateTime.Parse(EndTime).ToString("yyyy-MM-dd HH:mm:ss") + " limit 10000\" }";


                //定义query接口
                var url = GlobalContext.SystemConfig.IotDBURLNew;
                if (string.IsNullOrWhiteSpace(StartTime) && string.IsNullOrWhiteSpace(EndTime) || DateTime.Parse(StartTime) == DateTime.MinValue && DateTime.Parse(EndTime) == DateTime.MinValue)
                {
                    url = GlobalContext.SystemConfig.IotDBURLNew;
                }
                else
                {
                    url = GlobalContext.SystemConfig.IotDBURL;
                }

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                try
                {
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    response.EnsureSuccessStatusCode();
                    responseBody = await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException e)
                {

                    error = e.Message;
                }
            }

            IotDBinfoLow jsonData = JsonConvert.DeserializeObject<IotDBinfoLow>(responseBody);

            return jsonData;

        }

        /// <summary>
        /// echartData接口历史数据
        /// </summary>
        /// <param name="DoneWorkName"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="State"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public async Task<IotDBinfoLow> GetIOTDBDataSearch([FromQuery] string DoneWorkName, [FromQuery] string StartTime, [FromQuery] string EndTime, [FromQuery] string State)
        {
            //接受返回数据字符串
            string responseBody = "";

            //接受错误信息
            string error = "";
            using (HttpClient client = new HttpClient())
            {

                // 添加Authorization头root.test_group.test_device
                client.DefaultRequestHeaders.Add("Authorization", "cm9vdDpyb290");
                //定义查询语句 
                var json = "";
                //定义query接口 查询前段语句 条件为空时 默认
                //json = "{\"sql\":\"SELECT * FROM root.reactor.system1.techSystem1.DX.FFS_" + DoneWorkName + " where time >" + DateTime.Parse(StartTime).ToString("yyyy-MM-dd HH:mm:ss") + " and time<" + DateTime.Parse(EndTime).ToString("yyyy-MM-dd HH:mm:ss") + "\" limit 9000}";
                json = "{\"sql\":\"SELECT * FROM root.KNS.AA5." + DoneWorkName + " where time >" + DateTime.Parse(StartTime).ToString("yyyy-MM-dd HH:mm:ss") + " and time<" + DateTime.Parse(EndTime).ToString("yyyy-MM-dd HH:mm:ss") + " limit 1000\" }";


                //定义query接口
                var url = GlobalContext.SystemConfig.IotDBURLNew;
                if (string.IsNullOrWhiteSpace(StartTime) && string.IsNullOrWhiteSpace(EndTime) || DateTime.Parse(StartTime) == DateTime.MinValue && DateTime.Parse(EndTime) == DateTime.MinValue)
                {
                    url = GlobalContext.SystemConfig.IotDBURLNew;
                }
                else
                {
                    url = GlobalContext.SystemConfig.IotDBURL;
                }

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                try
                {
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    response.EnsureSuccessStatusCode();
                    responseBody = await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException e)
                {

                    error = e.Message;
                }
            }

            IotDBinfoLow jsonData = JsonConvert.DeserializeObject<IotDBinfoLow>(responseBody);
            return jsonData;

        }


        /// <summary>
        /// 智能查询接口
        /// </summary>
        /// <param name="DoneWorkName"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="State">实时值0，历史值1</param>
        /// 
        /// <returns></returns>
        [HttpGet]
        public async Task<List<iotData>> GetIOTDBDatas([FromQuery] string DoneWorkName, [FromQuery] string StartTime, [FromQuery] string EndTime, [FromQuery] string ValueMin, [FromQuery] string ValueMax, [FromQuery] string State, [FromQuery] string rolesLis)
        {

            bool IsNew = false;
            List<SearchCondition> result = new List<SearchCondition>();

            if (!string.IsNullOrEmpty(rolesLis))
            {
                result = ConvertQueryStringToList(rolesLis);
            }

            //接受返回数据字符串
            string responseBody = "";

            //接受错误信息
            string error = "";

            //定义多条件查询语句T-sql
            List<string> t_sqlList = new List<string>();
            if (result.Count==0)
            {
                if (string.IsNullOrWhiteSpace(StartTime) && string.IsNullOrWhiteSpace(EndTime) || DateTime.Parse(StartTime).ToString() == DateTime.MinValue.ToString() && DateTime.Parse(EndTime).ToString() == DateTime.MinValue.ToString())
                {
                    if (!string.IsNullOrWhiteSpace(DoneWorkName) && string.IsNullOrWhiteSpace(StartTime) && string.IsNullOrWhiteSpace(EndTime))
                    {
                        string jsonsql = "{\"prefix_paths\":[\"root\",\"KNS\",\"AA5\",\"*" + DoneWorkName + "*\"]}";
                        t_sqlList.Add(jsonsql);
                        IsNew = true;
                    }
                    else if (string.IsNullOrWhiteSpace(StartTime) && string.IsNullOrWhiteSpace(EndTime))
                    {
                        string jsonsql = "{\"prefix_paths\":[\"root\",\"KNS\",\"AA5\",\"*ABP*\"]}";
                        IsNew = true;
                        t_sqlList.Add(jsonsql);
                    }

                }
                else
                {
                    //定义query接口 查询前段语句 条件为空时 默认
                    string jsonsql = "select last* from root.KNS.AA5.** limit 10";
                    if (!string.IsNullOrWhiteSpace(DoneWorkName) && (DateTime.Parse(StartTime).ToString() != DateTime.MinValue.ToString()) && (DateTime.Parse(EndTime).ToString() != DateTime.MinValue.ToString()))
                    {
                        //名称进行模糊查询
                        jsonsql = "{\"sql\":\"SELECT last * FROM root.KNS.AA5.*" + DoneWorkName + "* where time >" + DateTime.Parse(StartTime).ToString("yyyy-MM-dd HH:mm:ss") + " and time<" + DateTime.Parse(EndTime).ToString("yyyy-MM-dd HH:mm:ss") + "\"}";
                    }
                    else if (DateTime.Parse(StartTime).ToString() != DateTime.MinValue.ToString() && DateTime.Parse(EndTime).ToString() != DateTime.MinValue.ToString())
                    {
                        //时间进行查询
                        jsonsql = "{\"sql\":\"SELECT last * FROM root.KNS.AA5.** where time >" + DateTime.Parse(StartTime).ToString("yyyy-MM-dd HH:mm:ss") + " and time<" + DateTime.Parse(EndTime).ToString("yyyy-MM-dd HH:mm:ss") + " limit 100\"}";
                    }
                    t_sqlList.Add(jsonsql);

                }

            }
            

            foreach (var item in result)
            {
                if (State == "1")
                {
                    string jsonsql = "select last* from root.KNS.AA5.** limit 100";
                    if (!string.IsNullOrWhiteSpace(item.doneWorkName) && !string.IsNullOrWhiteSpace(item.startTime) && !string.IsNullOrWhiteSpace(item.endTime))
                    {

                         jsonsql = "{\"sql\":\"SELECT * FROM root.KNS.AA5.*" + item.doneWorkName + "* where time >" + DateTime.Parse(item.startTime).ToString("yyyy-MM-dd HH:mm:ss") + " and time<" + DateTime.Parse(item.endTime).ToString("yyyy-MM-dd HH:mm:ss") + "limit 3000\"}";
                        t_sqlList.Add(jsonsql);
                    }
                    else
                    {
                         jsonsql = "{\"sql\":\"SELECT * FROM root.KNS.AA5.*" + item.doneWorkName + "* limit 1000\"}";
                        t_sqlList.Add(jsonsql);
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(StartTime) && string.IsNullOrWhiteSpace(EndTime) || DateTime.Parse(StartTime).ToString() == DateTime.MinValue.ToString() && DateTime.Parse(EndTime).ToString() == DateTime.MinValue.ToString())
                    {
                        if (!string.IsNullOrWhiteSpace(DoneWorkName) && string.IsNullOrWhiteSpace(StartTime) && string.IsNullOrWhiteSpace(EndTime))
                        {
                            string jsonsql = "{\"prefix_paths\":[\"root\",\"KNS\",\"AA5\",\"*" + DoneWorkName + "*\"]}";
                            t_sqlList.Add(jsonsql);
                            IsNew = true;
                        }
                        else if (string.IsNullOrWhiteSpace(StartTime) && string.IsNullOrWhiteSpace(EndTime))
                        {
                            string jsonsql = "{\"prefix_paths\":[\"root\",\"KNS\",\"AA5\",\"*ABP*\"]}";
                            t_sqlList.Add(jsonsql);
                            IsNew = true;
                        }

                    }
                    else
                    {
                        //定义query接口 查询前段语句 条件为空时 默认
                        string jsonsql = "select last* from root.KNS.AA5.** limit 10";
                        if (!string.IsNullOrWhiteSpace(DoneWorkName) && (DateTime.Parse(StartTime).ToString() != DateTime.MinValue.ToString()) && (DateTime.Parse(EndTime).ToString() != DateTime.MinValue.ToString()))
                        {
                            //名称进行模糊查询
                            jsonsql = "{\"sql\":\"SELECT last * FROM root.KNS.AA5.*" + DoneWorkName + "* where time >" + DateTime.Parse(StartTime).ToString("yyyy-MM-dd HH:mm:ss") + " and time<" + DateTime.Parse(EndTime).ToString("yyyy-MM-dd HH:mm:ss") + "\"}";
                        }
                        else if (DateTime.Parse(StartTime).ToString() != DateTime.MinValue.ToString() && DateTime.Parse(EndTime).ToString() != DateTime.MinValue.ToString())
                        {
                            //时间进行查询
                            jsonsql = "{\"sql\":\"SELECT last * FROM root.KNS.AA5.** where time >" + DateTime.Parse(StartTime).ToString("yyyy-MM-dd HH:mm:ss") + " and time<" + DateTime.Parse(EndTime).ToString("yyyy-MM-dd HH:mm:ss") + " limit 100\"}";
                        }
                        t_sqlList.Add(jsonsql);

                    }

                }

            }

            List<string> responseBodyList = new List<string>();
            using (HttpClient client = new HttpClient())
            {
                // 添加Authorization头root.test_group.test_device
                client.DefaultRequestHeaders.Add("Authorization", "cm9vdDpyb290");          

                foreach (var item in t_sqlList)
                {
                    var content = new StringContent(item, Encoding.UTF8, "application/json");
                    try
                    {
                        if (IsNew)
                        {
                            HttpResponseMessage response = await client.PostAsync(GlobalContext.SystemConfig.IotDBURLNew, content);
                            response.EnsureSuccessStatusCode();
                            responseBody = await response.Content.ReadAsStringAsync();
                            responseBodyList.Add(responseBody);
                        }
                        else
                        {
                            HttpResponseMessage response = await client.PostAsync(GlobalContext.SystemConfig.IotDBURL, content);
                            response.EnsureSuccessStatusCode();
                            responseBody = await response.Content.ReadAsStringAsync();
                            responseBodyList.Add(responseBody);
                        }
                       
                    }
                    catch (HttpRequestException e)
                    {

                        error = e.Message;
                    }
                }
            }

            //整合数据
            List<iotData> iotDataLists = new List<iotData>();
            foreach (var item in responseBodyList)
            {
                try
                {
                    TimeseriesData jsonData = JsonConvert.DeserializeObject<TimeseriesData>(item);

                    string pattern = @"(?<=\.)([A-Z0-9_]+)(?=\.value)";
                    if (jsonData != null && jsonData.Values.Count > 0)
                    {
                        int count = jsonData.Values[0][0].Count;
                        for (int i = 0; i < count; i++)
                        {
                            iotData data = new iotData
                            {
                                Id = jsonData.Timestamps[i],
                                Column_name = Regex.Match(jsonData.Values[0][0][i] as string, pattern).Groups[1].Value ?? Regex.Match(jsonData.Values[0][0][i].ToString(), pattern).Groups[1].Value,
                                DataValue = float.Parse(jsonData.Values[0][1][i]),
                                Data_type = jsonData.Values[0][2][i] as string ?? jsonData.Values[0][2][i].ToString()
                            };
                            iotDataLists.Add(data);
                        }
                    }
                    else
                    {
                        return null;
                    }

                }
                catch (Exception ex)
                {
                    IotDBinfoLow jsonData = JsonConvert.DeserializeObject<IotDBinfoLow>(item);
                    string pattern = @"(?<=\.)([A-Z0-9_]+)(?=\.value)";
                    if (jsonData != null && jsonData.values.Count > 0)
                    {
                        int count = jsonData.values[0].Count;
                        for (int i = 0; i < count; i++)
                        {
                            iotData data = new iotData
                            {
                                Id = jsonData.timestamps[i],
                                Column_name = Regex.Match(jsonData.expressions[0] as string, pattern).Groups[1].Value ?? Regex.Match(jsonData.expressions[0].ToString(), pattern).Groups[1].Value,
                                DataValue = float.Parse(jsonData.values[0][i]),
                                Data_type = jsonData.data_types[0] as string ?? jsonData.data_types[0].ToString()
                            };
                            iotDataLists.Add(data);
                        }

                    }
                    else
                    {
                      //  return null;
                    }
                }

            }

            List<iotData> filteredData = new List<iotData>();

            foreach (var data in iotDataLists)
            {
                bool isMatch = false;

                foreach (var item in result)
                {
                    if (data.Column_name == item.doneWorkName)
                    {
                        if (string.IsNullOrEmpty(item.valueMin)&&string.IsNullOrEmpty(item.valueMax) || data.DataValue > float.Parse(item.valueMin) && data.DataValue < float.Parse(item.valueMax))
                        {
                            isMatch = true;
                            break;
                        }
                    }
                }
                if (isMatch||string.IsNullOrWhiteSpace(DoneWorkName))
                {
                    filteredData.Add(data);
                }
            }

            return filteredData;

        }


        public class SearchCondition
        {
            public int condition { get; set; }
            public string doneWorkName { get; set; }
            public string startTime { get; set; }
            public string endTime { get; set; }
            public string valueMin { get; set; }
            public string valueMax { get; set; }
        }



        /// <summary>
        /// 获取IotDB区域节点数据40000条
        /// </summary>
        /// <returns></returns>
        /// 

        [ApiExplorerSettings(IgnoreApi = true)] // 让Swagger忽略这个接口
        [HttpGet]
        public async Task<TData<IotDBinfo>> GetIOTDBData40000()
        {
            TData<IotDBinfo> obj = new TData<IotDBinfo>();
            string responseBody = "";
            using (HttpClient client = new HttpClient())
            {
                // 添加Authorization头root.test_group.test_device
                client.DefaultRequestHeaders.Add("Authorization", "cm9vdDpyb290");
                var json = "{\"prefix_paths\":[\"root\",\"KNS\",\"AA5\"]}";

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                try
                {
                    HttpResponseMessage response = await client.PostAsync(GlobalContext.SystemConfig.IotDBURLNew, content);
                    response.EnsureSuccessStatusCode();
                    responseBody = await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException e)
                {
                    obj.Message = e.Message;
                }
            }
            IotDBinfo data = JsonConvert.DeserializeObject<IotDBinfo>(responseBody);
            obj.Data = data;
            return obj;
        }


        /// <summary>
        /// 获取IotDB节点数据(多个节点请用”，“”隔开，如：“root.KNS.AA5.5DVF060VAFSM5,root.KNS.AA5.5CFI001MOA1“)
        /// </summary>
        /// <param name="Nodes">root.KNS.AA5.5DVF060VAFSM5,root.KNS.AA5.5CFI001MOA1</param>
        /// <param name="SYS"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<TData<IotDBinfoLow>> GetIOTDBDataNodes([FromQuery] string Nodes, [FromQuery] string SYS)
        {
            TData<IotDBinfoLow> obj = new TData<IotDBinfoLow>();
            string responseBody = "";
            using (HttpClient client = new HttpClient())
            {
                // 添加Authorization头root.test_group.test_device
                client.DefaultRequestHeaders.Add(GlobalContext.SystemConfig.Authorization, GlobalContext.SystemConfig.AuthorizationValue);
                var json = "";
                if (SYS == "3K")
                {
                    json = "{\"sql\":\"select last * from " + Nodes + " order by timeseries desc;\"}";
                }
                else
                {
                    json = "{\"sql\":\"select last * from " + Nodes + " order by timeseries desc;\"}";

                }
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage response = await client.PostAsync(GlobalContext.SystemConfig.IotDBURL, content);
                    response.EnsureSuccessStatusCode();
                    responseBody = await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException e)
                {
                    obj.Message = e.Message;
                }
            }
            IotDBinfoLow jsonData = JsonConvert.DeserializeObject<IotDBinfoLow>(responseBody);
            obj.Data = jsonData;
            return obj;
        }


        /// <summary>
        /// 根据时间区间查询数据
        /// </summary>
        /// <param name="Node">点名(root.KNS.AA5.5DVD008ZVRP)</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="limit">最大数量</param>       
        /// <returns></returns>
        [HttpGet]
        public async Task<TData<IotDBinfo>> GetIOTDBTimeDataNodes([FromQuery] string Node, [FromQuery] DateTime startTime, [FromQuery] DateTime endTime, int limit = 100)
        {
            TData<IotDBinfo> obj = new TData<IotDBinfo>();
            string responseBody = "";
            using (HttpClient client = new HttpClient())
            {
                // 添加Authorization头root.test_group.test_device
                client.DefaultRequestHeaders.Add(GlobalContext.SystemConfig.Authorization, GlobalContext.SystemConfig.AuthorizationValue);
                var json = "{\"sql\":\"select last* from " + Node + " where time >" + startTime.ToString("yyyy-MM-dd HH:mm:ss") + " and time<" + endTime.ToString("yyyy-MM-dd HH:mm:ss") + " limit " + limit.ToString() + "\"}";
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                try
                {
                    HttpResponseMessage response = await client.PostAsync(GlobalContext.SystemConfig.IotDBURL, content);
                    response.EnsureSuccessStatusCode();
                    responseBody = await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException e)
                {
                    obj.Message = e.Message;
                }
            }
            IotDBinfo jsonData = JsonConvert.DeserializeObject<IotDBinfo>(responseBody);
            obj.Data = jsonData;
            return obj;
        }

        /// <summary>
        /// 系统名查询+时间
        /// </summary>
        /// <param name="SystemName">名字（模糊查询）</param>
        /// <param name="startTime">起始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<TData<IotDBinfoLow>> GetIOTDBSyetem([FromQuery] string SystemName, [FromQuery] DateTime startTime, [FromQuery] DateTime endTime)
        {
            TData<IotDBinfoLow> obj = new TData<IotDBinfoLow>();
            string responseBody = "";
            using (HttpClient client = new HttpClient())
            {
                // 添加Authorization头root.test_group.test_device
                client.DefaultRequestHeaders.Add(GlobalContext.SystemConfig.Authorization, GlobalContext.SystemConfig.AuthorizationValue);
                var json = "";
                if (startTime != default(DateTime) && endTime != default(DateTime))
                {
                    json = "{\"sql\":\"SELECT last * FROM root.KNS.AA5.*" + SystemName + "* where time >" + startTime.ToString("yyyy-MM-dd HH:mm:ss") + " and time<" + endTime.ToString("yyyy-MM-dd HH:mm:ss") + "\"}";
                }
                else
                {
                    json = "{\"sql\":\"SELECT last * FROM root.KNS.AA5.*" + SystemName + "*\"}";
                }
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                try
                {
                    HttpResponseMessage response = await client.PostAsync(GlobalContext.SystemConfig.IotDBURL, content);
                    response.EnsureSuccessStatusCode();
                    responseBody = await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException e)
                {
                    obj.Message = e.Message;
                }
            }
            IotDBinfoLow jsonData = JsonConvert.DeserializeObject<IotDBinfoLow>(responseBody);
            obj.Data = jsonData;
            return obj;
        }


        /// <summary>
        /// 系统名查询（最新接口）
        /// </summary>
        /// <param name="SystemName"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<TData<IotDBinfo>> GetIOTDBSyetem1([FromQuery] string SystemName)
        {
            TData<IotDBinfo> obj = new TData<IotDBinfo>();
            string responseBody = "";
            using (HttpClient client = new HttpClient())
            {
                // 添加Authorization头root.test_group.test_device
                client.DefaultRequestHeaders.Add("Authorization", "cm9vdDpyb290");
                var json = "{\"prefix_paths\":[\"root\",\"KNS\",\"AA5\",\"*" + SystemName + "*\"]}";

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                try
                {
                    HttpResponseMessage response = await client.PostAsync(GlobalContext.SystemConfig.IotDBURLNew, content);
                    response.EnsureSuccessStatusCode();
                    responseBody = await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException e)
                {
                    obj.Message = e.Message;
                }
            }
            IotDBinfo data = JsonConvert.DeserializeObject<IotDBinfo>(responseBody);
            obj.Data = data;
            return obj;
        }


        [HttpGet]
        public async Task<IotDBinfo> GetIOTDBThreeSyetem([FromQuery] string SystemName)
        {
            TData<IotDBinfo> obj = new TData<IotDBinfo>();
            string responseBody = "";
            using (HttpClient client = new HttpClient())
            {
                // 添加Authorization头root.test_group.test_device
                client.DefaultRequestHeaders.Add("Authorization", "cm9vdDpyb290");
               // var json = "{\"prefix_paths\":[\"root\",\"KNS\",\"AA5\",\"*" + SystemName + "*\"]}";
                var json = "{\"prefix_paths\":[\"root\",\"*\",\"*\",\"*\",\"*\",\"*\",\"*\",\"*" + SystemName + "*\"]}";


                var content = new StringContent(json, Encoding.UTF8, "application/json");
                try
                {
                    HttpResponseMessage response = await client.PostAsync(GlobalContext.SystemConfig.IotDBURLNew, content);
                    response.EnsureSuccessStatusCode();
                    responseBody = await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException e)
                {
                    obj.Message = e.Message;
                }
            }
            IotDBinfo data = JsonConvert.DeserializeObject<IotDBinfo>(responseBody);
            return data;
        }





        /// <summary>
        /// 点名查询（最新接口）
        /// </summary>
        /// <param name="PointName"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<TData<IotDBinfoLow>> GetIOTDBPoint([FromQuery] string PointName)
        {
            TData<IotDBinfoLow> obj = new TData<IotDBinfoLow>();
            string responseBody = "";
            using (HttpClient client = new HttpClient())
            {
                // 添加Authorization头root.test_group.test_device
                client.DefaultRequestHeaders.Add("Authorization", "cm9vdDpyb290");
                var json = "{\"prefix_paths\":[\"root\",\"KNS\",\"AA5\",\"*" + PointName + "*\"]}";

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                try
                {
                    HttpResponseMessage response = await client.PostAsync(GlobalContext.SystemConfig.IotDBURLNew, content);
                    response.EnsureSuccessStatusCode();
                    responseBody = await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException e)
                {
                    obj.Message = e.Message;
                }
            }
            IotDBinfoLow timeseriesData = JsonConvert.DeserializeObject<IotDBinfoLow>(responseBody);
            obj.Data = timeseriesData;
            return obj;
        }



        /// <summary>
        /// 数据清洗接口
        /// </summary>
        /// <param name="System">系统</param>
        /// <param name="DeviceName">设备名称</param>
        /// <param name="StartTime">起始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<iotData>> GetIotDBDataCleaning([FromQuery] string System, [FromQuery] string DeviceName, [FromQuery] string StartTime, [FromQuery] string EndTime)
        {
            //接受返回数据字符串
            string responseBody = "";

            //接受错误信息
            string error = "";
            using (HttpClient client = new HttpClient())
            {
                // 添加Authorization头root.test_group.test_device
                client.DefaultRequestHeaders.Add("Authorization", "cm9vdDpyb290");
                //定义查询语句 
                var json = "";
                if (string.IsNullOrWhiteSpace(StartTime) && string.IsNullOrWhiteSpace(EndTime) || DateTime.Parse(StartTime) != DateTime.MinValue && DateTime.Parse(EndTime) != DateTime.MinValue)
                {
                    //时间为空使用快速查询接口
                    if (!string.IsNullOrWhiteSpace(System) && !string.IsNullOrWhiteSpace(DeviceName) && string.IsNullOrWhiteSpace(StartTime) && string.IsNullOrWhiteSpace(EndTime))
                    {
                        json = "{\"prefix_paths\":[\"root\",\"KNS\",\"AA5\",\"*" + System + "*" + DeviceName + "*\"]}";

                    }
                    else if (!string.IsNullOrWhiteSpace(System) && string.IsNullOrWhiteSpace(StartTime) && string.IsNullOrWhiteSpace(EndTime))
                    {
                        json = "{\"prefix_paths\":[\"root\",\"KNS\",\"AA5\",\"*" + System + "*\"]}";

                    }
                    else if (!string.IsNullOrWhiteSpace(DeviceName) && string.IsNullOrWhiteSpace(StartTime) && string.IsNullOrWhiteSpace(EndTime))
                    {
                        json = "{\"prefix_paths\":[\"root\",\"KNS\",\"AA5\",\"*" + DeviceName + "*\"]}";
                    }
                    else if (string.IsNullOrWhiteSpace(StartTime) && string.IsNullOrWhiteSpace(EndTime))
                    {
                        json = "{\"prefix_paths\":[\"root\",\"KNS\",\"AA5\",\"*ABP*\"]}";
                    }
                    else if (!string.IsNullOrWhiteSpace(StartTime) && !string.IsNullOrWhiteSpace(EndTime) && DateTime.Parse(StartTime) != DateTime.MinValue && DateTime.Parse(EndTime) != DateTime.MinValue)
                    {
                        json = "{\"sql\":\"SELECT last * FROM root.KNS.AA5.** where time >" + DateTime.Parse(StartTime).ToString("yyyy-MM-dd HH:mm:ss") + " and time<" + DateTime.Parse(EndTime).ToString("yyyy-MM-dd HH:mm:ss") + " limit 2000\"}";

                    }

                }
                else
                {
                    //定义query接口 查询前段语句 条件为空时 默认
                    json = "select last* from root.KNS.AA5.** limit 10";
                    //时间不为空 使用query接口
                    if (!string.IsNullOrWhiteSpace(System) && !string.IsNullOrWhiteSpace(DeviceName) && DateTime.Parse(StartTime) != DateTime.MinValue && DateTime.Parse(EndTime) != DateTime.MinValue)
                    {
                        //类型和名称进行模糊查询
                        // json = "{\"sql\":\"SELECT last * FROM root.KNS.AA5.*" + System + "*" + DeviceName + "* where time >" + DateTime.Parse(StartTime).ToString("yyyy-MM-dd HH:mm:ss") + " and time<" + DateTime.Parse(EndTime).ToString("yyyy-MM-dd HH:mm:ss") + "\"}";
                        json = "{\"sql\":\"SELECT last * FROM root.KNS.AA5.*" + System + "* where time >" + DateTime.Parse(StartTime).ToString("yyyy-MM-dd HH:mm:ss") + " and time<" + DateTime.Parse(EndTime).ToString("yyyy-MM-dd HH:mm:ss") + "\"}";

                    }
                    else if (!string.IsNullOrWhiteSpace(System) && DateTime.Parse(StartTime) != DateTime.MinValue && DateTime.Parse(EndTime) != DateTime.MinValue)
                    {
                        //系统进行模糊查询
                        json = "{\"sql\":\"SELECT last * FROM root.KNS.AA5.*" + System + "* where time >" + DateTime.Parse(StartTime).ToString("yyyy-MM-dd HH:mm:ss") + " and time<" + DateTime.Parse(EndTime).ToString("yyyy-MM-dd HH:mm:ss") + "\"}";
                    }
                    else if (!string.IsNullOrWhiteSpace(DeviceName) && DateTime.Parse(StartTime) != DateTime.MinValue && DateTime.Parse(EndTime) != DateTime.MinValue)
                    {
                        //名称进行模糊查询
                        json = "{\"sql\":\"SELECT last * FROM root.KNS.AA5.*" + DeviceName + "* where time >" + DateTime.Parse(StartTime).ToString("yyyy-MM-dd HH:mm:ss") + " and time<" + DateTime.Parse(EndTime).ToString("yyyy-MM-dd HH:mm:ss") + "\"}";
                    }
                    else if (DateTime.Parse(StartTime) != DateTime.MinValue && DateTime.Parse(EndTime) != DateTime.MinValue)
                    {
                        //时间进行查询
                        json = "{\"sql\":\"SELECT last * FROM root.KNS.AA5.** where time >" + DateTime.Parse(StartTime).ToString("yyyy-MM-dd HH:mm:ss") + " and time<" + DateTime.Parse(EndTime).ToString("yyyy-MM-dd HH:mm:ss") + " limit 2000\"}";
                    }

                }

                //定义query接口
                var url = GlobalContext.SystemConfig.IotDBURLNew;
                if (string.IsNullOrWhiteSpace(StartTime) && string.IsNullOrWhiteSpace(EndTime) || DateTime.Parse(StartTime) == DateTime.MinValue && DateTime.Parse(EndTime) == DateTime.MinValue)
                {
                    url = GlobalContext.SystemConfig.IotDBURLNew;
                }
                else
                {
                    url = GlobalContext.SystemConfig.IotDBURL;
                }

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                try
                {
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    response.EnsureSuccessStatusCode();
                    responseBody = await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException e)
                {

                    error = e.Message;
                }
            }

            if (!string.IsNullOrWhiteSpace(error))
            {
                List<iotData> iotDatas = new List<iotData>();
                iotData data = new iotData
                {
                    Data_type = error
                };
                iotDatas.Add(data);
                return iotDatas;
            }
            else
            {
                try
                {
                    IotDBinfo jsonData = JsonConvert.DeserializeObject<IotDBinfo>(responseBody);
                    List<iotData> iotDatas = new List<iotData>();
                    string pattern = @"(?<=\.)([A-Z0-9_]+)(?=\.value)";
                    if (jsonData != null)
                    {
                        int count = jsonData.values[0][0].Count;
                        for (int i = 0; i < count; i++)
                        {
                            iotData data = new iotData
                            {
                                Id = jsonData.timestamps[i],
                                Column_name = Regex.Match(jsonData.values[0][0][i].ToString(), pattern).Groups[1].Value ?? Regex.Match(jsonData.values[0][0][i].ToString(), pattern).Groups[1].Value,
                                DataValue = float.Parse(jsonData.values[0][1][i]),
                                Data_type = jsonData.values[0][2][i].ToString() ?? jsonData.values[0][2][i].ToString()
                            };
                            iotDatas.Add(data);
                        }
                        return iotDatas;
                    }
                    else
                    {
                        return null;
                    }

                }
                catch (Exception ex)
                {

                    IotDBinfoLow jsonData = JsonConvert.DeserializeObject<IotDBinfoLow>(responseBody);
                    List<iotData> iotDatas = new List<iotData>();
                    string pattern = @"(?<=\.)([A-Z0-9_]+)(?=\.value)";
                    if (jsonData != null)
                    {
                        for (int i = 0; i < jsonData.values[0].Count; i++)
                        {
                            iotData data1 = new iotData
                            {
                                Id = jsonData.timestamps[i],
                                Column_name = Regex.Match(jsonData.values[0][i].ToString(), pattern).Groups[1].Value ?? Regex.Match(jsonData.values[0][i].ToString(), pattern).Groups[1].Value,
                                DataValue = float.Parse(jsonData.values[1][i]),
                                Data_type = jsonData.values[2][i].ToString() ?? jsonData.values[2][i].ToString()
                            };
                            iotDatas.Add(data1);

                        }

                        return iotDatas;
                    }
                    else
                    {
                        return null;

                    }

                }

            }

        }

        /// <summary>
        /// 3K 数据接口
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>

        [HttpGet]
        public async Task<List<iotData>> GetIotDBData3K([FromQuery] string node)
        {
            TData<IotDBinfo> obj = new TData<IotDBinfo>();
            string responseBody = "";
            using (HttpClient client = new HttpClient())
            {
                // 添加Authorization头root.test_group.test_device
                client.DefaultRequestHeaders.Add(GlobalContext.SystemConfig.Authorization, GlobalContext.SystemConfig.AuthorizationValue);
                string json = "{\"prefix_paths\":[\"root\",\"FSS\",\"AA5\",\"*" + node + "*\"]}";
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                try
                {
                    HttpResponseMessage response = await client.PostAsync(GlobalContext.SystemConfig.IotDBURLNew, content);
                    response.EnsureSuccessStatusCode();
                    responseBody = await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException e)
                {
                    obj.Message = e.Message;
                }
            }
            try
            {
                TimeseriesData jsonData = JsonConvert.DeserializeObject<TimeseriesData>(responseBody);
                List<iotData> iotDatas = new List<iotData>();
                string pattern = @"(?<=\.)([A-Z0-9_]+)(?=\.value)";
                if (jsonData != null && jsonData.Values.Count > 0)
                {
                    int count = jsonData.Values[0][0].Count;
                    for (int i = 0; i < count; i++)
                    {
                        iotData data = new iotData
                        {
                            Id = jsonData.Timestamps[i],
                            Column_name = Regex.Match(jsonData.Values[0][0][i] as string, pattern).Groups[1].Value ?? Regex.Match(jsonData.Values[0][0][i].ToString(), pattern).Groups[1].Value,
                            DataValue = float.Parse(jsonData.Values[0][1][i]),
                            Data_type = jsonData.Values[0][2][i] as string ?? jsonData.Values[0][2][i].ToString()
                        };
                        iotDatas.Add(data);
                    }
                    return iotDatas;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                IotDBinfoLow jsonData = JsonConvert.DeserializeObject<IotDBinfoLow>(responseBody);
                List<iotData> iotDatas = new List<iotData>();
                string pattern = @"(?<=\.)([A-Z0-9_]+)(?=\.value)";
                if (jsonData != null && jsonData.values.Count > 0)
                {
                    int count = jsonData.values[0].Count;
                    for (int i = 0; i < count; i++)
                    {
                        iotData data = new iotData
                        {
                            Id = jsonData.timestamps[i],
                            Column_name = Regex.Match(jsonData.expressions[0] as string, pattern).Groups[1].Value ?? Regex.Match(jsonData.expressions[0].ToString(), pattern).Groups[1].Value,
                            DataValue = float.Parse(jsonData.values[0][i]),
                            Data_type = jsonData.data_types[0] as string ?? jsonData.data_types[0].ToString()
                        };
                        iotDatas.Add(data);
                    }
                    return iotDatas;
                }
                else
                {
                    return null;
                }
            }



        }


        /// <summary>
        /// 热力性能接口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IotDBinfoLow> GetIotDBAfficiencyAnalysis()
        {
            string responseBody = "";
            string Nodes = "root.FSS.HYH_W3_HYH_DQHEEN8,root.FSS.HYH_W3_HYH_DQHEEN59,root.FSS.HYH_W3_HYH_DQHEEN61,root.FSS.HYH_W3_HYH_DQHEEN135,root.FSS.HYH_W3_HYH_DQHEEN67,root.FSS.HYH_W3_HYH_DQHEEN71,root.FSS.HYH_W3_HYH_DQHEEN76,root.FSS.HYH_W3_HYH_DQHEEN81,root.FSS.HYH_W3_HYH_DQHEEN88,root.FSS.HYH_W3_HYH_DQHEEN111,root.FSS.HYH_W3_HYH_DQHEEN114,root.FSS.HYH_W3_HYH_DQHEEN117,root.FSS.HYH_W3_HYH_DQHEEN125,root.FSS.HYH_W3_HYH_DQHEEN126,root.FSS.HYH_W3_HYH_DQHEEN141,root.FSS.HYH_W3_HYH_SZVXAU3,root.FSS.HYH_W3_HYH_SZVXAU27,root.FSS.HYH_W3_HYH_SZVXAU22,root.FSS.HYH_W3_HYH_SZVXAU26,root.FSS.HYH_W3_HYH_LHIYWZ2";

            using (HttpClient client = new HttpClient())
            {
                // 添加Authorization头root.test_group.test_device
                client.DefaultRequestHeaders.Add(GlobalContext.SystemConfig.Authorization, GlobalContext.SystemConfig.AuthorizationValue);
                var json = "";
                json = "{\"sql\":\"select last * from " + Nodes + " order by timeseries desc;\"}";
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage response = await client.PostAsync(GlobalContext.SystemConfig.IotDBURL, content);
                    response.EnsureSuccessStatusCode();
                    responseBody = await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException e)
                {

                }
            }
            IotDBinfoLow jsonData = JsonConvert.DeserializeObject<IotDBinfoLow>(responseBody);

            return jsonData;
        }


        /// <summary>
        /// 热力性能接口111
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<TData<IotDBinfoLow>> GetIotDBAfficiencyAnalysisEnergyAllocation()
        {
            string responseBody = "";
            string Nodes = "root.FSS.HYH_W3_HYH_DQHEEN8,root.FSS.HYH_W3_HYH_DQHEEN59,root.FSS.HYH_W3_HYH_DQHEEN61,root.FSS.HYH_W3_HYH_DQHEEN135,root.FSS.HYH_W3_HYH_DQHEEN67,root.FSS.HYH_W3_HYH_DQHEEN71,root.FSS.HYH_W3_HYH_DQHEEN76,root.FSS.HYH_W3_HYH_DQHEEN81,root.FSS.HYH_W3_HYH_DQHEEN88,root.FSS.HYH_W3_HYH_DQHEEN111,root.FSS.HYH_W3_HYH_DQHEEN114,root.FSS.HYH_W3_HYH_DQHEEN117,root.FSS.HYH_W3_HYH_DQHEEN125,root.FSS.HYH_W3_HYH_DQHEEN126,root.FSS.HYH_W3_HYH_DQHEEN141,root.FSS.HYH_W3_HYH_SZVXAU3,root.FSS.HYH_W3_HYH_SZVXAU27,root.FSS.HYH_W3_HYH_SZVXAU22,root.FSS.HYH_W3_HYH_SZVXAU26,root.FSS.HYH_W3_HYH_LHIYWZ2";

            using (HttpClient client = new HttpClient())
            {
                // 添加Authorization头root.test_group.test_device
                client.DefaultRequestHeaders.Add(GlobalContext.SystemConfig.Authorization, GlobalContext.SystemConfig.AuthorizationValue);
                var json = "";
                json = "{\"sql\":\"select last * from " + Nodes + " order by timeseries desc;\"}";
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage response = await client.PostAsync(GlobalContext.SystemConfig.IotDBURL, content);
                    response.EnsureSuccessStatusCode();
                    responseBody = await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException e)
                {

                }
            }
            TData<IotDBinfoLow> obj = new TData<IotDBinfoLow>();
            IotDBinfoLow jsonData = JsonConvert.DeserializeObject<IotDBinfoLow>(responseBody);
            obj.Tag = 1;
            obj.Data = jsonData;
            return obj;
        }



        /// <summary>
        /// check iotdb是否正常
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<object> IsIotDB()
        {
            bool isIotDB = false;
            bool isSimulator = false;
            bool isTwin = false;
            #region IoTDB连接状态.
            //接受返回数据字符串 IoTDB连接状态
            string responseBody = "";
            using (HttpClient client = new HttpClient())
            {
                // 添加Authorization头root.test_group.test_device
                client.DefaultRequestHeaders.Add("Authorization", "cm9vdDpyb290");
                var json = "{\"prefix_paths\":[\"root\",\"KNS\",\"AA5\",\"*5CEX004VL*\"]}";

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                try
                {
                    HttpResponseMessage response = await client.PostAsync(GlobalContext.SystemConfig.IotDBURLNew, content);
                    response.EnsureSuccessStatusCode();
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        isIotDB = true;
                    }
                }
                catch (HttpRequestException e)
                {
                    isIotDB = false;
                }
            }
            #endregion
            #region 模拟机连接状态
            //获取更新的模拟机时间
            TData<FSSocketEntity> obj = await socketBLL.GetEntity();
            TimeSpan? difference = obj.Data.BaseModifyTime - DateTime.Now;
            if (difference.HasValue && Math.Abs(difference.Value.TotalSeconds) < 10)
            {
                isSimulator = true;
            }
            #endregion
            var State = new
            {
                IsIotDB = isIotDB,
                IsSimulator = isSimulator,
                IsTwin = isTwin
            };
            return State;
        }




        /// <summary>
        /// 根据时间区间查询数据
        /// </summary>
        /// <param name="Node">点名(root.KNS.AA5.5DVD008ZVRP)</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="limit">最大数量</param>       
        /// <returns></returns>
        [HttpGet]
        public async Task<TData<IotDBinfo>> GetIOTDGet([FromQuery] string Node, [FromQuery] DateTime startTime, [FromQuery] DateTime endTime, int limit = 100)
        {
            TData<IotDBinfo> obj = new TData<IotDBinfo>();
            string responseBody = "";
            using (HttpClient client = new HttpClient())
            {
                // 添加Authorization头root.test_group.test_device
                client.DefaultRequestHeaders.Add(GlobalContext.SystemConfig.Authorization, GlobalContext.SystemConfig.AuthorizationValue);
                var json = "{\"sql\":\"select * from " + Node + " where time >" + startTime.ToString("yyyy-MM-dd HH:mm:ss") + " and time<" + endTime.ToString("yyyy-MM-dd HH:mm:ss") + " limit " + limit.ToString() + "\"}";
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                try
                {
                    HttpResponseMessage response = await client.PostAsync(GlobalContext.SystemConfig.IotDBURL, content);
                    response.EnsureSuccessStatusCode();
                    responseBody = await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException e)
                {
                    obj.Message = e.Message;
                }
            }
            IotDBinfo jsonData = JsonConvert.DeserializeObject<IotDBinfo>(responseBody);
            obj.Data = jsonData;
            return obj;
        }



    }
}