using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Entity.ThermalPerformanceManage;
using YiSha.Model.Param.ThermalPerformanceManage;
using YiSha.Service.ThermalPerformanceManage;

namespace YiSha.Business.ThermalPerformanceManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2024-08-19 15:23
    /// 描 述：热效率分析业务类
    /// </summary>
    public class AfficiencyAnalysisBLL
    {
        private AfficiencyAnalysisService afficiencyAnalysisService = new AfficiencyAnalysisService();

        #region 获取数据
        public async Task<TData<List<AfficiencyAnalysisEntity>>> GetList(AfficiencyAnalysisListParam param)
        {
            TData<List<AfficiencyAnalysisEntity>> obj = new TData<List<AfficiencyAnalysisEntity>>();
            obj.Data = await afficiencyAnalysisService.GetList(param);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<AfficiencyAnalysisEntity>>> GetPageList(AfficiencyAnalysisListParam param, Pagination pagination)
        {
            TData<List<AfficiencyAnalysisEntity>> obj = new TData<List<AfficiencyAnalysisEntity>>();
            obj.Data = await afficiencyAnalysisService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<AfficiencyAnalysisEntity>> GetEntity(long id)
        {
            TData<AfficiencyAnalysisEntity> obj = new TData<AfficiencyAnalysisEntity>();
            obj.Data = await afficiencyAnalysisService.GetEntity(id);
            if (obj.Data != null)
            {
                obj.Tag = 1;
            }
            return obj;
        }

        //
        public async Task<TData<List<long>>> GetKS()
        {
            TData<List<long>> obj = new TData<List<long>>();
            obj.Data = await afficiencyAnalysisService.GetKS();
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(AfficiencyAnalysisEntity entity)
        {
            TData<string> obj = new TData<string>();
            await afficiencyAnalysisService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await afficiencyAnalysisService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
