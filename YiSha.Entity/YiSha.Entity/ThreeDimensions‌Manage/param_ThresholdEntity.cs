using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;

namespace YiSha.Entity.ThreeDimensions‌Manage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2026-09-21 15:03
    /// 描 述：报警阈值实体类
    /// </summary>
    [Table("param_threshold")]
    public class param_ThresholdEntity : BaseExtensionEntity
    {
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        public string Remark { get; set; }
        /// <summary>
        /// 点名
        /// </summary>
        /// <returns></returns>
        public string Rollcall { get; set; }
        /// <summary>
        /// 百分比
        /// </summary>
        /// <returns></returns>
        public int? Value { get; set; }
    }
}
