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
    /// 日 期：2026-05-07 15:17
    /// 描 述：二层界面业务类
    /// </summary>
    public class TweFloorBLL
    {
        private TweFloorService tweFloorService = new TweFloorService();

        #region 获取数据
        public async Task<TData<List<TweFloorEntity>>> GetList(TweFloorListParam param)
        {
            TData<List<TweFloorEntity>> obj = new TData<List<TweFloorEntity>>();
            obj.Data = await tweFloorService.GetList(param);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<TweFloorEntity>>> GetPageList(TweFloorListParam param, Pagination pagination)
        {
            TData<List<TweFloorEntity>> obj = new TData<List<TweFloorEntity>>();
            obj.Data = await tweFloorService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<TweFloorEntity>> GetEntity(long id)
        {
            TData<TweFloorEntity> obj = new TData<TweFloorEntity>();
            obj.Data = await tweFloorService.GetEntity(id);
            if (obj.Data != null)
            {
                obj.Tag = 1;
            }
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(TweFloorEntity entity)
        {
            TData<string> obj = new TData<string>();
            await tweFloorService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await tweFloorService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
