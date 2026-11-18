using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Entity.IntelligentSearchManage;
using YiSha.Model.Param.IntelligentSearchManage;
using YiSha.Service.IntelligentSearchManage;

namespace YiSha.Business.IntelligentSearchManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2024-08-20 14:39
    /// 描 述：智能搜索业务类
    /// </summary>
    public class IntelligentSearchBLL
    {
        private IntelligentSearchService intelligentSearchService = new IntelligentSearchService();

        #region 获取数据
        public async Task<TData<List<IntelligentSearchEntity>>> GetList(IntelligentSearchListParam param)
        {
            TData<List<IntelligentSearchEntity>> obj = new TData<List<IntelligentSearchEntity>>();
            obj.Data = await intelligentSearchService.GetList(param);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<IntelligentSearchEntity>>> GetPageList(IntelligentSearchListParam param, Pagination pagination)
        {
            TData<List<IntelligentSearchEntity>> obj = new TData<List<IntelligentSearchEntity>>();
            obj.Data = await intelligentSearchService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<IntelligentSearchEntity>> GetEntity(long id)
        {
            TData<IntelligentSearchEntity> obj = new TData<IntelligentSearchEntity>();
            obj.Data = await intelligentSearchService.GetEntity(id);
            if (obj.Data != null)
            {
                obj.Tag = 1;
            }
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(IntelligentSearchEntity entity)
        {
            TData<string> obj = new TData<string>();
            await intelligentSearchService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await intelligentSearchService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
