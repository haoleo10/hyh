using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YiSha.Data;
using YiSha.Data.Repository;
using YiSha.Entity.SystemManage;
using YiSha.Entity.ThreeDimensions‌Manage;
using YiSha.Model.Param.ThreeDimensions‌Manage;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;

namespace YiSha.Service.ThreeDimensions‌Manage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2026-05-19 10:58
    /// 描 述：报警列表服务类
    /// </summary>
    public class ParamAlertsService :  RepositoryFactory
    {
        #region 获取数据
        public async Task<List<ParamAlertsEntity>> GetList(ListParamAlertsListParam param,string AlarmLayer,string SYS)
        {
            var expression = ListFilter(param, AlarmLayer, SYS);
            var list = await this.BaseRepository().FindList(expression);
            return list.OrderByDescending(t=>t.BaseCreateTime).ToList();
        }

        public async Task<List<ParamAlertsEntity>> GetPageList(ListParamAlertsListParam param, Pagination pagination, string AlarmLayer,string SYS)
        {
            var expression = ListFilter(param, AlarmLayer, SYS);
            var list= await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<ParamAlertsEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<ParamAlertsEntity>(id);
        }

        public async Task<List<ParamAlertsEntity>> GetLists(string AlarmLayer,string SYS)
        {
            var expressionFS = LinqExtensions.True<ParamAlertsEntity>();
           
            if (AlarmLayer == "1")
            {
                              
            }
            else if (AlarmLayer == "2")
            {
                expressionFS = expressionFS.And(t => t.AlarmLayer == int.Parse(AlarmLayer) && t.Source_SYS.Contains(SYS));
            }
            else if(AlarmLayer == "3")
            {
                string[] idArr = TextHelper.SplitToArray<string>(SYS, ',');
                expressionFS = expressionFS.And(t => idArr.Contains(t.Param_Key));

            }

              List<ParamAlertsEntity> list = this.BaseRepository().IQueryable(expressionFS).ToList();        
              return list;
        }

        public async Task<List<ParamAlertsEntity>> GetListNumber(string SYS)
        {
            var expressionFS = LinqExtensions.True<ParamAlertsEntity>();       
            expressionFS = expressionFS.And(t=>t.Source_SYS.Contains(SYS));
            List<ParamAlertsEntity> list = this.BaseRepository().IQueryable(expressionFS).ToList();
            return list;
        }

        #endregion

        #region 提交数据
        public async Task SaveForm(ParamAlertsEntity entity)
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
            await this.BaseRepository().Delete<ParamAlertsEntity>(idArr);
        }
        #endregion

        #region 私有方法
        private Expression<Func<ParamAlertsEntity, bool>> ListFilter(ListParamAlertsListParam param,string AlarmLayer,string SYS)
        {
            string[] idArr=null;
            if (!string.IsNullOrWhiteSpace(SYS))
            {
                idArr = TextHelper.SplitToArray<string>(SYS, ',');

            }

            var expression = LinqExtensions.True<ParamAlertsEntity>();
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
