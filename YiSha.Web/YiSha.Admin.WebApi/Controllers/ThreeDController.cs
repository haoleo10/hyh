using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NPOI.POIFS.Crypt.Dsig;
using NPOI.SS.Formula.Functions;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using YiSha.Entity.SystemManage;
using YiSha.Entity.ThreeDimensionsManage;
using YiSha.Model.IotDBData;
using YiSha.Service.SystemManage;
using YiSha.Service.ThreeDimensionsManage;
using YiSha.Util;
using YiSha.Util.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace YiSha.Admin.WebApi.Controllers
{

    /// <summary>
    /// 三维接口
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    [AuthorizeFilter]
    public class ThreeDController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ThreeDController> _logger;


        public ThreeDController(IHttpClientFactory httpClientFactory, ILogger<ThreeDController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        /// <summary>
        /// 二层左边数据列表 NEW
        /// </summary>
        /// <param name="SystemName"></param>
        /// <param name="DataType"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="Model"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<DeviceMatchResult>> GetIOTDBThreeKNSAnd3K(
        [FromQuery] string SystemName,
        [FromQuery] string DataType,
        [FromQuery] string StartTime,
        [FromQuery] string EndTime,
        [FromQuery] string Model)
        {
            try
            {
                // 1. 参数校验
                if (string.IsNullOrWhiteSpace(SystemName))
                    return new List<DeviceMatchResult>(); // 或抛出验证异常

                // 2. 构建查询（避免字符串拼接注入，此处仅为示例）
                var json = BuildJsonQuery(SystemName, DataType, StartTime, EndTime);

                // 3. 获取 HttpClient
                var client = _httpClientFactory.CreateClient("IotDBClient");

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var url = GlobalContext.SystemConfig.IotDBURL;

                // 4. 发送请求（使用 CancellationToken 支持超时）
                using var cts = CancellationTokenSource.CreateLinkedTokenSource(
                    CancellationToken.None,
                    HttpContext?.RequestAborted ?? CancellationToken.None);
                cts.CancelAfter(TimeSpan.FromSeconds(30)); // 双重超时保护

                var response = await client.PostAsync(url, content, cts.Token);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync(cts.Token);

                if (string.IsNullOrEmpty(responseBody))
                    return new List<DeviceMatchResult>();

                var jsonData = JsonConvert.DeserializeObject<IotDBinfoLow>(responseBody);
                if (jsonData == null)
                    return new List<DeviceMatchResult>();

                // 5. 匹配逻辑
                if (Model == "M" && DataType == "0")
                {
                    return Model == "M"
                   ? MatchFssAndReactor(jsonData, 'M')
                   : MatchFssAndReactor(jsonData, 'S');

                }
                else
                {
                    return Model == "M"
                   ? MatchFssAndReactor(jsonData, 'M', true)
                   : MatchFssAndReactor(jsonData, 'S', true);

                }

            }
            catch (OperationCanceledException) when (HttpContext?.RequestAborted.IsCancellationRequested == true)
            {
                _logger.LogWarning("请求被客户端取消，SystemName: {SystemName}", SystemName);
                return new List<DeviceMatchResult>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "查询 IoTDB 失败，SystemName: {SystemName}", SystemName);
                return new List<DeviceMatchResult>();  // 返回空集合，避免调用方 NullReference
            }
        }

        private string BuildJsonQuery(string systemName, string dataType, string startTime, string endTime)
        {
            if (dataType == "0")
            {
                return $"{{\"sql\":\"select last* value from root.KNS.AA5.*{systemName}*,root.FSS.AA5.*{systemName}*\" }}";
            }
            else
            {
                // 时间参数需要加引号（IoTDB SQL 要求字符串常量用单引号）
                var start = DateTime.TryParse(startTime, out var s) ? s.ToString("yyyy-MM-dd HH:mm:ss") : "1970-01-01 00:00:00";
                var end = DateTime.TryParse(endTime, out var e) ? e.ToString("yyyy-MM-dd HH:mm:ss") : "2038-01-01 00:00:00";
                return $"{{\"sql\":\"select * from root.KNS.AA5.*{systemName}*,root.FSS.AA5.*{systemName}* where time = {start} limit 9000\" }}";
            }
        }


        /// <summary>
        /// 二层左边数据列表
        /// </summary>
        /// <param name="SystemName">系统名字</param>
        /// <param name="DataType">数据类型（0 实时 1历史）</param>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<DeviceMatchResult>> GetIOTDBThreeKNSAnd3K1([FromQuery] string SystemName, [FromQuery] string DataType, [FromQuery] string StartTime, [FromQuery] string EndTime, [FromQuery] string Model)
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
                if (DataType == "0")
                {
                    json = "{\"sql\":\"select last* value from root.KNS.AA5.*" + SystemName + "*,root.FSS.AA5.*" + SystemName + "*\" }";
                }
                else
                {
                    json = "{\"sql\":\"select * value from root.KNS.AA5.*" + SystemName + "*,root.FSS.AA5.*" + SystemName + "* where time=" + DateTime.Parse(StartTime).ToString("yyyy-MM-dd HH:mm:ss") + " limit 40000\" }";

                }

                //定义query接口
                var url = GlobalContext.SystemConfig.IotDBURL;

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
            if (jsonData != null)
            {
                if (DataType == "1")
                {
                    if (Model == "M")
                    {
                        List<DeviceMatchResult> deviceMatchResults = MatchFssAndReactor(jsonData, 'M', true);
                        return deviceMatchResults;
                    }
                    else
                    {
                        List<DeviceMatchResult> deviceMatchResults = MatchFssAndReactor(jsonData, 'S', true);
                        return deviceMatchResults;
                    }

                }
                else
                {
                    if (Model == "M")
                    {
                        List<DeviceMatchResult> deviceMatchResults = MatchFssAndReactor(jsonData, 'M');
                        return deviceMatchResults;
                    }
                    else
                    {
                        List<DeviceMatchResult> deviceMatchResults = MatchFssAndReactor(jsonData, 'S');
                        return deviceMatchResults;
                    }

                }



            }
            return null;
        }


        // 配对结果
        public class DeviceMatchResult
        {

            public DateTime? Timestamp { get; set; }
            /// <summary>
            /// 设备编号 5ABP020SN
            /// </summary>
            public string DeviceCode { get; set; }

            public string FSS_Path { get; set; }
            public string FSS_Value { get; set; }

            public string Reactor_Path { get; set; }
            public string Reactor_Value { get; set; }

            public bool? IsAlert { get; set; }
            public int? WarningValue  { get; set; }

        }

        public static List<DeviceMatchResult> MatchFssAndReactor(IotDBinfoLow data, char Model, bool ishistory = false, bool includeSingleSide = false)
        {
            var result = new List<DeviceMatchResult>();

            if (ishistory)
            {

                var paths = data.expressions;          // List<string>
                var values = data.values;              // List<List<object>> 或 List<List<string>>
                var timestamps = data.timestamps;      // List<long> 毫秒级 Unix 时间戳

                if (paths == null || values == null || timestamps == null)
                    return null;

                if (paths.Count != values.Count)
                    return null;

                int timeCount = timestamps.Count;
                for (int t = 0; t < timeCount; t++)
                {
                    var fssDic = new Dictionary<string, (string path, double value)>();
                    var reactorDic = new Dictionary<string, (string path, double value)>();

                    for (int i = 0; i < paths.Count; i++)
                    {
                        var path = paths[i];
                        var valueList = values[i];

                        if (valueList == null || valueList.Count <= t)
                            continue;

                        var raw = valueList[t]?.ToString();
                        if (string.IsNullOrEmpty(raw))
                            ;

                        if (!double.TryParse(raw,
                                             System.Globalization.NumberStyles.Any,
                                             System.Globalization.CultureInfo.InvariantCulture,
                                             out double parsedValue))
                            ;

                        var code = ExtractDeviceCode(path);
                        if (string.IsNullOrEmpty(code))
                            ;

                        if (path.StartsWith("root.FSS", StringComparison.OrdinalIgnoreCase))
                            fssDic[code] = (path, parsedValue);
                        else if (path.StartsWith("root.KNS", StringComparison.OrdinalIgnoreCase))
                            reactorDic[code] = (path, parsedValue);
                    }

                    foreach (var code in reactorDic.Keys)
                    {
                        if (reactorDic.TryGetValue(code, out var reactorInfo))
                        {
                            if (!char.IsWhiteSpace(Model))
                            {
                                if (code.Length < 2 || char.ToUpper(code[^2]) != Model)
                                    continue;
                            }

                            // ★ 将 long 时间戳转换为 DateTime（这里使用 UTC 时间，可根据需求调整）
                            DateTime timestampDateTime = DateTimeOffset.FromUnixTimeMilliseconds(timestamps[t]).UtcDateTime;
                            try
                            {
                                result.Add(new DeviceMatchResult
                                {
                                    Timestamp = timestampDateTime,        // 现在为 DateTime 类型
                                    DeviceCode = code,
                                    FSS_Path = fssDic[code].path,
                                    FSS_Value = fssDic[code].value.ToString("F3"),
                                    Reactor_Path = reactorInfo.path,
                                    Reactor_Value = reactorInfo.value.ToString("F3")
                                });
                            }
                            catch (Exception ex)
                            {

                                var aa = "aa";
                            }

                        }
                    }
                }

            }
            else
            {
                // 取出路径、数值
                var paths = data.values[0];
                var values = data.values[1];

                // 字典：key=设备编号，value=(路径,值)
                var fssDic = new Dictionary<string, (string path, string value)>();
                var reactorDic = new Dictionary<string, (string path, string value)>();

                for (int i = 0; i < paths.Count; i++)
                {
                    var path = paths[i];
                    var value = values[i];
                    var code = ExtractDeviceCode(path);

                    if (string.IsNullOrEmpty(code)) continue;

                    // 分类存入 FSS / Reactor
                    if (path.StartsWith("root.FSS"))
                    {
                        fssDic[code] = (path, value);
                    }
                    else if (path.StartsWith("root.KNS"))
                    {
                        reactorDic[code] = (path, value);
                    }
                }

                // 配对：相同编号必须同时存在 FSS 和 Reactor
                foreach (var code in fssDic.Keys)
                {
                    if (reactorDic.TryGetValue(code, out var reactorInfo))
                    {
                        if (!char.IsWhiteSpace(Model))
                        {
                            if (code.Length < 2 || char.ToUpper(code[^2]) != Model)
                                continue;

                        }                   
                        result.Add(new DeviceMatchResult
                        {
                            DeviceCode = code,
                            FSS_Path = fssDic[code].path,
                            FSS_Value = double.Parse(fssDic[code].value).ToString("F3"),
                            Reactor_Path = reactorInfo.path,
                            Reactor_Value = double.Parse(reactorInfo.value).ToString("F3")
                        });
                    }
                    else if (includeSingleSide)
                    {
                        // 只有 FSS 单边数据：KNS 侧置 null
                        if (!char.IsWhiteSpace(Model))
                        {
                            if (code.Length < 2 || char.ToUpper(code[^2]) != Model)
                                continue;
                        }
                        result.Add(new DeviceMatchResult
                        {
                            DeviceCode = code,
                            FSS_Path = fssDic[code].path,
                            FSS_Value = double.Parse(fssDic[code].value).ToString("F3"),
                            Reactor_Path = null,
                            Reactor_Value = null
                        });
                    }
                }

                if (includeSingleSide)
                {
                    // 只有 KNS 单边数据：FSS 侧置 null
                    foreach (var code in reactorDic.Keys)
                    {
                        if (fssDic.ContainsKey(code)) continue;
                        if (!char.IsWhiteSpace(Model))
                        {
                            if (code.Length < 2 || char.ToUpper(code[^2]) != Model)
                                continue;
                        }
                        result.Add(new DeviceMatchResult
                        {
                            DeviceCode = code,
                            FSS_Path = null,
                            FSS_Value = null,
                            Reactor_Path = reactorDic[code].path,
                            Reactor_Value = double.Parse(reactorDic[code].value).ToString("F3")
                        });
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 从路径中提取设备编号：5ABP020SN
        /// </summary>
        private static string ExtractDeviceCode(string path)
        {
            try
            {
                // 切割路径，取倒数第二段：HYH_AA5_5ABP020SN
                var lastPart = path.Split('.').Reverse().Skip(1).First();
                // 按下划线切割，取最后一段就是编号
                return lastPart.Split('_').Last();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// KNS原始数据
        /// </summary>
        /// <param name="SystemName">系统名</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IotDBinfo> GetIOTDBThreeKNSSyetem([FromQuery] string SystemName)
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
            return data;
        }

        /// <summary>
        /// 3k
        /// </summary>
        /// <param name="SystemName">系统名</param>
        /// <returns></returns>
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
                var json = "{\"prefix_paths\":[\"root\",\"FSS\",\"AA5\",\"*" + SystemName + "*\"]}";

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
        /// 数据清洗后传到iotdb
        /// </summary>
        /// <param name="SystemName">系统名</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IotDBinfo> GetIOTDBThreeQXSyetem([FromQuery] string SystemName)
        {
            TData<IotDBinfo> obj = new TData<IotDBinfo>();
            string responseBody = "";
            using (HttpClient client = new HttpClient())
            {
                // 添加Authorization头root.test_group.test_device
                client.DefaultRequestHeaders.Add("Authorization", "cm9vdDpyb290");
                // var json = "{\"prefix_paths\":[\"root\",\"KNS\",\"AA5\",\"*" + SystemName + "*\"]}";
                var json = "{\"prefix_paths\":[\"root\",\"DATACL\",\"*" + SystemName + "*\"]}";


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
        /// EBsilon
        /// </summary>
        /// <param name="SystemName">系统名</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IotDBinfo> GetIOTDBThreeEBSyetem([FromQuery] string SystemName)
        {
            TData<IotDBinfo> obj = new TData<IotDBinfo>();
            string responseBody = "";
            using (HttpClient client = new HttpClient())
            {
                // 添加Authorization头root.test_group.test_device
                client.DefaultRequestHeaders.Add("Authorization", "cm9vdDpyb290");

                // var json = "{\"prefix_paths\":[\"root\",\"KNS\",\"AA5\",\"*" + SystemName + "*\"]}";
                var json = "{\"prefix_paths\":[\"root\",\"Ebsilon\",\"AA5\",\"*" + SystemName + "*\"]}";
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
        /// EBsilon 实时值，历史值接口
        /// </summary>
        /// <param name="PointName">点名</param>
        /// <param name="DataType">0;实时值,1：历史值</param>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IotDBinfoLow> GetEbsilon([FromQuery] string PointName, [FromQuery] string DataType, [FromQuery] string StartTime, [FromQuery] string EndTime)
        {
            //接受返回数据字符串
            string responseBody = "";

            //接受错误信息
            string error = "";
            string[] letters = PointName.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
            string point = "";
            for (int i = 0; i < letters.Length; i++)
            {
                string letter = letters[i];
                point = point + "root.Ebsilon.AA5.*" + letter + "*";

                if (i != letters.Length - 1) point += ",";
            }

            using (HttpClient client = new HttpClient())
            {

                // 添加Authorization头root.test_group.test_device
                client.DefaultRequestHeaders.Add("Authorization", "cm9vdDpyb290");
                //定义查询语句 
                var json = "";
                if (DataType == "0")
                {
                    json = "{\"sql\":\"select last* value from " + point + "\" }";

                }
                else
                {
                    json = "{\"sql\":\"select last* value from " + point + " where time >" + DateTime.Parse(StartTime).ToString("yyyy-MM-dd HH:mm:ss") + " and time< " + DateTime.Parse(EndTime).ToString("yyyy-MM-dd HH:mm:ss") + " limit 40000\" }";

                }
                var url = GlobalContext.SystemConfig.IotDBURL;

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
        /// 三维传到iotdb
        /// </summary>
        /// <param name="SystemName"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IotDBinfo> GetIOTDBThreeDataSyetem([FromQuery] string SystemName)
        {
            TData<IotDBinfo> obj = new TData<IotDBinfo>();
            string responseBody = "";
            using (HttpClient client = new HttpClient())
            {
                // 添加Authorization头root.test_group.test_device
                client.DefaultRequestHeaders.Add("Authorization", "cm9vdDpyb290");
                // var json = "{\"prefix_paths\":[\"root\",\"KNS\",\"AA5\",\"*" + SystemName + "*\"]}";
                var json = "{\"prefix_paths\":[\"root\",\"Vis\",\"*" + SystemName + "*\"]}";


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

        private ParamAlertsService paramAlertsService = new ParamAlertsService();


        /// <summary>
        ///  报警自己添加接口
        /// </summary>
        /// <param name="Param_Key"></param>
        /// <param name="Param_Value1"></param>
        /// <param name="Param_Value2"></param>
        /// <param name="Source_SYS"></param>
        /// <param name="AlarmLayer"></param>
        [HttpGet]
        public async void AlertBJ([FromQuery] string Param_Key, [FromQuery] string Param_Value1, [FromQuery] string Param_Value2, [FromQuery] string Source_SYS, [FromQuery] string AlarmLayer ,[FromQuery] string DataType)
        {
            List<ParamAlertsEntity> paramAlertsEntities = new List<ParamAlertsEntity>();
            try
            {
                paramAlertsEntities = await paramAlertsService.GetLists(AlarmLayer, Source_SYS);

            }
            catch (Exception ex)
            {

                throw;
            }


            if (paramAlertsEntities.Count == 0)
            {
                ParamAlertsEntity entity = new ParamAlertsEntity();
                entity.Name = "警报参数：" + entity.Param_Key;
                entity.Param_Key = Param_Key;
                entity.Param_Value = Param_Value1;
                entity.Param_Value1 = Param_Value2;

                entity.Source_SYS = Source_SYS;
                entity.AlarmLayer = int.Parse(AlarmLayer);
                await paramAlertsService.SaveForm(entity);
                return;
            }
            if (DataType == "1")
            {
                DateTime now = DateTime.Now;
                // 半小时前的时间节点
                DateTime halfHourAgo = now.AddMinutes(5);
                // LINQ 筛选（满足两个条件：Name严格等于ABP，时间大于等于半小时前）
                var filterList = paramAlertsEntities
                    .Where(p => p.Param_Key == Param_Key
                                && p.BaseCreateTime >= halfHourAgo
                                && p.BaseCreateTime <= now)
                    .ToList();

                if (filterList.Count > 0)
                {
                    return;
                }
                else
                {
                    ParamAlertsEntity entity = new ParamAlertsEntity();
                    entity.Param_Key = Param_Key;
                    entity.Param_Value = Param_Value1;
                    entity.Param_Value1 = Param_Value2;

                    entity.Source_SYS = Source_SYS;
                    entity.AlarmLayer = int.Parse(AlarmLayer);
                    entity.Remark = "快速演变";
                    await paramAlertsService.SaveForm(entity);
                }
            }
            else
            {
                DateTime now = DateTime.Now;
                // 半小时前的时间节点
                DateTime halfHourAgo = now.AddMinutes(-30);
                // LINQ 筛选（满足两个条件：Name严格等于ABP，时间大于等于半小时前）
                var filterList = paramAlertsEntities
                    .Where(p => p.Param_Key == Param_Key
                                && p.BaseCreateTime >= halfHourAgo
                                && p.BaseCreateTime <= now)
                    .ToList();

                if (filterList.Count > 0)
                {
                    return;
                }
                else
                {
                    ParamAlertsEntity entity = new ParamAlertsEntity();
                    entity.Param_Key = Param_Key;
                    entity.Param_Value = Param_Value1;
                    entity.Param_Value1 = Param_Value2;

                    entity.Source_SYS = Source_SYS;
                    entity.AlarmLayer = int.Parse(AlarmLayer);
                    await paramAlertsService.SaveForm(entity);
                }

            }

           
        }

        /// <summary>
        /// 警报数
        /// </summary>
        /// <param name="AlarmLayer">层级</param>
        /// <param name="SYS">系统名</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<int> AlertBJNum(string AlarmLayer, string SYS)
        {
            List<ParamAlertsEntity> paramAlertsEntities = new List<ParamAlertsEntity>();
            paramAlertsEntities = await paramAlertsService.GetLists(AlarmLayer, SYS);
            int num = paramAlertsEntities.Count;
            return num;
        }


        /// <summary>
        /// 点名
        /// </summary>
        /// <param name="PointName"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<DeviceMatchResult>> KNSAndFSSSinglePoint([FromQuery] string PointName)
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

                json = "{\"sql\":\"select last* value from root.KNS.AA5.*" + PointName + "*,root.FSS.AA5.*" + PointName + "*\" }";

                //定义query接口
                var url = GlobalContext.SystemConfig.IotDBURL;

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
            if (jsonData != null)
            {
                List<DeviceMatchResult> deviceMatchResults = MatchFssAndReactor(jsonData, 'M');
                return deviceMatchResults;

            }
            else
            {
                return null;
            }

        }


        /// <summary>
        /// 状态对比分析（右边界面）NEW
        /// </summary>
        /// <param name="PointName">点名（用逗号分隔）</param>
        /// <param name="DataType">数据类型（0 实时数据 1历史数据）</param>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<DeviceMatchResult>> RightView(
        [FromQuery] string PointName,
        [FromQuery] string DataType,
        [FromQuery] string StartTime,
        [FromQuery] string EndTime)
        {
            // 1. 参数校验
            if (string.IsNullOrWhiteSpace(PointName))
            {
                _logger.LogWarning("PointName 为空");
                return new List<DeviceMatchResult>();
            }

            try
            {
                // 2. 构建 IoTDB 路径（支持中英文逗号分隔）
                var pointPaths = BuildPointPaths(PointName);
                if (string.IsNullOrEmpty(pointPaths))
                    return new List<DeviceMatchResult>();

                // 3. 构建 JSON 查询语句
                var json = BuildQueryJson(pointPaths, DataType, StartTime, EndTime);

                // 4. 从工厂获取 HttpClient（不要 using 释放）
                var client = _httpClientFactory.CreateClient("IotDBClient");
                var url = GlobalContext.SystemConfig.IotDBURL;
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // 5. 支持取消令牌（前端关闭连接时自动中止）
                using var cts = CancellationTokenSource.CreateLinkedTokenSource(
                    CancellationToken.None,
                    HttpContext?.RequestAborted ?? CancellationToken.None);
                cts.CancelAfter(TimeSpan.FromSeconds(30));   // 双重超时保护

                var response = await client.PostAsync(url, content, cts.Token);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync(cts.Token);

                // 6. 反序列化
                if (string.IsNullOrEmpty(responseBody))
                    return new List<DeviceMatchResult>();

                var jsonData = JsonConvert.DeserializeObject<IotDBinfoLow>(responseBody);
                if (jsonData == null)
                    return new List<DeviceMatchResult>();

                // 7. 匹配逻辑（无 Model 参数，默认使用空格字符？根据原代码传入 ' '）
                var matches = DataType == "0"
                    ? MatchFssAndReactor(jsonData, ' ')
                    : MatchFssAndReactor(jsonData, ' ', true);

                // 8. KNS 值为负数时置为 0 再参与后续对比
                if (matches != null)
                {
                    foreach (var item in matches)
                    {
                        if (double.TryParse(item.Reactor_Value,
                                            System.Globalization.NumberStyles.Any,
                                            System.Globalization.CultureInfo.InvariantCulture,
                                            out double knsValue) && knsValue < 0)
                        {
                            item.Reactor_Value = 0d.ToString("F3");
                        }
                    }
                }
                return matches ?? new List<DeviceMatchResult>();
            }
            catch (OperationCanceledException) when (HttpContext?.RequestAborted.IsCancellationRequested == true)
            {
                _logger.LogWarning("客户端取消请求，PointName: {PointName}", PointName);
                return new List<DeviceMatchResult>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "查询 IoTDB 失败，PointName: {PointName}", PointName);
                return new List<DeviceMatchResult>();   // 返回空集合，避免 NullReference
            }
        }

        // ---------- 辅助方法（可抽取到静态工具类） ----------

        private string BuildPointPaths(string pointName)
        {
            var letters = pointName.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
            if (letters.Length == 0) return string.Empty;

            // 使用 StringBuilder 提升性能
            var sb = new StringBuilder();
            for (int i = 0; i < letters.Length; i++)
            {
                // 对每个点进行简单清理（防止注入，仅允许字母数字下划线）
                var clean = Regex.Replace(letters[i].Trim(), @"[^a-zA-Z0-9_]", "");
                if (string.IsNullOrEmpty(clean)) continue;

                sb.Append("root.KNS.AA5.*")
                  .Append(clean).Append("*,")
                  .Append("root.FSS.AA5.*").Append(clean).Append("*");

                if (i != letters.Length - 1) sb.Append(',');
            }
            return sb.ToString();
        }

        private string BuildQueryJson(string pointPaths, string dataType, string startTime, string endTime)
        {
            if (dataType == "0")
            {
                return $"{{\"sql\":\"select last* value from {pointPaths}\" }}";
            }
            else
            {
                // 时间解析与格式化
                var start = DateTime.TryParse(startTime, out var s)
                    ? s.ToString("yyyy-MM-dd HH:mm:ss")
                    : "1970-01-01 00:00:00";
                var end = DateTime.TryParse(endTime, out var e)
                    ? e.ToString("yyyy-MM-dd HH:mm:ss")
                    : "2038-01-01 00:00:00";

                return $"{{\"sql\":\"select * from {pointPaths} where time = {start} limit 9000\" }}";
            }
        }







        /// <summary>
        /// 状态对比分析（右边界面）
        /// </summary>
        /// <param name="PointName">点名（用逗号分隔）</param>
        /// <param name="DataType">数据类型（0 实时数据 1历史数据）</param>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<DeviceMatchResult>> RightView1([FromQuery] string PointName, [FromQuery] string DataType, [FromQuery] string StartTime, [FromQuery] string EndTime)
        {
            //接受返回数据字符串
            string responseBody = "";

            //接受错误信息
            string error = "";
            string[] letters = PointName.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
            string point = "";
            for (int i = 0; i < letters.Length; i++)
            {
                string letter = letters[i];
                point = point + "root.KNS.AA5.*" + letter + "*," + "root.FSS.AA5.*" + letter + "*";

                if (i != letters.Length - 1) point += ",";
            }

            using (HttpClient client = new HttpClient())
            {

                // 添加Authorization头root.test_group.test_device
                client.DefaultRequestHeaders.Add("Authorization", "cm9vdDpyb290");
                //定义查询语句 
                var json = "";
                if (DataType == "0")
                {
                    json = "{\"sql\":\"select last* value from " + point + "\" }";

                }
                else
                {
                    json = "{\"sql\":\"select last* value from " + point + " where time >" + DateTime.Parse(StartTime).ToString("yyyy-MM-dd HH:mm:ss") + " and time< " + DateTime.Parse(EndTime).ToString("yyyy-MM-dd HH:mm:ss") + " limit 9000\" }";

                }
                var url = GlobalContext.SystemConfig.IotDBURL;

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
            if (jsonData != null)
            {
                List<DeviceMatchResult> deviceMatchResults = MatchFssAndReactor(jsonData, ' ');
                return deviceMatchResults;
            }
            return null;
        }

        /// <summary>
        /// 登录日志
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<LogLoginEntity> sysloglogin()
        {

            LogLoginEntity logLoginEntity = new LogLoginEntity();
            LogLoginService logLoginService = new LogLoginService();
            logLoginEntity = await logLoginService.GetEntityNew();
            return logLoginEntity;
        }

        /// <summary>
        ///  三层右边View
        /// </summary>
        /// <param name="PointNameA"></param>
        /// <param name="PointNameB"></param>
        /// <param name="DataType"></param>
        /// <param name="StartTimeA"></param>
        /// <param name="StartTimeB"></param>
        /// <param name="StateA"></param>
        /// <param name="StateB"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<DeviceMatchResult>> ThreeRightView([FromQuery] string PointNameA, [FromQuery] string PointNameB, [FromQuery] string DataType, [FromQuery] string StartTimeA, [FromQuery] string StartTimeB, [FromQuery] string StateA, [FromQuery] string StateB)
        {
            //接受返回数据字符串
            string responseBodyA = "";
            string responseBodyB = "";

            //接受错误信息
            string error = "";
            string[] letterA = PointNameA.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
            string[] letterB = PointNameB.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
            string pointA = "";
            for (int i = 0; i < letterA.Length; i++)
            {
                string letter = letterA[i];
                if (StateA == "0")
                {
                    pointA = pointA + "root.KNS.AA5.*" + letter;
                }
                else
                {
                    pointA = pointA + "root.FSS.AA5.*" + letter;
                }

                if (i != letterA.Length - 1) pointA += ",";
            }

            string pointB = "";
            for (int i = 0; i < letterB.Length; i++)
            {
                string letter = letterB[i];

                if (StateB == "0")
                {
                    pointB = pointB + "root.KNS.AA5.*" + letter;
                }
                else
                {
                    pointB = pointB + "root.FSS.AA5.*" + letter;
                }
                if (i != letterB.Length - 1) pointB += ",";
            }

            using (HttpClient client = new HttpClient())
            {

                // 添加Authorization头root.test_group.test_device
                client.DefaultRequestHeaders.Add("Authorization", "cm9vdDpyb290");
                //定义查询语句 
                var jsonA = "";
                var jsonB = "";

                if (DataType == "0")
                {
                    jsonA = "{\"sql\":\"select last* value from " + pointA + "\" }";
                    jsonB = "{\"sql\":\"select last* value from " + pointB + "\" }";


                }
                else
                {
                    jsonA = "{\"sql\":\"select *  from " + pointA + " where time =" + DateTime.Parse(StartTimeA).ToString("yyyy-MM-dd HH:mm:ss") + " limit 40000\" }";
                    jsonB = "{\"sql\":\"select *  from " + pointB + " where time =" + DateTime.Parse(StartTimeB).ToString("yyyy-MM-dd HH:mm:ss") + " limit 40000\" }";


                }

                var url = GlobalContext.SystemConfig.IotDBURL;
                var contentA = new StringContent(jsonA, Encoding.UTF8, "application/json");
                var contentB = new StringContent(jsonB, Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage responseA = await client.PostAsync(url, contentA);
                    responseA.EnsureSuccessStatusCode();
                    responseBodyA = await responseA.Content.ReadAsStringAsync();

                    HttpResponseMessage responseB = await client.PostAsync(url, contentB);
                    responseB.EnsureSuccessStatusCode();
                    responseBodyB = await responseB.Content.ReadAsStringAsync();


                }
                catch (HttpRequestException e)
                {
                    error = e.Message;
                }
            }

            IotDBinfoLow jsonDataA = JsonConvert.DeserializeObject<IotDBinfoLow>(responseBodyA);
            IotDBinfoLow jsonDataB = JsonConvert.DeserializeObject<IotDBinfoLow>(responseBodyB);
            if (DataType == "0")
            {
                List<DeviceMatchResult> deviceMatchResultsA = MatchFssAndReactorThree(jsonDataA);
                List<DeviceMatchResult> deviceMatchResultsB = MatchFssAndReactorThree(jsonDataB);

                for (int i = 0; i < deviceMatchResultsA.Count; i++)
                {
                    deviceMatchResultsA[i].Reactor_Path = deviceMatchResultsB[i].FSS_Path;
                    deviceMatchResultsA[i].Reactor_Value = deviceMatchResultsB[i].FSS_Value;
                }

                return deviceMatchResultsA;

            }
            else
            {
                List<DeviceMatchResult> deviceMatchResultsA = MatchFssAndReactorThree(jsonDataA, true);
                List<DeviceMatchResult> deviceMatchResultsB = MatchFssAndReactorThree(jsonDataB, true);

                for (int i = 0; i < deviceMatchResultsA.Count; i++)
                {
                    deviceMatchResultsA[i].Reactor_Path = deviceMatchResultsB[i].FSS_Path;
                    deviceMatchResultsA[i].Reactor_Value = deviceMatchResultsB[i].FSS_Value;
                }
                return deviceMatchResultsA;
            }

        }


        public static List<DeviceMatchResult> MatchFssAndReactorThree(IotDBinfoLow dataA, bool ishot = false)
        {
            var result = new List<DeviceMatchResult>();

            if (ishot)
            {
                // 取出路径、数值
                var pathsA = dataA.expressions;
                var valuesA = dataA.values;

                var reactorDicA = new Dictionary<string, (string path, string value)>();

                for (int i = 0; i < pathsA.Count; i++)
                {
                    var pathA = pathsA[i];
                    var valueA = valuesA[i];
                    var codeA = ExtractDeviceCode(pathA);
                    if (string.IsNullOrEmpty(codeA)) continue;
                    reactorDicA[codeA] = (pathA, valueA[i]);
                }
                foreach (var key in reactorDicA.Keys)
                {
                    result.Add(new DeviceMatchResult
                    {
                        DeviceCode = key,
                        FSS_Path = reactorDicA[key].path,
                        FSS_Value = double.Parse(reactorDicA[key].value).ToString("F3"),
                    });
                }

            }
            else
            {
                // 取出路径、数值
                var pathsA = dataA.values[0];
                var valuesA = dataA.values[1];

                var reactorDicA = new Dictionary<string, (string path, string value)>();

                for (int i = 0; i < pathsA.Count; i++)
                {
                    var pathA = pathsA[i];
                    var valueA = valuesA[i];
                    var codeA = ExtractDeviceCode(pathA);
                    if (string.IsNullOrEmpty(codeA)) continue;
                    reactorDicA[codeA] = (pathA, valueA);
                }
                foreach (var key in reactorDicA.Keys)
                {
                    result.Add(new DeviceMatchResult
                    {
                        DeviceCode = key,
                        FSS_Path = reactorDicA[key].path,
                        FSS_Value = double.Parse(reactorDicA[key].value).ToString("F3"),
                    });
                }


            }
            return result;
        }


        // [ApiExplorerSettings(IgnoreApi = true)] // 让Swagger忽略这个接口
        /// <summary>
        ///  影子云接口
        /// </summary>
        /// <param name="Source">来源(0:KNS;1:3K）</param>
        /// <param name="PointName">点名（5ABP027SN）</param>
        /// <param name="StartTime">开始时间（2026-04-01 15:20:52）</param>
        /// <param name="EndTime">结束时间（2026-04-30 15:20:52）</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IotDBinfoLow> ShadowCloudData([FromQuery] string Source, [FromQuery] string PointName, [FromQuery] string StartTime, [FromQuery] string EndTime)
        {

            string[] letterA = PointName.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
            string pointA = "";
            for (int i = 0; i < letterA.Length; i++)
            {
                string letter = letterA[i];
                if (Source == "0")
                {
                    pointA = pointA + "root.KNS.AA5.*" + letter;
                }
                else
                {
                    pointA = pointA + "root.FSS.AA5.*" + letter;
                }

                if (i != letterA.Length - 1) pointA += ",";
            }
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
                json = "{\"sql\":\"SELECT * FROM " + pointA + " where time >" + DateTime.Parse(StartTime).ToString("yyyy-MM-dd HH:mm:ss") + " and time<" + DateTime.Parse(EndTime).ToString("yyyy-MM-dd HH:mm:ss") + " limit 40000\" }";


                //定义query接口
                var url = GlobalContext.SystemConfig.IotDBURL;
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
        /// 三层小窗口
        /// </summary>
        /// <param name="PointName">点名（多个点名用逗号分隔）</param>
        /// <param name="Time">时间（2026-05-11 08:11:47）</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<DeviceMatchResult>> ThreeSmallWindow([FromQuery] string PointName, [FromQuery] string Time)
        {
            string[] letters = PointName.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
            string point = "";
            for (int i = 0; i < letters.Length; i++)
            {
                string letter = letters[i];
                point = point + "root.KNS.AA5.*" + letter + "*," + "root.FSS.AA5.*" + letter + "*";

                if (i != letters.Length - 1) point += ",";
            }
            string responseBody = "";
            string error = "";
            using (HttpClient client = new HttpClient())
            {

                // 添加Authorization头root.test_group.test_device
                client.DefaultRequestHeaders.Add("Authorization", "cm9vdDpyb290");
                //定义查询语句 
                var json = "";
                //json = "{\"sql\":\"select last* value from " + point + " limit 9000\" }";

                if (!string.IsNullOrWhiteSpace(Time))
                {
                    json = "{\"sql\":\"select last* value from " + point + " where time =" + DateTime.Parse(Time).ToString("yyyy-MM-dd HH:mm:ss") + "\" }";

                }
                else
                {
                    json = "{\"sql\":\"select last* value from " + point + "\" }";
                }
                var url = GlobalContext.SystemConfig.IotDBURL;

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
            if (jsonData != null)
            {
                List<DeviceMatchResult> deviceMatchResults = MatchFssAndReactor(jsonData, ' ');
                return deviceMatchResults;
            }
            return null;
        }


        /// <summary>
        /// 第一层点数据
        /// </summary>
        /// <param name="PointName">点名</param>
        /// <param name="Time">时间（2026-05-11 08:11:47）</param>
        /// <param name="differencePercent">量程(默认5)</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public async Task<List<DeviceMatchResult>> ShadowCloudData2([FromQuery] string PointName, [FromQuery] string Time, [FromQuery] string differencePercent = "5")
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
                if (!string.IsNullOrWhiteSpace(Time))
                {
                    json = "{\"sql\":\"select last* value from root.KNS.AA5.*" + PointName + ",root.FSS.AA5.*" + PointName + " where time =" + DateTime.Parse(Time).ToString("yyyy-MM-dd HH:mm:ss") + "\" }";

                }
                else
                {
                    json = "{\"sql\":\"select last* value from root.KNS.AA5.*" + PointName + ",root.FSS.AA5.*" + PointName + "\" }";
                }

                //定义query接口
                var url = GlobalContext.SystemConfig.IotDBURL;
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
            Param_RangeService Param_RangeService = new Param_RangeService();
            if (jsonData != null)
            {
                List<DeviceMatchResult> deviceMatchResults = MatchFssAndReactor(jsonData, ' ', false, true);

                foreach (var item in deviceMatchResults)
                {
                    // 只有单边数据时无法对比，跳过报警判断，不影响数据返回
                    if (item.FSS_Value == null || item.Reactor_Value == null)
                    {
                        item.IsAlert = null;
                        continue;
                    }
                    // 与左边界面规则一致：配了量程就除量程算百分比，没配量程直接比原始差值
                    float diff = Math.Abs(float.Parse(item.FSS_Value) - float.Parse(item.Reactor_Value));
                    Param_RangeEntity param = await Param_RangeService.GetRangeEntity(item.DeviceCode);
                    if (param != null && ((float)param.Range - (float)param.RangeMin) != 0)
                    {
                        diff = diff / ((float)param.Range - (float)param.RangeMin) * 100;
                    }
                    param_ThresholdService param_ThresholdService = new param_ThresholdService();
                    var param_Threshold = await param_ThresholdService.GetNameEntity(item.DeviceCode);
                    if (param_Threshold != null)
                    {
                        if (diff > param_Threshold.Value)
                        {
                            item.IsAlert = true;
                            item.WarningValue = param_Threshold.Value;

                        }
                        else
                        {
                            item.IsAlert = false;
                            item.WarningValue = param_Threshold.Value;
                        }
                    }
                    else
                    {
                        item.IsAlert = null;
                    }
                    
                }

                return deviceMatchResults;
            }
            return null;
        }

        /// <summary>
        /// 特征量数据
        /// </summary>
        /// <param name="FeatureQuantityName">特征量点名</param>
        /// <param name="Time">时间（2026-05-11 08:11:47）</param>
        /// <param name="Limit">数量（默认 1）</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public async Task<IotDBinfoLow> FeatureQuantityData([FromQuery] string FeatureQuantityName, [FromQuery] string Time, [FromQuery] string Limit = "1")
        {
            string[] letters = FeatureQuantityName.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
            string point = "";
            for (int i = 0; i < letters.Length; i++)
            {
                string letter = letters[i];
                point = point + "root.FSS.AA5.*" + letter + "*";

                if (i != letters.Length - 1) point += ",";
            }
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
                if (!string.IsNullOrWhiteSpace(Time))
                {
                    json = "{\"sql\":\"select * from " + point + " where time =" + DateTime.Parse(Time).ToString("yyyy-MM-dd HH:mm:ss") + " limit " + Limit + "\" }";
                }
                else
                {
                    json = "{\"sql\":\"select last* from " + point + "\" }";
                }

                //定义query接口
                var url = GlobalContext.SystemConfig.IotDBURL;
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
        /// 各个系统的报警数量，以及24小时之前新增的报警数
        /// </summary>
        /// <param name="SYSName">（ABP，AHP）</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public async Task<List<SYSNewAtler>> SYSNewAtlers(string SYSName)
        {
            List<SYSNewAtler> sYSNewAtlerList = new List<SYSNewAtler>();
            string[] letters = SYSName.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in letters)
            {
                SYSNewAtler sYSNewAtler = new SYSNewAtler();
                List<ParamAlertsEntity> paramAlertsEntities = new List<ParamAlertsEntity>();
                paramAlertsEntities = await paramAlertsService.GetListNumber(item);
                sYSNewAtler.Name = item;
                sYSNewAtler.number = paramAlertsEntities.Count;
                DateTime dateTime = DateTime.Now;
                DateTime odldateTime = DateTime.Today;
                sYSNewAtler.addnumber = paramAlertsEntities.Count(t => t.BaseCreateTime < DateTime.Now && t.BaseCreateTime > odldateTime);
                sYSNewAtlerList.Add(sYSNewAtler);
            }
            return sYSNewAtlerList;

        }

        /// <summary>
        /// 获取量程
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> GetThresholdValue(string Name)
        {
            param_ThresholdService param_ThresholdService= new param_ThresholdService();
            var  param_Threshold = await param_ThresholdService.GetNameEntity(Name);
            if (param_Threshold != null)
            {
                return param_Threshold.Value.ToString();
            }
            else
            { 
                return "未知阈值";
            }            
        }

        [HttpGet]
        public async Task<string> UpdataThresholdValue(string Name,string Value)
        {
            param_ThresholdService param_ThresholdService = new param_ThresholdService();
            try
            {
                await param_ThresholdService.UpdataSaveNameForm(Name, Value);
                return "success";
            }
            catch (Exception ex) 
            { 
                return ex.ToString();           
            }
            
           
        }



    }
}
