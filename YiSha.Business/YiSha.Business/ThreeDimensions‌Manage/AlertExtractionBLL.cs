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
    /// 日 期：2026-06-17 14:39
    /// 描 述：萃取业务类
    /// </summary>
    public class AlertExtractionBLL
    {
        private AlertExtractionService alertExtractionService = new AlertExtractionService();

        #region 获取数据
        public async Task<TData<List<AlertExtractionEntity>>> GetList(AlertExtractionListParam param)
        {
            TData<List<AlertExtractionEntity>> obj = new TData<List<AlertExtractionEntity>>();
            obj.Data = await alertExtractionService.GetList(param);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<AlertExtractionEntity>>> GetPageList(AlertExtractionListParam param, Pagination pagination)
        {
            TData<List<AlertExtractionEntity>> obj = new TData<List<AlertExtractionEntity>>();
            obj.Data = await alertExtractionService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<AlertExtractionEntity>>> CompleteGetPageListJson(AlertExtractionListParam param, Pagination pagination)
        {
            TData<List<AlertExtractionEntity>> obj = new TData<List<AlertExtractionEntity>>();
            obj.Data = await alertExtractionService.CompleteGetPageListJson(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }
        

        public async Task<TData<AlertExtractionEntity>> GetEntity(long id)
        {
            TData<AlertExtractionEntity> obj = new TData<AlertExtractionEntity>();
            obj.Data = await alertExtractionService.GetEntity(id);
            if (obj.Data != null)
            {
                obj.Tag = 1;
            }
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(AlertExtractionEntity entity)
        {
            TData<string> obj = new TData<string>();
            await alertExtractionService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await alertExtractionService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> Extraction(string id)
        {
            TData obj = new TData();
            await alertExtractionService.Extraction(id);
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> CompleteExtraction(string id)
        {
            TData obj = new TData();
            await alertExtractionService.CompleteExtraction(id);
            obj.Tag = 1;
            return obj;
        }
        

        #endregion

        #region 私有方法
        #endregion
    }
}
