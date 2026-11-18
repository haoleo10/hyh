using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using YiSha.Admin.Web.Controllers;
using YiSha.Business.ThreeDimensions‌Manage;
using YiSha.Entity;
using YiSha.Entity.ThreeDimensions‌Manage;
using YiSha.Model;
using YiSha.Model.Param.ThreeDimensions‌Manage;
using YiSha.Util;
using YiSha.Util.Model;

namespace YiSha.Admin.Web.Areas.ThreeDimensions‌Manage.Controllers
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2026-05-19 15:37
    /// 描 述：贴吧控制器类
    /// </summary>
    [Area("ThreeDimensions‌Manage")]
    public class param_TiebaController :  BaseController
    {
        private param_TiebaBLL param_TiebaBLL = new param_TiebaBLL();

        #region 视图功能
        [AuthorizeFilter("threedimensions‌:param_tieba:view")]
        public ActionResult param_TiebaIndex()
        {
            return View();
        }

        public ActionResult param_TiebaForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("threedimensions‌:param_tieba:search")]
        public async Task<ActionResult> GetListJson(param_TiebaListParam param)
        {
            TData<List<param_TiebaEntity>> obj = await param_TiebaBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("threedimensions‌:param_tieba:search")]
        public async Task<ActionResult> GetPageListJson(param_TiebaListParam param, Pagination pagination)
        {
            TData<List<param_TiebaEntity>> obj = await param_TiebaBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<param_TiebaEntity> obj = await param_TiebaBLL.GetEntity(id);
            return Json(obj);
        }
        #endregion

        #region 提交数据

        [HttpPost]     
        public async Task<ActionResult> SaveFormJson(param_TiebaEntity entity)
        {
            TData<string> obj = await param_TiebaBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("threedimensions‌:param_tieba:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await param_TiebaBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}
