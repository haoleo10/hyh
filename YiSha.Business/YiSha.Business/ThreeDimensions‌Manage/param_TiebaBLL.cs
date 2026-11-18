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
    /// 日 期：2026-05-19 15:37
    /// 描 述：贴吧业务类
    /// </summary>
    public class param_TiebaBLL
    {
        private param_TiebaService param_TiebaService = new param_TiebaService();

        #region 获取数据
        public async Task<TData<List<param_TiebaEntity>>> GetList(param_TiebaListParam param)
        {
            TData<List<param_TiebaEntity>> obj = new TData<List<param_TiebaEntity>>();
            obj.Data = await param_TiebaService.GetList(param);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<param_TiebaEntity>>> GetPageList(param_TiebaListParam param, Pagination pagination)
        {
            TData<List<param_TiebaEntity>> obj = new TData<List<param_TiebaEntity>>();
            obj.Data = await param_TiebaService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<param_TiebaEntity>> GetEntity(long id)
        {
            TData<param_TiebaEntity> obj = new TData<param_TiebaEntity>();
            obj.Data = await param_TiebaService.GetEntity(id);
            if (obj.Data != null)
            {
                obj.Tag = 1;
            }
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(param_TiebaEntity entity)
        {
            TData<string> obj = new TData<string>();
            await param_TiebaService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await param_TiebaService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
