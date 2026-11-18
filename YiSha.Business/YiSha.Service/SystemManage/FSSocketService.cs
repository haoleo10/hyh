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
using YiSha.Entity.SystemManage;
using YiSha.Model.Param.SystemManage;
using Org.BouncyCastle.Crypto;

namespace YiSha.Service.SystemManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2024-10-31 14:56
    /// 描 述：数据通信服务类
    /// </summary>
    public class FSSocketService :  RepositoryFactory
    {
        #region 获取数据
        public async Task<List<FSSocketEntity>> GetList(FSSocketListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();            
        }

        public async Task<List<FSSocketEntity>> GetPageList(FSSocketListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list= await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<FSSocketEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<FSSocketEntity>(id);
        }

        public async Task<FSSocketEntity> GetEntity()
        {
            var expression = LinqExtensions.True<FSSocketEntity>();
            return  this.BaseRepository().IQueryable(expression).FirstOrDefault(); ;
        }

        public async Task<DynamicObjectGroupEntity> GetDynamicObjectGroup(string Name)
        {
            var expression = LinqExtensions.True<DynamicObjectGroupEntity>();
            expression = expression.And(t => t.ProtName == Name);
            return  this.BaseRepository().IQueryable(expression).FirstOrDefault();
        }

        public async Task<List<DynamicObjectGroupEntity>> GetDynamicObjectGroups(string Name)
        {
            List<DynamicObjectGroupEntity> dynamicObjectGroupEntities=new List<DynamicObjectGroupEntity>();
            
            string[] idArr = TextHelper.SplitToArray<string>(Name, ',');
            foreach (var item in idArr)
            {
                DynamicObjectGroupEntity dynamicObjectGroupEntity=new DynamicObjectGroupEntity();
                var expression = LinqExtensions.True<DynamicObjectGroupEntity>();

                expression = expression.And(t => t.ProtName== item);
                dynamicObjectGroupEntity = this.BaseRepository().IQueryable(expression).FirstOrDefault();
                if (dynamicObjectGroupEntity != null)
                {
                    dynamicObjectGroupEntities.Add(dynamicObjectGroupEntity);
                }
            }
            return dynamicObjectGroupEntities;
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(FSSocketEntity entity)
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
            await this.BaseRepository().Delete<FSSocketEntity>(idArr);
        }
        #endregion

        #region 私有方法
        private Expression<Func<FSSocketEntity, bool>> ListFilter(FSSocketListParam param)
        {
            var expression = LinqExtensions.True<FSSocketEntity>();
            if (param != null)
            {
            }
            return expression;
        }
        #endregion
    }
}
