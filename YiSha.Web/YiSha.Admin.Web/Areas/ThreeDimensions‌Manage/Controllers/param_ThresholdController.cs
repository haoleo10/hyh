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
    /// 日 期：2026-09-21 15:03
    /// 描 述：报警阈值控制器类
    /// </summary>
    [Area("ThreeDimensions‌Manage")]
    public class param_ThresholdController :  BaseController
    {
        private param_ThresholdBLL param_ThresholdBLL = new param_ThresholdBLL();

        #region 视图功能
        [AuthorizeFilter("threedimensions‌:param_threshold:view")]
        public ActionResult param_ThresholdIndex()
        {
            return View();
        }

        public ActionResult param_ThresholdForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("threedimensions‌:param_threshold:search")]
        public async Task<ActionResult> GetListJson(param_ThresholdListParam param)
        {
            TData<List<param_ThresholdEntity>> obj = await param_ThresholdBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("threedimensions‌:param_threshold:search")]
        public async Task<ActionResult> GetPageListJson(param_ThresholdListParam param, Pagination pagination)
        {
            TData<List<param_ThresholdEntity>> obj = await param_ThresholdBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<param_ThresholdEntity> obj = await param_ThresholdBLL.GetEntity(id);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetNameFormJson(string Name)
        {
            TData<param_ThresholdEntity> obj = await param_ThresholdBLL.GetNameEntity(Name);
            return Json(obj);
        }


        [HttpGet]      
        public async Task<ActionResult> GetListNameFormJson(string Name)
        {
            TData<List<param_ThresholdEntity>> obj = await param_ThresholdBLL.GetListNameEntity(Name);
            return Json(obj);
        }

        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("threedimensions‌:param_threshold:add,threedimensions‌:param_threshold:edit")]
        public async Task<ActionResult> SaveFormJson(param_ThresholdEntity entity)
        {
            TData<string> obj = await param_ThresholdBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("threedimensions‌:param_threshold:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await param_ThresholdBLL.DeleteForm(ids);
            return Json(obj);
        }

        [HttpPost]      
        public async Task<ActionResult> SaveNameForm(string Name)
        {
            TData<string> obj = await param_ThresholdBLL.SaveNameForm(Name);
            return Json(obj);
        }
        
        #endregion
    }
}
