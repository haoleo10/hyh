using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YiSha.Data;
using YiSha.Data.Repository;
using YiSha.Entity.ThreeDimensions‌Manage;
using YiSha.Model.Param.ThreeDimensions‌Manage;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;

namespace YiSha.Service.ThreeDimensions‌Manage
{
    /// <summary>
    /// 描 述：预报警列表服务类
    /// </summary>
    public class PreAlertsService : RepositoryFactory
    {
        #region 获取数据
        public async Task<List<PreAlertsEntity>> GetList(ListParamAlertsListParam param, string AlarmLayer, string SYS)
        {
            var expression = ListFilter(param, AlarmLayer, SYS);
            var list = await this.BaseRepository().FindList(expression);
            return list.OrderByDescending(t => t.BaseCreateTime).ToList();
        }

        public async Task<List<PreAlertsEntity>> GetPageList(ListParamAlertsListParam param, Pagination pagination, string AlarmLayer, string SYS)
        {
            var expression = ListFilter(param, AlarmLayer, SYS);
            var list = await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<PreAlertsEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<PreAlertsEntity>(id);
        }

        /// <summary>
        /// AlertBJ 去重查询（按层级+系统过滤未删除记录）
        /// </summary>
        public async Task<List<PreAlertsEntity>> GetLists(string AlarmLayer, string SYS)
        {
            var expressionFS = LinqExtensions.True<PreAlertsEntity>();

            if (AlarmLayer == "1")
            {

            }
            else if (AlarmLayer == "2")
            {
                expressionFS = expressionFS.And(t => t.AlarmLayer == int.Parse(AlarmLayer) && t.Source_SYS.Contains(SYS));
            }
            else if (AlarmLayer == "3")
            {
                string[] idArr = TextHelper.SplitToArray<string>(SYS, ',');
                expressionFS = expressionFS.And(t => idArr.Contains(t.Param_Key));
            }

            List<PreAlertsEntity> list = this.BaseRepository().IQueryable(expressionFS).ToList();
            return list;
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(PreAlertsEntity entity)
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

        /// <summary>
        /// 更新预报警状态（1:已确认报警；2:已萃取）
        /// </summary>
        public async Task UpdateStatus(long id, int status)
        {
            PreAlertsEntity entity = await this.BaseRepository().FindEntity<PreAlertsEntity>(id);
            if (entity != null)
            {
                entity.Status = status;
                await entity.Modify();
                await this.BaseRepository().Update(entity);
            }
        }

        public async Task DeleteForm(string ids)
        {
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            await this.BaseRepository().Delete<PreAlertsEntity>(idArr);
        }
        #endregion

        #region 私有方法
        private Expression<Func<PreAlertsEntity, bool>> ListFilter(ListParamAlertsListParam param, string AlarmLayer, string SYS)
        {
            string[] idArr = null;
            if (!string.IsNullOrWhiteSpace(SYS))
            {
                idArr = TextHelper.SplitToArray<string>(SYS, ',');
            }

            var expression = LinqExtensions.True<PreAlertsEntity>();
            // 只显示未确认（Status=0）的预报警
            expression = expression.And(t => t.Status == 0);
            if (AlarmLayer == "2" && !string.IsNullOrWhiteSpace(SYS))
            {
                expression = expression.And(t => t.AlarmLayer == int.Parse(AlarmLayer) && t.Source_SYS.Contains(SYS));
            }

            if (AlarmLayer == "3")
            {
                expression = expression.And(t => t.AlarmLayer == int.Parse(AlarmLayer) && idArr.Contains(t.Param_Key));
            }
            return expression;
        }
        #endregion
    }
}
