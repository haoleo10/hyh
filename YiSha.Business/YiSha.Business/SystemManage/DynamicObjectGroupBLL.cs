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
using YiSha.Model.Param;

namespace YiSha.Business.SystemManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2024-12-17 15:20
    /// 描 述：动态数据组业务类
    /// </summary>
    public class DynamicObjectGroupBLL
    {
        private DynamicObjectGroupService dynamicObjectGroupService = new DynamicObjectGroupService();

        #region 获取数据
        public async Task<TData<List<DynamicObjectGroupEntity>>> GetList(DynamicObjectGroupListParam param)
        {
            TData<List<DynamicObjectGroupEntity>> obj = new TData<List<DynamicObjectGroupEntity>>();
            obj.Data = await dynamicObjectGroupService.GetList(param);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<DynamicObjectGroupEntity>>> GetPageList(DynamicObjectGroupListParam param, Pagination pagination)
        {
            
            TData<List<DynamicObjectGroupEntity>> obj = new TData<List<DynamicObjectGroupEntity>>();
            obj.Data = await dynamicObjectGroupService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<DynamicObjectGroupEntity>> GetEntity(long id)
        {
            TData<DynamicObjectGroupEntity> obj = new TData<DynamicObjectGroupEntity>();
            obj.Data = await dynamicObjectGroupService.GetEntity(id);
            if (obj.Data != null)
            {
                obj.Tag = 1;
            }
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(DynamicObjectGroupEntity entity)
        {
            TData<string> obj = new TData<string>();
            await dynamicObjectGroupService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await dynamicObjectGroupService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> ImportJson(ImportParam param)
        {
            TData obj = new TData();
            await dynamicObjectGroupService.ImportJson(param);
            obj.Tag = 1;
            return obj;
        }
        
        #endregion

        #region 私有方法
        #endregion
    }
}
