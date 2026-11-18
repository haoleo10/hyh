using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;

namespace YiSha.Entity.ThreeDimensions‌Manage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2026-05-07 15:17
    /// 描 述：二层界面实体类
    /// </summary>
    [Table("three_dimensions‌")]
    public class TweFloorEntity : BaseExtensionEntity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string KNS { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string NamePort { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string RealTime { get; set; }
    }
}
