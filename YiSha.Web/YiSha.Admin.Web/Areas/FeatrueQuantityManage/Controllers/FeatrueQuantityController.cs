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
using YiSha.Entity.FeatrueQuantityManage;
using YiSha.Business.FeatrueQuantityManage;
using YiSha.Model.Param.FeatrueQuantityManage;
using Newtonsoft.Json;
using System.Net.Http;
using YiSha.Model.Param.IntelligentSearchManage;
using YiSha.Model.IotDBData;
using YiSha.Business.ResultAnalysisManage;

namespace YiSha.Admin.Web.Areas.FeatrueQuantityManage.Controllers
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2024-08-20 11:09
    /// 描 述：特征量计算控制器类
    /// </summary>
    [Area("FeatrueQuantityManage")]
    public class FeatrueQuantityController : BaseController
    {
        private FeatrueQuantityBLL featrueQuantityBLL = new FeatrueQuantityBLL();

        #region 视图功能
        [AuthorizeFilter("featruequantity:featruequantity:view")]
        public ActionResult FeatrueQuantityIndex()
        {
            return View();
        }

        public ActionResult FeatrueQuantityForm()
        {
            return View();
        }

        public ActionResult FeatrueQuantityTrendChart()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("featruequantity:featruequantity:search")]
        public async Task<ActionResult> GetListJson(FeatrueQuantityListParam param)
        {
            TData<List<FeatrueQuantityEntity>> obj = await featrueQuantityBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("featruequantity:featruequantity:search")]
        public async Task<ActionResult> GetPageListJson(FeatrueQuantityListParam param, Pagination pagination)
        {
            TData<List<FeatrueQuantityEntity>> obj = await featrueQuantityBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetPageListJsonChart(FeatrueQuantityListParam param, Pagination pagination)
        {
            TData<List<FeatrueQuantityEntity>> obj = await featrueQuantityBLL.GetPageListChart(param, pagination);
            return Json(obj);
        }


        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<FeatrueQuantityEntity> obj = await featrueQuantityBLL.GetEntity(id);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetState()
        {
            TData<object> obj = await featrueQuantityBLL.GetState();
            return Json(obj);
        }

        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("featruequantity:featruequantity:add,featruequantity:featruequantity:edit")]
        public async Task<ActionResult> SaveFormJson(FeatrueQuantityEntity entity)
        {
            TData<string> obj = await featrueQuantityBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("featruequantity:featruequantity:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await featrueQuantityBLL.DeleteForm(ids);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> StartCompute()
        {
            await featrueQuantityBLL.StartCompute();
            TData obj = new TData();
            obj.Tag = 1;
            obj.Message = "计算任务已启动";
            return Json(obj);  
        }


        [HttpGet]
        public async Task<ActionResult> StopCompute()
        {
            await featrueQuantityBLL.StopCompute();
            TData obj = new TData();
            obj.Tag = 1;
            obj.Message = "计算任务停止";
            return Json(obj);
        }



        public class iotData
        {
            public long? Id { get; set; }
            public string Column_name { get; set; }

            public string Data_type { get; set; }

            public float? DataValue { get; set; }
        }

        [HttpGet]
        public async Task<ActionResult> GetEchartDataListJsons(string Device)
        {
            DateTime now = DateTime.Now;

            // 计算6个月前的时间
            DateTime sixMonthsAgo = now.AddMonths(-12);
            List<iotData> iotDatas = new List<iotData>();

            // 定义接口参数
            var parameters = new Dictionary<string, string>
            {
                 { "DoneWorkName",Device },
                 { "StartTime",sixMonthsAgo.ToString() },
                 { "EndTime", now.ToString() },
                 { "State", "1"},
            };
            string responseBody = "";
            // 构建查询字符串
            var queryParams = string.Join("&", parameters.Select(p =>
                $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value)}"));

            string url = "http://" + @GlobalContext.SystemConfig.WebApiIP + ":" + @GlobalContext.SystemConfig.WebApiPort + $"/IotDBData/GetIOTDBechatrDatas?{queryParams}";

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
                responseBodyData.Data = iotDatas;
                responseBodyData.Tag = 1;
                return Json(responseBodyData);
            }
            else return null;
        }

        [HttpGet]
        public async Task<ActionResult> GetEchartDataListJsonss(string Device)
        {
            DateTime now = DateTime.Now;

            // 计算6个月前的时间
            DateTime sixMonthsAgo = now.AddMonths(-24);
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

            string url = "http://" + @GlobalContext.SystemConfig.WebApiIP + ":" + @GlobalContext.SystemConfig.WebApiPort + $"/IotDBData/GetIOTDBDatasLS?{queryParams}";

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
            foreach (var item in iotDatas.values[0])
            {
                strings.Add(Device);
            }
            iotDatas.column_names = strings;
            return Json(iotDatas);

        }

        #endregion
    }
}
