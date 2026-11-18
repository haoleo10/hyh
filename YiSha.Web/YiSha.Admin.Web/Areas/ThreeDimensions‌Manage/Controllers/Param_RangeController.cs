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
    /// 日 期：2026-08-06 15:35
    /// 描 述：量程控制器类
    /// </summary>
    [Area("ThreeDimensions‌Manage")]
    public class Param_RangeController :  BaseController
    {
        private Param_RangeBLL param_RangeBLL = new Param_RangeBLL();

        #region 视图功能
        [AuthorizeFilter("threedimensions‌:param_range:view")]
        public ActionResult Param_RangeIndex()
        {
            return View();
        }

        public ActionResult Param_RangeForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("threedimensions‌:param_range:search")]
        public async Task<ActionResult> GetListJson(ListParam_RangeListParam param)
        {
            TData<List<Param_RangeEntity>> obj = await param_RangeBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("threedimensions‌:param_range:search")]
        public async Task<ActionResult> GetPageListJson(ListParam_RangeListParam param, Pagination pagination)
        {
            pagination.PageSize = 50000;
            TData<List<Param_RangeEntity>> obj = await param_RangeBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<Param_RangeEntity> obj = await param_RangeBLL.GetEntity(id);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetRangeEntity(string dotName, string Value)
        {
            TData<Param_RangeEntity> obj = await param_RangeBLL.GetRangeEntity(dotName, Value);
            return Json(obj);
        }


        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("threedimensions‌:param_range:add,threedimensions‌:param_range:edit")]
        public async Task<ActionResult> SaveFormJson(Param_RangeEntity entity)
        {
            TData<string> obj = await param_RangeBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("threedimensions‌:param_range:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await param_RangeBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}
