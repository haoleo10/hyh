using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using YiSha.Util;
using YiSha.Util.Model;
using YiSha.Entity;
using YiSha.Model;
using YiSha.Admin.Web.Controllers;
using YiSha.Entity.IntelligentSearchManage;
using YiSha.Business.IntelligentSearchManage;
using YiSha.Model.Param.IntelligentSearchManage;
using System.Net.Http;
using Newtonsoft.Json;
using YiSha.Service.IntelligentSearchManage;
using YiSha.Model.IotDBData;
using Microsoft.CodeAnalysis.Elfie.Model.Strings;
using static YiSha.Admin.Web.Areas.IntelligentSearchManage.Controllers.IntelligentSearchController;

namespace YiSha.Admin.Web.Areas.IntelligentSearchManage.Controllers
{


    public class SearchConditionConverter
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
    }

    /// <summary>
    /// 创 建：admin
    /// 日 期：2024-08-20 14:39
    /// 描 述：智能搜索控制器类
    /// </summary>
    [Area("IntelligentSearchManage")]
    public class IntelligentSearchController : BaseController
    {
        private IntelligentSearchBLL intelligentSearchBLL = new IntelligentSearchBLL();

        #region 视图功能

        public ActionResult IntelligentSearchEchartForm()
        {
            return View();
        }
        public ActionResult IntelligentSearchForm()
        {
            return View();
        }

        [AuthorizeFilter("intelligentsearch:intelligentsearch:view")]
        public ActionResult IntelligentSearchIndex()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<IntelligentSearchEntity> obj = await intelligentSearchBLL.GetEntity(id);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("intelligentsearch:intelligentsearch:search")]
        public async Task<ActionResult> GetListJson(IntelligentSearchListParam param)
        {
            TData<List<IntelligentSearchEntity>> obj = await intelligentSearchBLL.GetList(param);
            return Json(obj);

        }

        [HttpGet]
        [AuthorizeFilter("intelligentsearch:intelligentsearch:search")]
        public async Task<ActionResult> GetPageListJson(IntelligentSearchListParam param, Pagination pagination)
        {
            TData<List<IntelligentSearchEntity>> obj = await intelligentSearchBLL.GetPageList(param, pagination);       
            return Json(obj);
        }

        public class iotData
        {
            public long? Id { get; set; }
            public string Column_name { get; set; }

            public string Data_type { get; set; }

            public float? DataValue { get; set; }
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
        public async Task<ActionResult> GetPageListJsons(IntelligentSearchListParam param, Pagination pagination)
        {
            List<SearchCondition> result =new List<SearchCondition>();

            if (!string.IsNullOrEmpty(param.doneWorkName123))
            {
                result = SearchConditionConverter.ConvertQueryStringToList(param.doneWorkName123); 
            }
           
            List<iotData> iotDatas = new List<iotData>();

            // 定义接口参数
            var parameters = new Dictionary<string, string>
            {             
                 { "DoneWorkName", param.DoneWorkName },
                 { "StartTime",param.StartTime.ToString() },
                 { "EndTime", param.EndTime.ToString() },
                 { "ValueMin", param.ValueMin.ToString() },
                 { "ValueMax", param.ValueMax.ToString() },
                 { "State", param.State.ToString() },
                 {"rolesLis", param.doneWorkName123 }

            };
            string responseBody = "";
            // 构建查询字符串
            var queryParams = string.Join("&", parameters.Select(p =>
                $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value)}"));

            string url = "http://" + @GlobalContext.SystemConfig.WebApiIP + ":" + @GlobalContext.SystemConfig.WebApiPort + $"/IotDBData/GetIOTDBDatas?{queryParams}";

            using (var client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {                     
                        responseBody = await response.Content.ReadAsStringAsync();
                        if (!string.IsNullOrWhiteSpace(responseBody))
                        {
                            iotDatas = JsonConvert.DeserializeObject<List<iotData>>(responseBody);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"请求失败，状态码：{(int)response.StatusCode} {response.ReasonPhrase}");
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"网络请求异常：{ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"发生异常：{ex.Message}");
                }
            }
            if (!string.IsNullOrEmpty(responseBody))
            {
                TData<List<iotData>> responseBodyData = new TData<List<iotData>>();

                if (param.ValueMin != null && param.ValueMax != null)
                {
                    responseBodyData.Data = iotDatas.Select(p => new iotData { Id = p.Id, Column_name = p.Column_name, Data_type = p.Data_type, DataValue = p.DataValue }).Where(p => p.DataValue >= param.ValueMin && p.DataValue <= param.ValueMax).ToList();
                }
                else 
                {
                    responseBodyData.Data = iotDatas;
                }
                responseBodyData.Total = pagination.TotalCount;
                responseBodyData.Tag = 1;
                return Json(responseBodyData);

            }
            else return null;
        }

        [HttpGet]
        public async Task<ActionResult> GetEchartDataListJsons(string Device, string State)
        {
            DateTime now = DateTime.Now;
            DateTime sixMonthsAgo = now.AddMonths(-12);
            if (State == "0")
            {
                sixMonthsAgo= now.AddHours(-24);
            }
            if (State == "1")
            {
                sixMonthsAgo = now.AddDays(-7);

            }
            if (State == "2")
            {
                sixMonthsAgo = now.AddMonths(-1);
            }
            if (State == "3")
            { 
                sixMonthsAgo = now.AddYears(-1);
            }
            if (State == "4")
            { 
                sixMonthsAgo = now.AddYears(-2);

            }

            // 计算6个月前的时间

            IotDBinfoLow iotDatas = new IotDBinfoLow();

            // 定义接口参数
            var parameters = new Dictionary<string, string>
            {
                 { "DoneWorkName", Device },
                 { "StartTime",sixMonthsAgo.ToString() },
                 { "EndTime", now.ToString() },
                 { "State", "1"},
            };
            string responseBody = "";
            // 构建查询字符串
            var queryParams = string.Join("&", parameters.Select(p =>
                $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value)}"));

            string url ="http://" + @GlobalContext.SystemConfig.WebApiIP + ":" + @GlobalContext.SystemConfig.WebApiPort + $"/IotDBData/GetIOTDBDataSearch?{queryParams}";

            using (var client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        responseBody = await response.Content.ReadAsStringAsync();
                        if (!string.IsNullOrWhiteSpace(responseBody))
                        {
                            iotDatas = JsonConvert.DeserializeObject<IotDBinfoLow>(responseBody);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"请求失败，状态码：{(int)response.StatusCode} {response.ReasonPhrase}");
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"网络请求异常：{ex.Message}");
                }
            }
            List<string> strings = new List<string>();
            if (iotDatas.values[0].Count > 0)
            {
                foreach (var item in iotDatas.values[0])
                {
                    strings.Add(Device);
                }
                iotDatas.column_names = strings;
            }
           
         
            return Json(iotDatas);

        }

        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("intelligentsearch:intelligentsearch:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await intelligentSearchBLL.DeleteForm(ids);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("intelligentsearch:intelligentsearch:add,intelligentsearch:intelligentsearch:edit")]
        public async Task<ActionResult> SaveFormJson(IntelligentSearchEntity entity)
        {
            TData<string> obj = await intelligentSearchBLL.SaveForm(entity);
            return Json(obj);
        }
        #endregion
    }
}
