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
    /// 日 期：2024-09-09 09:23
    /// 描 述：特征量对比分析控制器类
    /// </summary>
    [Area("ResultAnalysisManage")]
    public class AnalysisTzlController :  BaseController
    {
        private AnalysisTzlBLL analysisTzlBLL = new AnalysisTzlBLL();

        #region 视图功能
        [AuthorizeFilter("resultanalysis:analysistzl:view")]
        public ActionResult AnalysisTzlIndex()
        {
            return View();
        }

        public ActionResult AnalysisTzlForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("resultanalysis:analysistzl:search")]
        public async Task<ActionResult> GetListJson(AnalysisTzlListParam param)
        {
            TData<List<AnalysisTzlEntity>> obj = await analysisTzlBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("resultanalysis:analysistzl:search")]
        public async Task<ActionResult> GetPageListJson(AnalysisTzlListParam param, Pagination pagination)
        {
            TData<List<AnalysisTzlEntity>> obj = await analysisTzlBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<AnalysisTzlEntity> obj = await analysisTzlBLL.GetEntity(id);
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("resultanalysis:analysistzl:add,resultanalysis:analysistzl:edit")]
        public async Task<ActionResult> SaveFormJson(AnalysisTzlEntity entity)
        {
            TData<string> obj = await analysisTzlBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("resultanalysis:analysistzl:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await analysisTzlBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}
