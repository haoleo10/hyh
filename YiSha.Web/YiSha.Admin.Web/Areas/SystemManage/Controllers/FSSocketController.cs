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

namespace YiSha.Admin.Web.Areas.SystemManage.Controllers
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2024-10-31 14:56
    /// 描 述：数据通信控制器类
    /// </summary>
    [Area("SystemManage")]
    public class FSSocketController :  BaseController
    {
        private FSSocketBLL fSSocketBLL = new FSSocketBLL();

        #region 视图功能
        [AuthorizeFilter("system:fssocket:view")]
        public ActionResult FSSocketIndex()
        {
            return View();
        }

        public ActionResult FSSocketForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("system:fssocket:search")]
        public async Task<ActionResult> GetListJson(FSSocketListParam param)
        {
            TData<List<FSSocketEntity>> obj = await fSSocketBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("system:fssocket:search")]
        public async Task<ActionResult> GetPageListJson(FSSocketListParam param, Pagination pagination)
        {
            TData<List<FSSocketEntity>> obj = await fSSocketBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<FSSocketEntity> obj = await fSSocketBLL.GetEntity(id);
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("system:fssocket:add,system:fssocket:edit")]
        public async Task<ActionResult> SaveFormJson(FSSocketEntity entity)
        {
            TData<string> obj = await fSSocketBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("system:fssocket:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await fSSocketBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}
