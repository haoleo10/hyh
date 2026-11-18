using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;

namespace YiSha.Entity.ThreeDimensions‌Manage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2026-05-19 15:37
    /// 描 述：贴吧实体类
    /// </summary>
    [Table("param_tieba")]
    public class param_TiebaEntity : BaseExtensionEntity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
        public long? Param_alerts_Id { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        /// <returns></returns>
        public string Param_Key { get; set; }
        /// <summary>
        /// 值(3k)
        /// </summary>
        /// <returns></returns>
        public string Param_Value { get; set; }
        /// <summary>
        /// 值(kns)
        /// </summary>
        /// <returns></returns>
        /// 
        public string Param_Value1 { get; set; }
        /// <summary>
        /// 所属系统
        /// </summary>
        /// <returns></returns>
        public string Source_SYS { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        /// <returns></returns>
        public string Content { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        public string Remark { get; set; }
        /// <summary>
        /// 名字
        /// </summary>
        /// <returns></returns>
        public string Name { get; set; }

        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string BaseCreateName { get; set; }


        /// <summary>
        /// 图片存储
        /// </summary>
        public string ImageFile { get; set; }
    }
}
