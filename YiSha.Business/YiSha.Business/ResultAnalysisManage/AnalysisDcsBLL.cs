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
    /// 日 期：2024-09-09 09:21
    /// 描 述：DCS对比分析业务类
    /// </summary>
    public class AnalysisDcsBLL
    {
        private AnalysisDcsService analysisDcsService = new AnalysisDcsService();

        #region 获取数据
        public async Task<TData<List<AnalysisDcsEntity>>> GetList(AnalysisDcsListParam param)
        {
            TData<List<AnalysisDcsEntity>> obj = new TData<List<AnalysisDcsEntity>>();
            obj.Data = await analysisDcsService.GetList(param);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<AnalysisDcsEntity>>> GetPageList(AnalysisDcsListParam param, Pagination pagination)
        {
            TData<List<AnalysisDcsEntity>> obj = new TData<List<AnalysisDcsEntity>>();
            obj.Data = await analysisDcsService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<AnalysisDcsEntity>> GetEntity(long id)
        {
            TData<AnalysisDcsEntity> obj = new TData<AnalysisDcsEntity>();
            obj.Data = await analysisDcsService.GetEntity(id);
            if (obj.Data != null)
            {
                obj.Tag = 1;
            }
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(AnalysisDcsEntity entity)
        {
            TData<string> obj = new TData<string>();
            await analysisDcsService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await analysisDcsService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
