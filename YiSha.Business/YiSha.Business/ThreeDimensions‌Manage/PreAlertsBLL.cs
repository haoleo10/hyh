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
    /// 描 述：预报警列表业务类
    /// </summary>
    public class PreAlertsBLL
    {
        private PreAlertsService preAlertsService = new PreAlertsService();
        private ParamAlertsService paramAlertsService = new ParamAlertsService();

        #region 获取数据
        public async Task<TData<List<PreAlertsEntity>>> GetList(ListParamAlertsListParam param, string AlarmLayer, string SYS)
        {
            TData<List<PreAlertsEntity>> obj = new TData<List<PreAlertsEntity>>();
            obj.Data = await preAlertsService.GetList(param, AlarmLayer, SYS);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<PreAlertsEntity>>> GetPageList(ListParamAlertsListParam param, Pagination pagination, string AlarmLayer, string SYS)
        {
            TData<List<PreAlertsEntity>> obj = new TData<List<PreAlertsEntity>>();
            obj.Data = await preAlertsService.GetPageList(param, pagination, AlarmLayer, SYS);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<PreAlertsEntity>> GetEntity(long id)
        {
            TData<PreAlertsEntity> obj = new TData<PreAlertsEntity>();
            obj.Data = await preAlertsService.GetEntity(id);
            if (obj.Data != null)
            {
                obj.Tag = 1;
            }
            return obj;
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 确认报警：把预报警记录复制到报警列表（param_alerts），并标记预报警为已确认
        /// </summary>
        public async Task<TData> ConfirmAlarm(long id)
        {
            TData obj = new TData();
            PreAlertsEntity preAlert = await preAlertsService.GetEntity(id);
            if (preAlert == null)
            {
                obj.Message = "预报警记录不存在";
                return obj;
            }

            ParamAlertsEntity alarm = new ParamAlertsEntity
            {
                Name = preAlert.Name,
                Param_Key = preAlert.Param_Key,
                Param_Value = preAlert.Param_Value,
                Param_Value1 = preAlert.Param_Value1,
                Source_SYS = preAlert.Source_SYS,
                Remark = preAlert.Remark,
                AlarmLayer = preAlert.AlarmLayer,
                // 保留原始报警产生时间
                BaseCreateTime = preAlert.BaseCreateTime
            };
            await paramAlertsService.SaveForm(alarm);

            await preAlertsService.UpdateStatus(id, 1);
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<string>> SaveForm(PreAlertsEntity entity)
        {
            TData<string> obj = new TData<string>();
            await preAlertsService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await preAlertsService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }
        #endregion
    }
}
