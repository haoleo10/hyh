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
            // 从预报警表读取记录，复制到萃取表，并把预报警标记为已萃取（Status=2）
            PreAlertsEntity preAlertsEntity = await this.BaseRepository().FindEntity<PreAlertsEntity>(long.Parse(id));

            AlertExtractionEntity alertExtractionEntit=new AlertExtractionEntity();
            alertExtractionEntit.Name = preAlertsEntity.Name;
            alertExtractionEntit.Param_Key= preAlertsEntity.Param_Key;
            alertExtractionEntit.Param_Value=preAlertsEntity.Param_Value;
            alertExtractionEntit.Param_Value1 = preAlertsEntity.Param_Value1;
            alertExtractionEntit.Source_SYS=preAlertsEntity.Source_SYS;
            alertExtractionEntit.Status = 1;
            alertExtractionEntit.AlarmLayer = preAlertsEntity.AlarmLayer;
            // 保留原始报警产生时间
            alertExtractionEntit.BaseCreateTime = preAlertsEntity.BaseCreateTime;
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

            PreAlertsService preAlertsService = new PreAlertsService();
            await preAlertsService.UpdateStatus(preAlertsEntity.Id.Value, 2);
        }

        public async Task CompleteExtraction(string id) 
        {
            AlertExtractionEntity alertExtractionEntity = await this.BaseRepository().FindEntity<AlertExtractionEntity>(long.Parse(id));
            alertExtractionEntity.Status = 2;
            await alertExtractionEntity.Modify();
            await this.BaseRepository().Update(alertExtractionEntity);

            // 确认萃取后写入报警列表（param_alerts），保留原始报警产生时间
            ParamAlertsEntity alarm = new ParamAlertsEntity
            {
                Name = alertExtractionEntity.Name,
                Param_Key = alertExtractionEntity.Param_Key,
                Param_Value = alertExtractionEntity.Param_Value,
                Param_Value1 = alertExtractionEntity.Param_Value1,
                Source_SYS = alertExtractionEntity.Source_SYS,
                Remark = alertExtractionEntity.Remark,
                AlarmLayer = alertExtractionEntity.AlarmLayer,
                BaseCreateTime = alertExtractionEntity.BaseCreateTime
            };
            ParamAlertsService paramAlertsService = new ParamAlertsService();
            await paramAlertsService.SaveForm(alarm);
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
