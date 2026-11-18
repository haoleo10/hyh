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
    /// 日 期：2026-05-19 10:58
    /// 描 述：报警列表控制器类
    /// </summary>
    [Area("ThreeDimensions‌Manage")]
    public class ParamAlertsController :  BaseController
    {
        private ParamAlertsBLL paramAlertsBLL = new ParamAlertsBLL();

        #region 视图功能
        [AuthorizeFilter("threedimensions‌:paramalerts:view")]
        public ActionResult ParamAlertsIndex()
        {
            return View();
        }

        public ActionResult ParamAlertsForm()
        {
            return View();
        }

        public ActionResult ExtractionIndex() 
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("threedimensions‌:paramalerts:search")]
        public async Task<ActionResult> GetListJson(ListParamAlertsListParam param, string AlarmLayer, string SYS)
        {
            TData<List<ParamAlertsEntity>> obj = await paramAlertsBLL.GetList(param, AlarmLayer, SYS);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("threedimensions‌:paramalerts:search")]
        public async Task<ActionResult> GetPageListJson(ListParamAlertsListParam param, Pagination pagination, string AlarmLayer,string SYS)
        {
            TData<List<ParamAlertsEntity>> obj = await paramAlertsBLL.GetPageList(param, pagination, AlarmLayer, SYS);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<ParamAlertsEntity> obj = await paramAlertsBLL.GetEntity(id);
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("threedimensions‌:paramalerts:add,threedimensions‌:paramalerts:edit")]
        public async Task<ActionResult> SaveFormJson(ParamAlertsEntity entity)
        {
            TData<string> obj = await paramAlertsBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("threedimensions‌:paramalerts:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await paramAlertsBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}
