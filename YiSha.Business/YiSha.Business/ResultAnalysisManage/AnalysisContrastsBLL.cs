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

namespace YiSha.Business.ResultAnalysisManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2024-08-19 15:07
    /// 描 述：结果分析(状态对比分析)业务类
    /// </summary>
    public class AnalysisContrastsBLL
    {
        private AnalysisContrastsService analysisContrastsService = new AnalysisContrastsService();

        #region 获取数据
        public async Task<TData<List<AnalysisContrastsEntity>>> GetList(AnalysisContrastsListParam param)
        {
            TData<List<AnalysisContrastsEntity>> obj = new TData<List<AnalysisContrastsEntity>>();
            obj.Data = await analysisContrastsService.GetList(param);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<AnalysisContrastsEntity>>> GetPageList(AnalysisContrastsListParam param, Pagination pagination)
        {
            TData<List<AnalysisContrastsEntity>> obj = new TData<List<AnalysisContrastsEntity>>();
            obj.Data = await analysisContrastsService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<AnalysisContrastsEntity>> GetEntity(long id)
        {
            TData<AnalysisContrastsEntity> obj = new TData<AnalysisContrastsEntity>();
            obj.Data = await analysisContrastsService.GetEntity(id);
            if (obj.Data != null)
            {
                obj.Tag = 1;
            }
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(AnalysisContrastsEntity entity)
        {
            TData<string> obj = new TData<string>();
            await analysisContrastsService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await analysisContrastsService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
