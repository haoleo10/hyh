using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;

namespace YiSha.Entity.ThermalPerformanceManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2024-08-19 15:23
    /// 描 述：热效率分析实体类
    /// </summary>
    [Table("efficiency_analysis")]
    public class AfficiencyAnalysisEntity : BaseExtensionEntity
    {
        /// <summary>
        /// 设备名称
        /// </summary>
        public string? Device { get; set; }

        /// <summary>
        /// 设备的别名
        /// </summary>
        public string? Alias { get; set; }

        /// <summary>
        /// 设备参数
        /// </summary>
        public string? Parameter { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        public string? Remark { get; set; }
    }
}
