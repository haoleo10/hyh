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
using YiSha.Entity.FeatrueQuantityManage;
using YiSha.Model.Param.FeatrueQuantityManage;
using YiSha.Entity.SystemManage;
using YiSha.Service.ResultAnalysisManage;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using MathNet.Numerics;

namespace YiSha.Service.FeatrueQuantityManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2024-08-20 11:09
    /// 描 述：特征量计算服务类
    /// </summary>
    public class FeatrueQuantityService :  RepositoryFactory
    {
        #region 获取数据
        public async Task<List<FeatrueQuantityEntity>> GetList(FeatrueQuantityListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<FeatrueQuantityEntity>> GetPageList(FeatrueQuantityListParam param, Pagination pagination)
        {                          
            var expression = ListFilter(param);
            var list= await this.BaseRepository().FindList(expression, pagination);
           return list.ToList();           
        }


        

        public async Task<List<FeatrueQuantityEntity>> GetPageListChart(FeatrueQuantityListParam param, Pagination pagination)
        {         
            var expression = ListFilterChart(param);
            var list = await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public object GetState()
        {
            #region 获取状态,速度慢 稳定


            //var expressionFS = LinqExtensions.True<FSSocketEntity>();
            //FSSocketEntity fSSocketEntity = this.BaseRepository().IQueryable(expressionFS).FirstOrDefault();
            //SocketObj person = new SocketObj();
            //person = JsonConvert.DeserializeObject<SocketObj>(fSSocketEntity.Jsons);
            //List<string> strings=new List<string>();
            //if (person != null && person.body.ArrData.Count > 853)
            //{
            //    for (int i = 824; i < person.body.ArrData.Count; i++)
            //    {
            //        if (i == 824)
            //        { 
            //            string N = person.body.ArrData[i].ToString();
            //            strings.Add(N);
            //        }
            //        if (i == 825)
            //        {
            //            string N = person.body.ArrData[i].ToString();
            //            strings.Add(N);
            //        }
            //        if (i == 826)
            //        {
            //            string N = person.body.ArrData[i].ToString();
            //            strings.Add(N);
            //        }
            //        if (i == 827)
            //        {
            //            string N = person.body.ArrData[i].ToString();
            //            strings.Add(N);
            //        }
            //        if (i == 828)
            //        {
            //            string N = person.body.ArrData[i].ToString();
            //            strings.Add(N);
            //        }
            //        if (i == 829)
            //        {
            //            string N = person.body.ArrData[i].ToString();
            //            strings.Add(N);
            //        }
            //    }


            //}      
            //return strings;
            #endregion

            #region 速度快 依赖于OGD点表
            var expressionFS = LinqExtensions.True<DynamicObjectGroupEntity>();
            expressionFS=expressionFS.And(t => t.ProtName.Contains("SynPolicySteadyCondtion") || t.ProtName.Contains("SynPolicyLogicSynEd") || t.ProtName.Contains("SynPolicyLSEDelay") || t.ProtName.Contains("SynPolicyMaxDelt")|| t.ProtName.Contains("HYH3IMPORT"));
            List<DynamicObjectGroupEntity> list = this.BaseRepository().IQueryable(expressionFS).ToList();
            if (list.Count > 0)
            {
                string SynPolicySteadyCondtionState = "", SynPolicyLogicSynEdState = "", SynPolicyLSEDelayState = "", SynPolicyMaxDeltState = "", HYH3IMPORTState="";
                foreach (var item in list)
                {
                    if (item.ProtName.Contains("SynPolicySteadyCondtion"))
                    {
                        SynPolicySteadyCondtionState = item.ProtVelue;
                    }
                    if (item.ProtName.Contains("SynPolicyLogicSynEd"))
                    {
                        SynPolicyLogicSynEdState = item.ProtVelue;
                    }
                    if (item.ProtName.Contains("SynPolicyLSEDelay"))
                    {
                        SynPolicyLSEDelayState = item.ProtVelue;
                    }
                    if (item.ProtName.Contains("SynPolicyMaxDelt"))
                    {
                        SynPolicyMaxDeltState = item.ProtVelue;
                    }
                    if (item.ProtName.Contains("HYH3IMPORT"))
                    {
                        HYH3IMPORTState= item.ProtVelue;
                    }


                }
                var state = new
                {
                    SynPolicySteadyCondtionState = SynPolicySteadyCondtionState,
                    SynPolicyLogicSynEdState = SynPolicyLogicSynEdState,
                    SynPolicyLSEDelayState = SynPolicyLSEDelayState,
                    SynPolicyMaxDeltState = SynPolicyMaxDeltState,
                    HYH3IMPORTState=HYH3IMPORTState
                };
                return state;
            }
            else
            { 
                return null;
            }
           
            #endregion
        }

        public async Task<FeatrueQuantityEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<FeatrueQuantityEntity>(id);
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(FeatrueQuantityEntity entity)
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
            await this.BaseRepository().Delete<FeatrueQuantityEntity>(idArr);
        }


        /// <summary>
        /// 开始反算
        /// </summary>
        /// <returns></returns>
        public async Task StartCompute()
        {
            if (GlobalContext.SystemConfig.dsc == false)
            {
                GlobalContext.SystemConfig.dsc = true;
                GlobalContext.SystemConfig.PointName = GlobalContext.SystemConfig.StartCompute;
                GlobalContext.SystemConfig.PointValue = 1;
                GlobalContext.SystemConfig.cmd = 1;
            }
        }

        /// <summary>
        /// 停止反算
        /// </summary>
        /// <returns></returns>
        public async Task StopCompute()
        {
            if (GlobalContext.SystemConfig.dsc == false)
            {
                GlobalContext.SystemConfig.dsc = true;
                GlobalContext.SystemConfig.PointName = GlobalContext.SystemConfig.StopCompute;
                GlobalContext.SystemConfig.PointValue = 1;
                GlobalContext.SystemConfig.cmd = 1;
            }
        }

        #endregion

        #region 私有方法
        private Expression<Func<FeatrueQuantityEntity, bool>> ListFilter(FeatrueQuantityListParam param)
        {
            var expression = LinqExtensions.True<FeatrueQuantityEntity>();
            if (param != null)
            {
                if (param.FeattrueQuantitySysType != "-1")
                {
                    expression = expression.And(t => t.Device.Contains(param.KeyWord) && t.SystemName.Contains(param.FeattrueQuantitySysType));
                }
                else
                {
                    expression = expression.And(t => t.Device.Contains(param.KeyWord));
                }
            }
            return expression;
        }


        private Expression<Func<FeatrueQuantityEntity, bool>> ListFilterChart(FeatrueQuantityListParam param)
        {
            var expression = LinqExtensions.True<FeatrueQuantityEntity>();
            if (param != null)
            {
               
            }
            return expression;
        }
        #endregion
    }
}
