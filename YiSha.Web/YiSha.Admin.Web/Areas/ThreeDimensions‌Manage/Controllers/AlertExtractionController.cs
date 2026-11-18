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
using YiSha.Entity.ThreeDimensions‌Manage;
using YiSha.Business.ThreeDimensions‌Manage;
using YiSha.Model.Param.ThreeDimensions‌Manage;

namespace YiSha.Admin.Web.Areas.ThreeDimensions‌Manage.Controllers
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2026-06-17 14:39
    /// 描 述：萃取控制器类
    /// </summary>
    [Area("ThreeDimensions‌Manage")]
    public class AlertExtractionController :  BaseController
    {
        private AlertExtractionBLL alertExtractionBLL = new AlertExtractionBLL();

        #region 视图功能
        [AuthorizeFilter("threedimensions‌:alertextraction:view")]
        public ActionResult AlertExtractionIndex()
        {
            return View();
        }

        public ActionResult AlertExtractionForm()
        {
            return View();
        }

        public ActionResult CompleteAlertExtractionIndex()
        {
            return View();
        }



        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("threedimensions‌:alertextraction:search")]
        public async Task<ActionResult> GetListJson(AlertExtractionListParam param)
        {
            TData<List<AlertExtractionEntity>> obj = await alertExtractionBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("threedimensions‌:alertextraction:search")]
        public async Task<ActionResult> GetPageListJson(AlertExtractionListParam param, Pagination pagination)
        {
            TData<List<AlertExtractionEntity>> obj = await alertExtractionBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        public async Task<ActionResult> CompleteGetPageListJson(AlertExtractionListParam param, Pagination pagination)
        {
            TData<List<AlertExtractionEntity>> obj = await alertExtractionBLL.CompleteGetPageListJson(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<AlertExtractionEntity> obj = await alertExtractionBLL.GetEntity(id);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> Extraction(string id)
        {
            TData obj = await alertExtractionBLL.Extraction(id);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> CompleteExtraction(string id)
        {
            TData obj = await alertExtractionBLL.CompleteExtraction(id);
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("threedimensions‌:alertextraction:add,threedimensions‌:alertextraction:edit")]
        public async Task<ActionResult> SaveFormJson(AlertExtractionEntity entity)
        {
            TData<string> obj = await alertExtractionBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("threedimensions‌:alertextraction:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await alertExtractionBLL.DeleteForm(ids);
            return Json(obj);
        }

       


        #endregion
    }
}
