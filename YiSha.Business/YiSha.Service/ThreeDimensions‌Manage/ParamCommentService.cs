using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YiSha.Data;
using YiSha.Data.Repository;
using YiSha.Entity.ThreeDimensions‌Manage;
using YiSha.Model.Param.ThreeDimensions‌Manage;
using YiSha.Model.Result;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Web.Code;

namespace YiSha.Service.ThreeDimensions‌Manage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2026-06-04 10:20
    /// 描 述：评论数据服务类
    /// </summary>
    public class ParamCommentService : RepositoryFactory
    {
        #region 获取数据
        public async Task<List<ParamCommentEntity>> GetList(ListParamCommentListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<ParamCommentEntity>> GetPageList(ListParamCommentListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<ParamCommentEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<ParamCommentEntity>(id);
        }

        public async Task<CommentList> GetCommentJson(long id)
        {
            //获取贴吧数据
           // param_TiebaEntity param_TiebaEntity = await this.BaseRepository().FindEntity<param_TiebaEntity>(id);
            //获取评论数据
          
            var expression = LinqExtensions.True<ParamCommentEntity>();
            expression = expression.And(t => t.TiebaId == id);

            List<ParamCommentEntity> paramCommentEntities = this.BaseRepository().IQueryable(expression).ToList();
            CommentList commentList = new CommentList();          

            List<Comment> rootCommentList = new List<Comment>();

            foreach (var rootEntity in paramCommentEntities.Where(e => e.Reply_to_user_id == 0))
            {
                // 手动构建根评论，不用扩展方法
                Comment rootComment = new Comment
                {
                    Id = rootEntity.Id,
                    User_id = rootEntity.User_id,
                    Username = rootEntity.Username,
                    UserLevel = 1,
                    Content = rootEntity.Content,
                    BaseCreateTime = rootEntity.BaseCreateTime ?? DateTime.Now,
                    Like_count = (int)(rootEntity.Like_count ?? 0),
                    Status = (int)(rootEntity.Status ?? 0),
                    Parent_id = 0,
                    Reply_to_user_id = null,
                    Replies = new List<Comment>()
                };

                var childEntities = paramCommentEntities.Where(x => x.Parent_id == rootEntity.Id).ToList();
                foreach (var child in childEntities)
                {
                    Comment childComment = new Comment
                    {
                        Id = child.Id,
                        User_id = child.User_id,
                        Username = child.Username,
                        UserLevel = 1,
                        Content = child.Content,
                        BaseCreateTime = child.BaseCreateTime ?? DateTime.Now,
                        Like_count = (int)(child.Like_count ?? 0),
                        Status = (int)(child.Status ?? 0),
                        Parent_id = rootEntity.Id,
                        Reply_to_user_id = child.Reply_to_user_id
                    };

                    rootComment.Replies.Add(childComment);
                }

                rootCommentList.Add(rootComment);
            }
            commentList.Comments = rootCommentList;
            return commentList;
        }
        public async Task<param_TiebaEntity> GetTieBaController(long id)
        {
            //获取贴吧数据
            param_TiebaEntity param_TiebaEntity = await this.BaseRepository().FindEntity<param_TiebaEntity>(id);
            return param_TiebaEntity;

        }



        #endregion

        #region 提交数据
        public async Task SaveForm(ParamCommentEntity entity)
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


        public async Task SubmitReply(Comment entity)
        {

            OperatorInfo operatorInfo = await Operator.Instance.Current();

            ParamCommentEntity paramCommentEntity=new ParamCommentEntity();
            paramCommentEntity.User_id = operatorInfo.UserId;     //当前用户的用户Id
            paramCommentEntity.Username = operatorInfo.UserName;  //当前用户的用户名
            paramCommentEntity.Content = entity.Content;          //内容
            paramCommentEntity.BaseCreateTime = entity.BaseCreateTime;  //创建时间
            paramCommentEntity.Like_count= entity.Like_count;     //点赞数
            paramCommentEntity.Status=entity.Status;               //状态
            paramCommentEntity.Parent_id = entity.Parent_id;        //上级评论ID
            paramCommentEntity.Reply_to_user_id=entity.Reply_to_user_id;  //回复的目标用户ID（用于@功能） 
            paramCommentEntity.TiebaId = entity.Id;

            if (paramCommentEntity.Id.IsNullOrZero())
            {
                await paramCommentEntity.Create();
                await this.BaseRepository().Insert(paramCommentEntity);
            }
            else
            {
                await paramCommentEntity.Modify();
                await this.BaseRepository().Update(paramCommentEntity);
            }
        }


        public async Task DeleteForm(string ids)
        {
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            await this.BaseRepository().Delete<ParamCommentEntity>(idArr);
        }
        #endregion

        #region 私有方法
        private Expression<Func<ParamCommentEntity, bool>> ListFilter(ListParamCommentListParam param)
        {
            var expression = LinqExtensions.True<ParamCommentEntity>();
            if (param != null)
            {
            }
            return expression;
        }

        private Expression<Func<ParamCommentEntity, bool>> ListCommentFilter(long id)
        {
            var expression = LinqExtensions.True<ParamCommentEntity>();
            expression = expression.And(t => t.TiebaId == id);
            return expression;
        }
        #endregion
    }
}
