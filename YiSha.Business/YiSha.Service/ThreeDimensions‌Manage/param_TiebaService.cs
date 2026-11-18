using NPOI.SS.Formula.Functions;
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
using YiSha.Web.Code;

namespace YiSha.Service.ThreeDimensions‌Manage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2026-05-19 15:37
    /// 描 述：贴吧服务类
    /// </summary>
    public class param_TiebaService :  RepositoryFactory
    {
        #region 获取数据
        public async Task<List<param_TiebaEntity>> GetList(param_TiebaListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<param_TiebaEntity>> GetPageList(param_TiebaListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list= await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<param_TiebaEntity> GetEntity(long id)
        {
            OperatorInfo operatorInfo = await Operator.Instance.Current();
            ParamAlertsEntity paramAlertsEntity = await this.BaseRepository().FindEntity<ParamAlertsEntity>(id);
            param_TiebaEntity param_TiebaEntity = new param_TiebaEntity();
            if (paramAlertsEntity != null)
            {
                param_TiebaEntity.Param_alerts_Id = paramAlertsEntity.Id;
                param_TiebaEntity.Name = "二层" + paramAlertsEntity.Param_Key + "报警：KNS("+ paramAlertsEntity.Param_Value + ")和3Ｋ("+ paramAlertsEntity.Param_Value1 + ")差值相差" + Math.Abs(float.Parse(paramAlertsEntity.Param_Value) - float.Parse(paramAlertsEntity.Param_Value1)).ToString();
                param_TiebaEntity.Param_Key = paramAlertsEntity.Param_Key;
                param_TiebaEntity.Param_Value = paramAlertsEntity.Param_Value;
                param_TiebaEntity.Param_Value1 = paramAlertsEntity.Param_Value1;
                param_TiebaEntity.Source_SYS = paramAlertsEntity.Source_SYS;
                param_TiebaEntity.BaseCreateTime = paramAlertsEntity.BaseCreateTime;
                param_TiebaEntity.BaseCreateName = operatorInfo.UserName;
                return param_TiebaEntity;
            }
            else
            {
                return null;           
            }     
          
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(param_TiebaEntity entity)
        {
            OperatorInfo operatorInfo = await Operator.Instance.Current();
            if (entity.Id.IsNullOrZero())
            {
                await entity.Create();
                entity.BaseCreateName = operatorInfo.UserName;
                await this.BaseRepository().Insert(entity);
            }
            else
            {
                await entity.Modify();
                entity.BaseCreateName = operatorInfo.UserName;
                await this.BaseRepository().Update(entity);
            }
        }

        public async Task DeleteForm(string ids)
        {
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            await this.BaseRepository().Delete<param_TiebaEntity>(idArr);
        }
        #endregion

        #region 私有方法
        private Expression<Func<param_TiebaEntity, bool>> ListFilter(param_TiebaListParam param)
        {
            var expression = LinqExtensions.True<param_TiebaEntity>();
            if (param != null)
            {
            }
            return expression;
        }
        #endregion
    }
}
