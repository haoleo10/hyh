using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace YiSha.Model.Result
{

    /// <summary>
    /// 评论实体类
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// 评论ID
        /// </summary>
        /// 
        [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
        public long? Id { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long? User_id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// 用户等级
        /// </summary>
        public int UserLevel { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime BaseCreateTime { get; set; }

        /// <summary>
        /// 点赞数
        /// </summary>
        public int Like_count { get; set; }

        /// <summary>
        /// 状态（1: 正常）
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 父评论ID（0表示顶层评论）
        /// </summary>
        public long? Parent_id { get; set; }

        /// <summary>
        /// 回复目标用户ID（null表示回复楼主）
        /// </summary>
        public long? Reply_to_user_id { get; set; }

        /// <summary>
        /// 子回复列表
        /// </summary>
        public List<Comment> Replies { get; set; } = new List<Comment>();
    }

    /// <summary>
    /// 评论列表根对象
    /// </summary>
    public class CommentList
    {
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
