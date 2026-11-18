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
    /// 日 期：2024-09-09 09:23
    /// 描 述：特征量对比分析业务类
    /// </summary>
    public class AnalysisTzlBLL
    {
        private AnalysisTzlService analysisTzlService = new AnalysisTzlService();

        #region 获取数据
        public async Task<TData<List<AnalysisTzlEntity>>> GetList(AnalysisTzlListParam param)
        {
            TData<List<AnalysisTzlEntity>> obj = new TData<List<AnalysisTzlEntity>>();
            obj.Data = await analysisTzlService.GetList(param);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<AnalysisTzlEntity>>> GetPageList(AnalysisTzlListParam param, Pagination pagination)
        {
            TData<List<AnalysisTzlEntity>> obj = new TData<List<AnalysisTzlEntity>>();
            obj.Data = await analysisTzlService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<AnalysisTzlEntity>> GetEntity(long id)
        {
            TData<AnalysisTzlEntity> obj = new TData<AnalysisTzlEntity>();
            obj.Data = await analysisTzlService.GetEntity(id);
            if (obj.Data != null)
            {
                obj.Tag = 1;
            }
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(AnalysisTzlEntity entity)
        {
            TData<string> obj = new TData<string>();
            await analysisTzlService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await analysisTzlService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
