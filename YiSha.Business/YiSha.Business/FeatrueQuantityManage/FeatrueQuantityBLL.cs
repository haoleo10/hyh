using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Entity.FeatrueQuantityManage;
using YiSha.Model.Param.FeatrueQuantityManage;
using YiSha.Service.FeatrueQuantityManage;

namespace YiSha.Business.FeatrueQuantityManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2024-08-20 11:09
    /// 描 述：特征量计算业务类
    /// </summary>
    public class FeatrueQuantityBLL
    {
        private FeatrueQuantityService featrueQuantityService = new FeatrueQuantityService();

        #region 获取数据
        public async Task<TData<List<FeatrueQuantityEntity>>> GetList(FeatrueQuantityListParam param)
        {
            TData<List<FeatrueQuantityEntity>> obj = new TData<List<FeatrueQuantityEntity>>();
            obj.Data = await featrueQuantityService.GetList(param);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<FeatrueQuantityEntity>>> GetPageList(FeatrueQuantityListParam param, Pagination pagination)
        {
            TData<List<FeatrueQuantityEntity>> obj = new TData<List<FeatrueQuantityEntity>>();
            obj.Data = await featrueQuantityService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<FeatrueQuantityEntity>>> GetPageListChart(FeatrueQuantityListParam param, Pagination pagination)
        {
            TData<List<FeatrueQuantityEntity>> obj = new TData<List<FeatrueQuantityEntity>>();
            obj.Data = await featrueQuantityService.GetPageListChart(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        

        public async Task<TData<FeatrueQuantityEntity>> GetEntity(long id)
        {
            TData<FeatrueQuantityEntity> obj = new TData<FeatrueQuantityEntity>();
            obj.Data = await featrueQuantityService.GetEntity(id);
            if (obj.Data != null)
            {
                obj.Tag = 1;
            }
            return obj;
        }

        public async Task<TData<object>> GetState()
        {
            TData<object> obj = new TData<object>();
            obj.Data =  featrueQuantityService.GetState();           
            obj.Tag = 1;
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(FeatrueQuantityEntity entity)
        {
            TData<string> obj = new TData<string>();
            await featrueQuantityService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await featrueQuantityService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }

        public async Task StartCompute()
        {
            await featrueQuantityService.StartCompute();           
        }

        public async Task StopCompute()
        {
            await featrueQuantityService.StopCompute();
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
