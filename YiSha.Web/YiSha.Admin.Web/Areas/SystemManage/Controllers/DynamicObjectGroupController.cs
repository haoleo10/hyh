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
using YiSha.Entity.SystemManage;
using YiSha.Business.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Business.OrganizationManage;
using YiSha.Entity.OrganizationManage;
using YiSha.Model.Param;

namespace YiSha.Admin.Web.Areas.SystemManage.Controllers
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2024-12-17 15:20
    /// 描 述：动态数据组控制器类
    /// </summary>
    [Area("SystemManage")]
    public class DynamicObjectGroupController :  BaseController
    {
        private DynamicObjectGroupBLL dynamicObjectGroupBLL = new DynamicObjectGroupBLL();

        #region 视图功能
        [AuthorizeFilter("system:dynamicobjectgroup:view")]
        public ActionResult DynamicObjectGroupIndex()
        {
            return View();
        }

        public ActionResult DynamicObjectGroupForm()
        {
            return View();
        }
        public ActionResult ImportObjectGroupForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("system:dynamicobjectgroup:search")]
        public async Task<ActionResult> GetListJson(DynamicObjectGroupListParam param)
        {
            TData<List<DynamicObjectGroupEntity>> obj = await dynamicObjectGroupBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("system:dynamicobjectgroup:search")]
        public async Task<ActionResult> GetPageListJson(DynamicObjectGroupListParam param, Pagination pagination)
        {
            TData<List<DynamicObjectGroupEntity>> obj = await dynamicObjectGroupBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<DynamicObjectGroupEntity> obj = await dynamicObjectGroupBLL.GetEntity(id);
            return Json(obj);
        }     
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("system:dynamicobjectgroup:add,system:dynamicobjectgroup:edit")]
        public async Task<ActionResult> SaveFormJson(DynamicObjectGroupEntity entity)
        {
            TData<string> obj = await dynamicObjectGroupBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("system:dynamicobjectgroup:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await dynamicObjectGroupBLL.DeleteForm(ids);
            return Json(obj);
        }

        [HttpPost]
        public async Task<IActionResult> ImportJson(ImportParam param)
        {
            TData obj = await dynamicObjectGroupBLL.ImportJson(param);
            return Json(obj);
        }

        #endregion
    }
}
