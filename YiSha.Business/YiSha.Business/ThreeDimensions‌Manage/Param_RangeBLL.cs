using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Entity.ThreeDimensions‌Manage;
using YiSha.Model.Param.ThreeDimensions‌Manage;
using YiSha.Service.ThreeDimensions‌Manage;

namespace YiSha.Business.ThreeDimensions‌Manage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2026-08-06 15:35
    /// 描 述：量程业务类
    /// </summary>
    public class Param_RangeBLL
    {
        private Param_RangeService param_RangeService = new Param_RangeService();

        #region 获取数据
        public async Task<TData<List<Param_RangeEntity>>> GetList(ListParam_RangeListParam param)
        {
            TData<List<Param_RangeEntity>> obj = new TData<List<Param_RangeEntity>>();
            obj.Data = await param_RangeService.GetList(param);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<Param_RangeEntity>>> GetPageList(ListParam_RangeListParam param, Pagination pagination)
        {
            TData<List<Param_RangeEntity>> obj = new TData<List<Param_RangeEntity>>();
            obj.Data = await param_RangeService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<Param_RangeEntity>> GetEntity(long id)
        {
            TData<Param_RangeEntity> obj = new TData<Param_RangeEntity>();
            obj.Data = await param_RangeService.GetEntity(id);
            if (obj.Data != null)
            {
                obj.Tag = 1;
            }
            return obj;
        }

        public async Task<TData<Param_RangeEntity>> GetRangeEntity(string dotName, string Value)
        {
            TData<Param_RangeEntity> obj = new TData<Param_RangeEntity>();
            obj.Data = await param_RangeService.GetRangeEntity(dotName, Value);
            if (obj.Data != null)
            {
                obj.Tag = 1;
            }
            return obj;
        }
        


        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(Param_RangeEntity entity)
        {
            TData<string> obj = new TData<string>();
            await param_RangeService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await param_RangeService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
