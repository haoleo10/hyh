using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Entity.DataCleaningManage;
using YiSha.Model.Param.DataCleaningManage;
using YiSha.Service.DataCleaningManage;

namespace YiSha.Business.DataCleaningManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2025-05-14 14:23
    /// 描 述：数据清洗业务类
    /// </summary>
    public class DataCleaningBLL
    {
        private DataCleaningService dataCleaningService = new DataCleaningService();

        #region 获取数据
        public async Task<TData<List<DataCleaningEntity>>> GetList(DataCleaningListParam param)
        {
            TData<List<DataCleaningEntity>> obj = new TData<List<DataCleaningEntity>>();
            obj.Data = await dataCleaningService.GetList(param);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<DataCleaningEntity>>> GetPageList(DataCleaningListParam param, Pagination pagination)
        {
            TData<List<DataCleaningEntity>> obj = new TData<List<DataCleaningEntity>>();
            obj.Data = await dataCleaningService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<DataCleaningEntity>> GetEntity(long id)
        {
            TData<DataCleaningEntity> obj = new TData<DataCleaningEntity>();
            obj.Data = await dataCleaningService.GetEntity(id);
            if (obj.Data != null)
            {
                obj.Tag = 1;
            }
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(DataCleaningEntity entity)
        {
            TData<string> obj = new TData<string>();
            await dataCleaningService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await dataCleaningService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
