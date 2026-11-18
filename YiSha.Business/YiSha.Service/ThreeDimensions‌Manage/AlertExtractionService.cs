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
    /// 日 期：2026-06-17 14:39
    /// 描 述：萃取服务类
    /// </summary>
    public class AlertExtractionService :  RepositoryFactory
    {
        #region 获取数据
        public async Task<List<AlertExtractionEntity>> GetList(AlertExtractionListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<AlertExtractionEntity>> GetPageList(AlertExtractionListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list= await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<List<AlertExtractionEntity>> CompleteGetPageListJson(AlertExtractionListParam param, Pagination pagination)
        {
            var expression = CompleteListFilter(param);
            var list = await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<AlertExtractionEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<AlertExtractionEntity>(id);
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(AlertExtractionEntity entity)
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
            await this.BaseRepository().Delete<AlertExtractionEntity>(idArr);
        }


        public async Task Extraction(string id)
        {
            ParamAlertsEntity paramAlertsEntit = await this.BaseRepository().FindEntity<ParamAlertsEntity>(long.Parse(id));

            AlertExtractionEntity alertExtractionEntit=new AlertExtractionEntity();
            alertExtractionEntit.Name = paramAlertsEntit.Name;
            alertExtractionEntit.Param_Key= paramAlertsEntit.Param_Key;
            alertExtractionEntit.Param_Value=paramAlertsEntit.Param_Value;
            alertExtractionEntit.Param_Value1 = paramAlertsEntit.Param_Value1;
            alertExtractionEntit.Source_SYS=paramAlertsEntit.Source_SYS;
            alertExtractionEntit.Status = 1;
            if (alertExtractionEntit.Id.IsNullOrZero())
            {
                await alertExtractionEntit.Create();
                await this.BaseRepository().Insert(alertExtractionEntit);
            }
            else
            {
                await alertExtractionEntit.Modify();
                await this.BaseRepository().Update(alertExtractionEntit);
            }
        }

        public async Task CompleteExtraction(string id) 
        {
            AlertExtractionEntity alertExtractionEntity = await this.BaseRepository().FindEntity<AlertExtractionEntity>(long.Parse(id));
            alertExtractionEntity.Status = 2;
            await alertExtractionEntity.Modify();
            await this.BaseRepository().Update(alertExtractionEntity);
        }
        
        #endregion

        #region 私有方法
        private Expression<Func<AlertExtractionEntity, bool>> ListFilter(AlertExtractionListParam param)
        {
            var expression = LinqExtensions.True<AlertExtractionEntity>();
            expression = expression.And(t => t.Status == 1);
            if (param != null)
            {
            }
            return expression;
        }

        private Expression<Func<AlertExtractionEntity, bool>> CompleteListFilter(AlertExtractionListParam param)
        {
            var expression = LinqExtensions.True<AlertExtractionEntity>();
            expression = expression.And(t => t.Status == 2);
            if (param != null)
            {
            }
            return expression;
        }
        #endregion
    }
}
