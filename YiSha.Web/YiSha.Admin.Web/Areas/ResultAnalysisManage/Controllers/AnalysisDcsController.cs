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
    /// 日 期：2024-09-09 09:21
    /// 描 述：DCS对比分析控制器类
    /// </summary>
    [Area("ResultAnalysisManage")]
    public class AnalysisDcsController :  BaseController
    {
        private AnalysisDcsBLL analysisDcsBLL = new AnalysisDcsBLL();

        #region 视图功能
        [AuthorizeFilter("resultanalysis:analysisdcs:view")]
        public ActionResult AnalysisDcsIndex()
        {
            return View();
        }

        public ActionResult AnalysisDcsForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("resultanalysis:analysisdcs:search")]
        public async Task<ActionResult> GetListJson(AnalysisDcsListParam param)
        {
            TData<List<AnalysisDcsEntity>> obj = await analysisDcsBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("resultanalysis:analysisdcs:search")]
        public async Task<ActionResult> GetPageListJson(AnalysisDcsListParam param, Pagination pagination)
        {
            TData<List<AnalysisDcsEntity>> obj = await analysisDcsBLL.GetPageList(param, pagination);
            string ddd = Json(obj).ToString(); ;
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<AnalysisDcsEntity> obj = await analysisDcsBLL.GetEntity(id);
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("resultanalysis:analysisdcs:add,resultanalysis:analysisdcs:edit")]
        public async Task<ActionResult> SaveFormJson(AnalysisDcsEntity entity)
        {
            TData<string> obj = await analysisDcsBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("resultanalysis:analysisdcs:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await analysisDcsBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}
