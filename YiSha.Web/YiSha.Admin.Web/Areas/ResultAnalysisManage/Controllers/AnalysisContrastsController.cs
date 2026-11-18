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
using YiSha.Entity.ResultAnalysisManage;
using YiSha.Business.ResultAnalysisManage;
using YiSha.Model.Param.ResultAnalysisManage;

namespace YiSha.Admin.Web.Areas.ResultAnalysisManage.Controllers
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2024-08-19 15:07
    /// 描 述：结果分析(状态对比分析)控制器类
    /// </summary>
    [Area("ResultAnalysisManage")]
    public class AnalysisContrastsController :  BaseController
    {
        private AnalysisContrastsBLL analysisContrastsBLL = new AnalysisContrastsBLL();

        #region 视图功能
        [AuthorizeFilter("resultanalysis:analysiscontrasts:view")]
        public ActionResult AnalysisContrastsIndex()
        {
            return View();
        }

        public ActionResult AnalysisContrastsForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("resultanalysis:analysiscontrasts:search")]
        public async Task<ActionResult> GetListJson(AnalysisContrastsListParam param)
        {
            TData<List<AnalysisContrastsEntity>> obj = await analysisContrastsBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("resultanalysis:analysiscontrasts:search")]
        public async Task<ActionResult> GetPageListJson(AnalysisContrastsListParam param, Pagination pagination)
        {
            TData<List<AnalysisContrastsEntity>> obj = await analysisContrastsBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<AnalysisContrastsEntity> obj = await analysisContrastsBLL.GetEntity(id);
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("resultanalysis:analysiscontrasts:add,resultanalysis:analysiscontrasts:edit")]
        public async Task<ActionResult> SaveFormJson(AnalysisContrastsEntity entity)
        {
            TData<string> obj = await analysisContrastsBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("resultanalysis:analysiscontrasts:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await analysisContrastsBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}
