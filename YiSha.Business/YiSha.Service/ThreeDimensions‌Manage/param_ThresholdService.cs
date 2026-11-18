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
using Microsoft.EntityFrameworkCore;

namespace YiSha.Service.ThreeDimensions‌Manage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2026-09-21 15:03
    /// 描 述：报警阈值服务类
    /// </summary>
    public class param_ThresholdService :  RepositoryFactory
    {
        #region 获取数据
        public async Task<List<param_ThresholdEntity>> GetList(param_ThresholdListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<param_ThresholdEntity>> GetPageList(param_ThresholdListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list= await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<param_ThresholdEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<param_ThresholdEntity>(id);
        }

        public async Task<param_ThresholdEntity> GetNameEntity(string Name)
        {         
            var expression = ListNameFilter(Name);
            param_ThresholdEntity param_ThresholdEntity= this.BaseRepository().IQueryable(expression).FirstOrDefault();
            if (param_ThresholdEntity == null)
            {
                SaveNameForm(Name);
            }
            return param_ThresholdEntity;
        }


        public async Task<List<param_ThresholdEntity>> GetListNameEntity(string Name)
        {
            string[] NameArr = TextHelper.SplitToArray<string>(Name, ',');
            var expression = ListNameFilter(NameArr);
            List<param_ThresholdEntity> param_ThresholdEntity = this.BaseRepository().IQueryable(expression).ToList();          
            return param_ThresholdEntity;
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(param_ThresholdEntity entity)
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

        public async Task SaveNameForm(string Name)
        {
            param_ThresholdEntity entity = new param_ThresholdEntity();
            entity.Rollcall = Name;
            entity.Value = 5;
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

        public async Task UpdataSaveNameForm(string Name,string Value)
        {
            var expression = ListNameFilter(Name);
            param_ThresholdEntity param_ThresholdEntity = this.BaseRepository().IQueryable(expression).FirstOrDefault();
            param_ThresholdEntity.Value=int.Parse(Value);
 
            await param_ThresholdEntity.Modify();
            await this.BaseRepository().Update(param_ThresholdEntity);

        }



        public async Task DeleteForm(string ids)
        {
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            await this.BaseRepository().Delete<param_ThresholdEntity>(idArr);
        }
        #endregion

        #region 私有方法
        private Expression<Func<param_ThresholdEntity, bool>> ListFilter(param_ThresholdListParam param)
        {
            var expression = LinqExtensions.True<param_ThresholdEntity>();
            if (param != null)
            {
            }
            return expression;
        }

        private Expression<Func<param_ThresholdEntity, bool>> ListNameFilter(string Name)
        {
            var expression = LinqExtensions.True<param_ThresholdEntity>();
            expression = expression.And(t =>t.Rollcall==Name);
            return expression;
        }

        private Expression<Func<param_ThresholdEntity, bool>> ListNameFilter(string[] Name)
        {
            var expression = LinqExtensions.True<param_ThresholdEntity>();
            expression = expression.And(t => Name.Contains(t.Rollcall));
            return expression;
        }
        #endregion
    }
}
