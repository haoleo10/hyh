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
using YiSha.Entity.ResultAnalysisManage;
using YiSha.Model.Param.ResultAnalysisManage;

namespace YiSha.Service.ResultAnalysisManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2024-08-19 15:07
    /// 描 述：结果分析(状态对比分析)服务类
    /// </summary>
    public class AnalysisContrastsService :  RepositoryFactory
    {
        #region 获取数据
        public async Task<List<AnalysisContrastsEntity>> GetList(AnalysisContrastsListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<AnalysisContrastsEntity>> GetPageList(AnalysisContrastsListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list= await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<AnalysisContrastsEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<AnalysisContrastsEntity>(id);
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(AnalysisContrastsEntity entity)
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
            await this.BaseRepository().Delete<AnalysisContrastsEntity>(idArr);
        }
        #endregion

        #region 私有方法
        private Expression<Func<AnalysisContrastsEntity, bool>> ListFilter(AnalysisContrastsListParam param)
        {
            var expression = LinqExtensions.True<AnalysisContrastsEntity>();
            if (param != null)
            {
            }
            return expression;
        }
        #endregion
    }
}
