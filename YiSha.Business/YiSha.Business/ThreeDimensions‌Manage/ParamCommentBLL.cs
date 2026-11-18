using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YiSha.Entity.ThreeDimensions‌Manage;
using YiSha.Model.Param.ThreeDimensions‌Manage;
using YiSha.Model.Result;
using YiSha.Service.ThreeDimensions‌Manage;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;

namespace YiSha.Business.ThreeDimensions‌Manage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2026-06-04 10:20
    /// 描 述：评论数据业务类
    /// </summary>
    public class ParamCommentBLL
    {
        private ParamCommentService paramCommentService = new ParamCommentService();

        #region 获取数据
        public async Task<TData<List<ParamCommentEntity>>> GetList(ListParamCommentListParam param)
        {
            TData<List<ParamCommentEntity>> obj = new TData<List<ParamCommentEntity>>();
            obj.Data = await paramCommentService.GetList(param);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<ParamCommentEntity>>> GetPageList(ListParamCommentListParam param, Pagination pagination)
        {
            TData<List<ParamCommentEntity>> obj = new TData<List<ParamCommentEntity>>();
            obj.Data = await paramCommentService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<ParamCommentEntity>> GetEntity(long id)
        {
            TData<ParamCommentEntity> obj = new TData<ParamCommentEntity>();
            obj.Data = await paramCommentService.GetEntity(id);
            if (obj.Data != null)
            {
                obj.Tag = 1;
            }
            return obj;
        }
        public async Task<CommentList> GetCommentJson(long id)
        {
            
           return await paramCommentService.GetCommentJson(id);
        }

        public async Task<param_TiebaEntity> GetTieBaController(long id)
        {

            return await paramCommentService.GetTieBaController(id);
        }

        


        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(ParamCommentEntity entity)
        {
            TData<string> obj = new TData<string>();
            await paramCommentService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }


        public async Task<TData<string>> SubmitReply(Comment entity)
        {
            TData<string> obj = new TData<string>();
            await paramCommentService.SubmitReply(entity);
           //obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await paramCommentService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
