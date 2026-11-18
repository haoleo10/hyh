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
using YiSha.Business.FeatrueQuantityManage;

namespace YiSha.Admin.Web.Areas.ResultAnalysisManage.Controllers
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2024-08-19 15:00
    /// 描 述：结果分析(状态初始化)控制器类
    /// </summary>
    [Area("ResultAnalysisManage")]
    public class AnalysisInitializationController : BaseController
    {
        private AnalysisInitializationBLL analysisInitializationBLL = new AnalysisInitializationBLL();

        #region 视图功能
        [AuthorizeFilter("resultanalysis:analysisinitialization:view")]
        public ActionResult AnalysisInitializationIndex()
        {
            return View();
        }

        public ActionResult AnalysisInitializationForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("resultanalysis:analysisinitialization:search")]
        public async Task<ActionResult> GetListJson(AnalysisInitializationListParam param)
        {
            TData<List<AnalysisInitializationEntity>> obj = await analysisInitializationBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("resultanalysis:analysisinitialization:search")]
        public async Task<ActionResult> GetPageListJson(AnalysisInitializationListParam param, Pagination pagination)
        {
            TData<List<AnalysisInitializationEntity>> obj = await analysisInitializationBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<AnalysisInitializationEntity> obj = await analysisInitializationBLL.GetEntity(id);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> TestCmd(int cmd)
        {
            TData obj = await analysisInitializationBLL.TestCmd(cmd);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> TestCmdData() {
            TData obj = await analysisInitializationBLL.TestCmdData();
            return Json(obj);
        }


        /// <summary>
        /// 开始同步
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> StartSyncCmdData()
        {
            TData obj = await analysisInitializationBLL.StartSyncCmdData();
            return Json(obj);
        }

        /// <summary>
        /// 停止同步
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> StopSyncCmdData()
        {
            TData obj = await analysisInitializationBLL.StopSyncCmdData();
            return Json(obj);
        }

        /// <summary>
        /// 差异同步
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> DiversitySyncCmdData()
        {
            TData obj = await analysisInitializationBLL.DiversitySyncCmdData();
            return Json(obj);
        }


        /// <summary>
        /// 开始初始化
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> InitializationStarted()
        {
            await analysisInitializationBLL.InitializationStarted();
            TData obj = new TData();
            obj.Tag = 1;
            obj.Message = "开始初始化";
            return Json(obj);
        }

        /// <summary>
        /// 初始化暂停
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> InitializationPause()
        {
            await analysisInitializationBLL.InitializationPause();
            TData obj = new TData();
            obj.Tag = 1;
            obj.Message = "初始化暂停";
            return Json(obj);
        }

        /// <summary>
        /// 初始化终止
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> InitializationTermination()
        {
            await analysisInitializationBLL.InitializationTermination();
            TData obj = new TData();
            obj.Tag = 1;
            obj.Message = "初始化终止";
            return Json(obj);
        }

        /// <summary>
        /// 加载IC
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> LoadingIC()
        {
            await analysisInitializationBLL.LoadingIC();
            TData obj = new TData();
            obj.Tag = 1;
            obj.Message = "加载IC";
            return Json(obj);
        }

        /// <summary>
        /// 数据同步
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> DataSynchronization()
        {
            await analysisInitializationBLL.DataSynchronization();
            TData obj = new TData();
            obj.Tag = 1;
            obj.Message = "数据同步";
            return Json(obj);
        }


        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("resultanalysis:analysisinitialization:add,resultanalysis:analysisinitialization:edit")]
        public async Task<ActionResult> SaveFormJson(AnalysisInitializationEntity entity)
        {
            TData<string> obj = await analysisInitializationBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("resultanalysis:analysisinitialization:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await analysisInitializationBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}
