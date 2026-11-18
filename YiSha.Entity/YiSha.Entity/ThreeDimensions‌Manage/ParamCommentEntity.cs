using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;

namespace YiSha.Entity.ThreeDimensions‌Manage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2026-06-04 10:20
    /// 描 述：评论数据实体类
    /// </summary>
    [Table("param_comment")]
    public class ParamCommentEntity : BaseExtensionEntity
    {
        /// <summary>
        /// 评论内容
        /// </summary>
        /// <returns></returns>
        public string Content { get; set; }
        /// <summary>
        /// 点踩数（可选）
        /// </summary>
        /// <returns></returns>
        public int? Dislike_count { get; set; }
        /// <summary>
        /// 点赞数
        /// </summary>
        /// <returns></returns>
        public int? Like_count { get; set; }
        /// <summary>
        /// 父评论ID，0表示顶级评论
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
        public long? Parent_id { get; set; }
        /// <summary>
        /// 回复的目标用户ID（用于@功能）
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
        public long? Reply_to_user_id { get; set; }
        /// <summary>
        /// 状态：1-正常，2-已删除（软删除），3-审核中，4-违规屏蔽
        /// </summary>
        /// <returns></returns>
        public int? Status { get; set; }
        /// <summary>
        /// 评论对象ID
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
        public long? Target_id { get; set; }
        /// <summary>
        /// 发表评论的用户ID
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
        public long? User_id { get; set; }

        public string Username { get; set; }

        public long? TiebaId { get; set; }

    }
}
