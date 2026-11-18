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
    /// 日 期：2026-08-06 15:35
    /// 描 述：量程服务类
    /// </summary>
    public class Param_RangeService :  RepositoryFactory
    {
        #region 获取数据
        public async Task<List<Param_RangeEntity>> GetList(ListParam_RangeListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<Param_RangeEntity>> GetPageList(ListParam_RangeListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list= await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<Param_RangeEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<Param_RangeEntity>(id);
        }

        public async Task<Param_RangeEntity> GetRangeEntity(string dotName,string Value)
        {
            var expressionThreshold = LinqExtensions.True<param_ThresholdEntity>();
            expressionThreshold = expressionThreshold.And(t => t.Rollcall == dotName);
            param_ThresholdEntity param_ThresholdEntity = this.BaseRepository().IQueryable(expressionThreshold).FirstOrDefault();
            
            if (param_ThresholdEntity.Value != int.Parse(Value))
            {
                param_ThresholdEntity.Value = int.Parse(Value);

                await param_ThresholdEntity.Modify();
                await this.BaseRepository().Update(param_ThresholdEntity);
            }           

            var expression = ListFilter(dotName);
            Param_RangeEntity param =this.BaseRepository().IQueryable(expression).FirstOrDefault();
            return param;        
        }

        public async Task<Param_RangeEntity> GetRangeEntity(string dotName)
        {
            var expression = ListFilter(dotName);
            Param_RangeEntity param = this.BaseRepository().IQueryable(expression).FirstOrDefault();
            return param;
        }


        #endregion

        #region 提交数据
        public async Task SaveForm(Param_RangeEntity entity)
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
            await this.BaseRepository().Delete<Param_RangeEntity>(idArr);
        }
        #endregion

        #region 私有方法
        private Expression<Func<Param_RangeEntity, bool>> ListFilter(ListParam_RangeListParam param)
        {
            var expression = LinqExtensions.True<Param_RangeEntity>();
            if (param != null)
            {
            }
            return expression;
        }


        private Expression<Func<Param_RangeEntity, bool>> ListFilter(string dotName)
        {
            var expression = LinqExtensions.True<Param_RangeEntity>();
            expression = expression.And(t => t.DotName.Contains(dotName));
            return expression;
        }
        #endregion
    }
}
