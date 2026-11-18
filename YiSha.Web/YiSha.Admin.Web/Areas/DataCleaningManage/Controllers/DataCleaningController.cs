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
using YiSha.Entity.DataCleaningManage;
using YiSha.Business.DataCleaningManage;
using YiSha.Model.Param.DataCleaningManage;
using YiSha.Model.IotDBData;
using Newtonsoft.Json;
using static YiSha.Admin.Web.Areas.IntelligentSearchManage.Controllers.IntelligentSearchController;
using System.Net.Http;

namespace YiSha.Admin.Web.Areas.DataCleaningManage.Controllers
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2025-05-14 14:23
    /// 描 述：数据清洗控制器类
    /// </summary>
    [Area("DataCleaningManage")]
    public class DataCleaningController :  BaseController
    {
        private DataCleaningBLL dataCleaningBLL = new DataCleaningBLL();

        #region 视图功能
        [AuthorizeFilter("datacleaning:datacleaning:view")]
        public ActionResult DataCleaningIndex()
        {
            return View();
        }

        public ActionResult DataCleaningForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("datacleaning:datacleaning:search")]
        public async Task<ActionResult> GetListJson(DataCleaningListParam param)
        {
            TData<List<DataCleaningEntity>> obj = await dataCleaningBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("datacleaning:datacleaning:search")]
        public async Task<ActionResult> GetPageListJson(DataCleaningListParam param, Pagination pagination)
        {
            List<IotDBResultData> iotDBResultDatas = new List<IotDBResultData>();
            var parameters = new Dictionary<string, string>
            {
                 { "System", param.System },
                 { "DeviceName",param.DeviceName },               
                 { "StartTime",param.StartTime.ToString() },
                 { "EndTime", param.EndTime.ToString() },
            };
            // 构建查询字符串
            var queryParams = string.Join("&", parameters.Select(p =>
                $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value)}"));
            string url = "http://" + @GlobalContext.SystemConfig.WebApiIP + ":" + @GlobalContext.SystemConfig.WebApiPort + $"/IotDBData/GetIotDBDataCleaning?{queryParams}";
            //接收数据
            string responseBody = "";

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
                            iotDBResultDatas = JsonConvert.DeserializeObject<List<IotDBResultData>>(responseBody);
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
                TData<List<IotDBResultData>> responseBodyData = new TData<List<IotDBResultData>>();
                responseBodyData.Data = iotDBResultDatas.FindAll(s => s.Column_name.Contains(param.DeviceName));
                responseBodyData.Total = pagination.TotalCount;
                responseBodyData.Tag = 1;
                return Json(responseBodyData);
            }
            else return null;

        }

        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<DataCleaningEntity> obj = await dataCleaningBLL.GetEntity(id);
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("datacleaning:datacleaning:add,datacleaning:datacleaning:edit")]
        public async Task<ActionResult> SaveFormJson(DataCleaningEntity entity)
        {
            TData<string> obj = await dataCleaningBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("datacleaning:datacleaning:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await dataCleaningBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}
