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
using YiSha.Entity.ThermalPerformanceManage;
using YiSha.Model.Param.ThermalPerformanceManage;
using Newtonsoft.Json;
using YiSha.Entity.SystemManage;
using YiSha.Service.ResultAnalysisManage;
using YiSha.Entity.ResultAnalysisManage;

namespace YiSha.Service.ThermalPerformanceManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2024-08-19 15:23
    /// 描 述：热效率分析服务类
    /// </summary>
    public class AfficiencyAnalysisService :  RepositoryFactory
    {
        #region 获取数据
        public async Task<List<AfficiencyAnalysisEntity>> GetList(AfficiencyAnalysisListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<AfficiencyAnalysisEntity>> GetPageList(AfficiencyAnalysisListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            try
            {
                var list = await this.BaseRepository().FindList(expression, pagination);
                return list.ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        public async Task<AfficiencyAnalysisEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<AfficiencyAnalysisEntity>(id);
        }
   
        public async Task<List<long>> GetKS()
        {
            var expressionFS = LinqExtensions.True<FSSocketEntity>();
            FSSocketEntity fSSocketEntity = this.BaseRepository().IQueryable(expressionFS).FirstOrDefault();
            SocketObj person = new SocketObj();
            person = JsonConvert.DeserializeObject<SocketObj>(fSSocketEntity.Jsons);
            //接收值
            List<long> list = new List<long>();
            if (person != null && person.body.ArrData.Count > 0)
            {
                for (int i = 0; i < person.body.ArrData.Count; i++)
                {
                    //if (i == 0)
                    //{
                    //    //汽轮机发电机组输入热量
                    //    list.Add(person.body.ArrData[i].ToString());
                    //}
                    //if (i == 1)
                    //{ 
                    //    //发电量热当量
                    //    list.Add(person.body.ArrData[i].ToString());
                    //}
                    //if (i == 2)
                    //{
                    //    //发电机损失热量
                    //    list.Add(person.body.ArrData[i].ToString());
                    //}
                    //if (i == 3)
                    //{
                    //    //汽轮机损失热量
                    //    list.Add(person.body.ArrData[i].ToString());
                    //}                    
                }
                return person.body.ArrData;
            }
            else
            { 
                return new List<long>();
            }
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(AfficiencyAnalysisEntity entity)
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
            await this.BaseRepository().Delete<AfficiencyAnalysisEntity>(idArr);
        }
        #endregion

        #region 私有方法
        private Expression<Func<AfficiencyAnalysisEntity, bool>> ListFilter(AfficiencyAnalysisListParam param)
        {
            var expression = LinqExtensions.True<AfficiencyAnalysisEntity>();
            if (param != null)
            {
            }
            return expression;
        }
        #endregion
    }
}
