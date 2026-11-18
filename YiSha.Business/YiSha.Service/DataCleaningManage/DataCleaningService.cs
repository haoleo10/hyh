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
using YiSha.Entity.DataCleaningManage;
using YiSha.Model.Param.DataCleaningManage;

namespace YiSha.Service.DataCleaningManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2025-05-14 14:23
    /// 描 述：数据清洗服务类
    /// </summary>
    public class DataCleaningService :  RepositoryFactory
    {
        #region 获取数据
        public async Task<List<DataCleaningEntity>> GetList(DataCleaningListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<DataCleaningEntity>> GetPageList(DataCleaningListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list= await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<DataCleaningEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<DataCleaningEntity>(id);
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(DataCleaningEntity entity)
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
            await this.BaseRepository().Delete<DataCleaningEntity>(idArr);
        }
        #endregion

        #region 私有方法
        private Expression<Func<DataCleaningEntity, bool>> ListFilter(DataCleaningListParam param)
        {
            var expression = LinqExtensions.True<DataCleaningEntity>();
            if (param != null)
            {
            }
            return expression;
        }
        #endregion
    }
}
