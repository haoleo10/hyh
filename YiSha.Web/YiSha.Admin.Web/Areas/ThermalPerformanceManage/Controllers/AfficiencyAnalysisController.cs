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
using YiSha.Entity.ThermalPerformanceManage;
using YiSha.Business.ThermalPerformanceManage;
using YiSha.Model.Param.ThermalPerformanceManage;
using Newtonsoft.Json;
using System.Net.Http;
using YiSha.Model.Param.IntelligentSearchManage;
using YiSha.Model.IotDBData;

namespace YiSha.Admin.Web.Areas.ThermalPerformanceManage.Controllers
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2024-08-19 15:23
    /// 描 述：热效率分析控制器类
    /// </summary>
    [Area("ThermalPerformanceManage")]
    public class AfficiencyAnalysisController :  BaseController
    {
        private AfficiencyAnalysisBLL afficiencyAnalysisBLL = new AfficiencyAnalysisBLL();

        #region 视图功能
        [AuthorizeFilter("thermalperformance:afficiencyanalysis:view")]
        public ActionResult AfficiencyAnalysisIndex()
        {
            return View();
        }

        public ActionResult AfficiencyAnalysisForm()
        {
            return View();
        }

        public ActionResult CalculationsForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("thermalperformance:afficiencyanalysis:search")]
        public async Task<ActionResult> GetListJson(AfficiencyAnalysisListParam param)
        {
            TData<List<AfficiencyAnalysisEntity>> obj = await afficiencyAnalysisBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("thermalperformance:afficiencyanalysis:search")]
        public async Task<ActionResult> GetPageListJson(AfficiencyAnalysisListParam param, Pagination pagination)
        {
            TData<List<AfficiencyAnalysisEntity>> obj = await afficiencyAnalysisBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<AfficiencyAnalysisEntity> obj = await afficiencyAnalysisBLL.GetEntity(id);
            return Json(obj);
        }

        public async Task<ActionResult> GetKS()
        {
            TData<List<long>> obj = await afficiencyAnalysisBLL.GetKS();
            return Json(obj);
        }

        public async Task<ActionResult> Getittdb()
        {
            TData<IotDBinfoLow> iotDatas = new TData<IotDBinfoLow>();
            string responseBody = "";
          

            string url = "http://" + @GlobalContext.SystemConfig.WebApiIP + ":" + @GlobalContext.SystemConfig.WebApiPort + "/IotDBData/GetIotDBAfficiencyAnalysisEnergyAllocation";

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
                            iotDatas = JsonConvert.DeserializeObject<TData<IotDBinfoLow>>(responseBody);
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
                
                return Json(iotDatas);

            }
            else return null;
        }

        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("thermalperformance:afficiencyanalysis:add,thermalperformance:afficiencyanalysis:edit")]
        public async Task<ActionResult> SaveFormJson(AfficiencyAnalysisEntity entity)
        {
            TData<string> obj = await afficiencyAnalysisBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("thermalperformance:afficiencyanalysis:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await afficiencyAnalysisBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}