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
    /// 日 期：2026-05-19 10:58
    /// 描 述：报警列表业务类
    /// </summary>
    public class ParamAlertsBLL
    {
        private ParamAlertsService paramAlertsService = new ParamAlertsService();

        #region 获取数据
        public async Task<TData<List<ParamAlertsEntity>>> GetList(ListParamAlertsListParam param, string AlarmLayer,string SYS)
        {
            TData<List<ParamAlertsEntity>> obj = new TData<List<ParamAlertsEntity>>();
            obj.Data = await paramAlertsService.GetList(param, AlarmLayer, SYS);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<ParamAlertsEntity>>> GetPageList(ListParamAlertsListParam param, Pagination pagination,string AlarmLayer,string SYS)
        {
            TData<List<ParamAlertsEntity>> obj = new TData<List<ParamAlertsEntity>>();
            obj.Data = await paramAlertsService.GetList(param, AlarmLayer,SYS);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<ParamAlertsEntity>> GetEntity(long id)
        {
            TData<ParamAlertsEntity> obj = new TData<ParamAlertsEntity>();
            obj.Data = await paramAlertsService.GetEntity(id);
            if (obj.Data != null)
            {
                obj.Tag = 1;
            }
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(ParamAlertsEntity entity)
        {
            TData<string> obj = new TData<string>();
            await paramAlertsService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await paramAlertsService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
