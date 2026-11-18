using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Entity.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Service.SystemManage;

namespace YiSha.Business.SystemManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2024-10-31 14:56
    /// 描 述：数据通信业务类
    /// </summary>
    public class FSSocketBLL
    {
        private FSSocketService fSSocketService = new FSSocketService();

        #region 获取数据
        public async Task<TData<List<FSSocketEntity>>> GetList(FSSocketListParam param)
        {
            TData<List<FSSocketEntity>> obj = new TData<List<FSSocketEntity>>();
            obj.Data = await fSSocketService.GetList(param);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<FSSocketEntity>>> GetPageList(FSSocketListParam param, Pagination pagination)
        {
            TData<List<FSSocketEntity>> obj = new TData<List<FSSocketEntity>>();
            obj.Data = await fSSocketService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<FSSocketEntity>> GetEntity(long id)
        {
            TData<FSSocketEntity> obj = new TData<FSSocketEntity>();
            obj.Data = await fSSocketService.GetEntity(id);
            if (obj.Data != null)
            {
                obj.Tag = 1;
            }
            return obj;
        }

        public async Task<TData<FSSocketEntity>> GetEntity()
        {
            TData<FSSocketEntity> obj = new TData<FSSocketEntity>();
            obj.Data = await fSSocketService.GetEntity();
            if (obj.Data != null)
            {
                obj.Tag = 1;
            }
            return obj;
        }

        public async Task<TData<DynamicObjectGroupEntity>> GetDynamicObjectGroup(string Name)
        {
            TData<DynamicObjectGroupEntity> obj = new TData<DynamicObjectGroupEntity>();
            obj.Data = await fSSocketService.GetDynamicObjectGroup(Name);
            if (obj.Data != null)
            {
                obj.Tag = 1;
            }
            return obj;
        }

        public async Task<TData<List<DynamicObjectGroupEntity>>> GetDynamicObjectGroups(string Name)
        {
            TData<List<DynamicObjectGroupEntity>> obj = new TData<List<DynamicObjectGroupEntity>>();
            obj.Data = await fSSocketService.GetDynamicObjectGroups(Name);
            if (obj.Data != null)
            {
                obj.Tag = 1;
            }
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(FSSocketEntity entity)
        {
            TData<string> obj = new TData<string>();
            await fSSocketService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await fSSocketService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
