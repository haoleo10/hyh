using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using YiSha.Admin.Web.Controllers;
using YiSha.Business.ThreeDimensions‌Manage;
using YiSha.Entity;
using YiSha.Entity.ThreeDimensions‌Manage;
using YiSha.Model;
using YiSha.Model.Param.ThreeDimensions‌Manage;
using YiSha.Model.Result;
using YiSha.Util;
using YiSha.Util.Model;

namespace YiSha.Admin.Web.Areas.ThreeDimensions‌Manage.Controllers
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2026-06-04 10:20
    /// 描 述：评论数据控制器类
    /// </summary>
    [Area("ThreeDimensions‌Manage")]
    public class ParamCommentController :  BaseController
    {
        private ParamCommentBLL paramCommentBLL = new ParamCommentBLL();

        #region 视图功能
        [AuthorizeFilter("threedimensions‌:paramcomment:view")]
        public ActionResult ParamCommentIndex()
        {
            return View();
        }

        public ActionResult ParamCommentForm()
        {
            return View();
        }

        public ActionResult ToParamCommentIndex()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("threedimensions‌:paramcomment:search")]
        public async Task<ActionResult> GetListJson(ListParamCommentListParam param)
        {
            TData<List<ParamCommentEntity>> obj = await paramCommentBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("threedimensions‌:paramcomment:search")]
        public async Task<ActionResult> GetPageListJson(ListParamCommentListParam param, Pagination pagination)
        {
            TData<List<ParamCommentEntity>> obj = await paramCommentBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<ParamCommentEntity> obj = await paramCommentBLL.GetEntity(id);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetCommentJson(long id)
        {
            CommentList obj = await paramCommentBLL.GetCommentJson(id);        
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetTieBaController(long id)
        {
            param_TiebaEntity obj = await paramCommentBLL.GetTieBaController(id);


            return Json(obj);
        }





        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("threedimensions‌:paramcomment:add,threedimensions‌:paramcomment:edit")]
        public async Task<ActionResult> SaveFormJson(ParamCommentEntity entity)
        {
            TData<string> obj = await paramCommentBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("threedimensions‌:paramcomment:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await paramCommentBLL.DeleteForm(ids);
            return Json(obj);
        }

        [HttpPost]
        public async Task<ActionResult> SubmitReply(Comment entity)
        {
            TData<string> obj = await paramCommentBLL.SubmitReply(entity);
            return Json(obj);

        }


        #endregion
    }
}
