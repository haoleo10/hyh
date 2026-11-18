using System;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Data;
using YiSha.Data.Repository;
using YiSha.Entity.ThreeDimensions‌Manage;
using YiSha.Model.Param.ThreeDimensions‌Manage;

namespace YiSha.Service.ThreeDimensions‌Manage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2026-05-07 15:17
    /// 描 述：二层界面服务类
    /// </summary>
    public class TweFloorService :  RepositoryFactory
    {
        #region 获取数据
        public async Task<List<TweFloorEntity>> GetList(TweFloorListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<TweFloorEntity>> GetPageList(TweFloorListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list= await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<TweFloorEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<TweFloorEntity>(id);
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(TweFloorEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                await entity.Create();
                await this.BaseRepository().Insert(entity);
            }
            else
            {
                await entity.Modify();
                await this.BaseRepository().Update(entity);
            }
        }

        public async Task DeleteForm(string ids)
        {
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            await this.BaseRepository().Delete<TweFloorEntity>(idArr);
        }
        #endregion

        #region 私有方法
        private Expression<Func<TweFloorEntity, bool>> ListFilter(TweFloorListParam param)
        {
            var expression = LinqExtensions.True<TweFloorEntity>();
            if (param != null)
            {
            }
            return expression;
        }
        #endregion
    }
}
