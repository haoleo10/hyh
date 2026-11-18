using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Entity.ThreeDimensions‌Manage;
using YiSha.Model.Param.ThreeDimensions‌Manage;
using YiSha.Service.ThreeDimensions‌Manage;

namespace YiSha.Business.ThreeDimensions‌Manage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2026-09-21 15:03
    /// 描 述：报警阈值业务类
    /// </summary>
    public class param_ThresholdBLL
    {
        private param_ThresholdService param_ThresholdService = new param_ThresholdService();

        #region 获取数据
        public async Task<TData<List<param_ThresholdEntity>>> GetList(param_ThresholdListParam param)
        {
            TData<List<param_ThresholdEntity>> obj = new TData<List<param_ThresholdEntity>>();
            obj.Data = await param_ThresholdService.GetList(param);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<param_ThresholdEntity>>> GetPageList(param_ThresholdListParam param, Pagination pagination)
        {
            TData<List<param_ThresholdEntity>> obj = new TData<List<param_ThresholdEntity>>();
            obj.Data = await param_ThresholdService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<param_ThresholdEntity>> GetEntity(long id)
        {
            TData<param_ThresholdEntity> obj = new TData<param_ThresholdEntity>();
            obj.Data = await param_ThresholdService.GetEntity(id);
            if (obj.Data != null)
            {
                obj.Tag = 1;
            }
            return obj;
        }


        public async Task<TData<param_ThresholdEntity>> GetNameEntity(string Name)
        {
            TData<param_ThresholdEntity> obj = new TData<param_ThresholdEntity>();
            obj.Data = await param_ThresholdService.GetNameEntity(Name);
            if (obj.Data != null)
            {
                obj.Tag = 1;
            }
            return obj;
        }

        public async Task<TData<List<param_ThresholdEntity>>> GetListNameEntity(string Name)
        {

            TData<List<param_ThresholdEntity>> obj = new TData<List<param_ThresholdEntity>>();
            obj.Data = await param_ThresholdService.GetListNameEntity(Name);         
            obj.Tag = 1;
            return obj;         
        }


        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(param_ThresholdEntity entity)
        {
            TData<string> obj = new TData<string>();
            await param_ThresholdService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<string>> SaveNameForm(string Name)
        {
            TData<string> obj = new TData<string>();
            await param_ThresholdService.SaveNameForm(Name);           
            obj.Tag = 1;
            return obj;
        }



        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await param_ThresholdService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
