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
using YiSha.Entity.IntelligentSearchManage;
using YiSha.Model.Param.IntelligentSearchManage;
using YiSha.Entity.SystemManage;

namespace YiSha.Service.IntelligentSearchManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2024-08-20 14:39
    /// 描 述：智能搜索服务类
    /// </summary>
    public class IntelligentSearchService :  RepositoryFactory
    {
        #region 获取数据
        public async Task<List<IntelligentSearchEntity>> GetList(IntelligentSearchListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<IntelligentSearchEntity>> GetPageList(IntelligentSearchListParam param, Pagination pagination)
        {
            List<DynamicObjectGroupEntity> dynamicObjectGroupEntities=new List<DynamicObjectGroupEntity>();
            List<IntelligentSearchEntity> intelligentSearchEntities=new List<IntelligentSearchEntity>();
           var expressionDynamic = LinqExtensions.True<DynamicObjectGroupEntity>();
            if (!string.IsNullOrWhiteSpace(param.KeyWorkName))
            {
                expressionDynamic = expressionDynamic.And(t => t.ProtName.Contains(param.KeyWorkName));
            }
            else
            {
                expressionDynamic = expressionDynamic.And(t => t.ProtName.Contains(",,,"));
            }
            dynamicObjectGroupEntities=this.BaseRepository().IQueryable(expressionDynamic).ToList();
            foreach (var item in dynamicObjectGroupEntities)
            {
                IntelligentSearchEntity intelligentSearchEntity=new IntelligentSearchEntity();
                intelligentSearchEntity.Device = item.ProtName; 
                intelligentSearchEntity.CharacteristicValue = item.ProtVelue; 
                intelligentSearchEntities.Add(intelligentSearchEntity);
            }
            // var expression = ListFilter(param);

            //var list= await this.BaseRepository().FindList(expression, pagination);
            return intelligentSearchEntities.ToList();
        }

        public async Task<IntelligentSearchEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<IntelligentSearchEntity>(id);
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(IntelligentSearchEntity entity)
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
            await this.BaseRepository().Delete<IntelligentSearchEntity>(idArr);
        }
        #endregion

        #region 私有方法
        private Expression<Func<IntelligentSearchEntity, bool>> ListFilter(IntelligentSearchListParam param)
        {
            var expression = LinqExtensions.True<IntelligentSearchEntity>();
            if (param != null)
            {

            }
            return expression;
        }
        #endregion
    }
}
