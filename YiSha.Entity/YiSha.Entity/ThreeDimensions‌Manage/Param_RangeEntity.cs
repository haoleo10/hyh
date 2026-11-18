using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;

namespace YiSha.Entity.ThreeDimensions‌Manage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2026-08-06 15:35
    /// 描 述：量程实体类
    /// </summary>
    [Table("param_range")]
    public class Param_RangeEntity : BaseExtensionEntity
    {
        /// <summary>
        /// 点名
        /// </summary>
        /// <returns></returns>
        public string DotName { get; set; }
        /// <summary>
        /// 量程
        /// </summary>
        /// <returns></returns>
        public float? Range { get; set; }

        /// <summary>
        /// 最小量程
        /// </summary>
        /// <returns></returns>
        public float? RangeMin { get; set; }

        
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        public string Remark { get; set; }
    }
}
