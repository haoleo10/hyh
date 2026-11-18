using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Entity.ResultAnalysisManage;
using YiSha.Model.Param.ResultAnalysisManage;
using YiSha.Service.ResultAnalysisManage;
using YiSha.Service.FeatrueQuantityManage;

namespace YiSha.Business.ResultAnalysisManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2024-08-19 15:00
    /// 描 述：结果分析(状态初始化)业务类
    /// </summary>
    public class AnalysisInitializationBLL
    {
        private AnalysisInitializationService analysisInitializationService = new AnalysisInitializationService();

        #region 获取数据
        public async Task<TData<List<AnalysisInitializationEntity>>> GetList(AnalysisInitializationListParam param)
        {
            TData<List<AnalysisInitializationEntity>> obj = new TData<List<AnalysisInitializationEntity>>();
            obj.Data = await analysisInitializationService.GetList(param);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<AnalysisInitializationEntity>>> GetPageList(AnalysisInitializationListParam param, Pagination pagination)
        {
            TData<List<AnalysisInitializationEntity>> obj = new TData<List<AnalysisInitializationEntity>>();
            obj.Data = await analysisInitializationService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<AnalysisInitializationEntity>> GetEntity(long id)
        {
            TData<AnalysisInitializationEntity> obj = new TData<AnalysisInitializationEntity>();
            obj.Data = await analysisInitializationService.GetEntity(id);
            if (obj.Data != null)
            {
                obj.Tag = 1;
            }
            return obj;
        }

        public async Task<TData> TestCmd(int cmd)
        {
            TData<string> obj = new TData<string>();
            await analysisInitializationService.TestCmd(cmd);
            obj.Data = "";
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> TestCmdData()
        {
            TData<string> obj = new TData<string>();
            await analysisInitializationService.TestCmdData();
            obj.Data = "";
            obj.Tag = 1;
            return obj;
        }
        public async Task<TData> StartSyncCmdData()
        {
            TData<string> obj = new TData<string>();
            await analysisInitializationService.StartSyncCmdData();
            obj.Data = "";
            obj.Tag = 1;
            return obj;
        }
        public async Task<TData> StopSyncCmdData()
        {
            TData<string> obj = new TData<string>();
            await analysisInitializationService.StopSyncCmdData();
            obj.Data = "";
            obj.Tag = 1;
            return obj;
        }


        public async Task<TData> DiversitySyncCmdData()
        {
            TData<string> obj = new TData<string>();
            await analysisInitializationService.DiversitySyncCmdData();
            obj.Data = "";
            obj.Tag = 1;
            return obj;
        }

        public async Task InitializationStarted()
        {
            await analysisInitializationService.InitializationStarted();
        }

        public async Task InitializationPause()
        {
            await analysisInitializationService.InitializationPause();
        }

        public async Task InitializationTermination()
        {
            await analysisInitializationService.InitializationTermination();
        }

        public async Task LoadingIC()
        {
            await analysisInitializationService.LoadingIC();
        }

        public async Task DataSynchronization()
        {
            await analysisInitializationService.DataSynchronization();
        }





        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(AnalysisInitializationEntity entity)
        {
            TData<string> obj = new TData<string>();
            await analysisInitializationService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await analysisInitializationService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
