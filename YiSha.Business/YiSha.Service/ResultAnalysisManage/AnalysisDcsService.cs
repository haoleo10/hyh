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
    /// 日 期：2024-09-09 09:21
    /// 描 述：DCS对比分析服务类
    /// </summary>
    public class AnalysisDcsService :  RepositoryFactory
    {
        #region 获取数据
        public async Task<List<AnalysisDcsEntity>> GetList(AnalysisDcsListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<AnalysisDcsEntity>> GetPageList(AnalysisDcsListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list= await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<AnalysisDcsEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<AnalysisDcsEntity>(id);
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(AnalysisDcsEntity entity)
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
            await this.BaseRepository().Delete<AnalysisDcsEntity>(idArr);
        }
        #endregion

        #region 私有方法
        private Expression<Func<AnalysisDcsEntity, bool>> ListFilter(AnalysisDcsListParam param)
        {
            var expression = LinqExtensions.True<AnalysisDcsEntity>();
            if (param != null)
            {
            }
            return expression;
        }
        #endregion
    }
}
