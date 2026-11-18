using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;

namespace YiSha.Entity.ResultAnalysisManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2024-09-09 09:23
    /// 描 述：特征量对比分析实体类
    /// </summary>
    [Table("result_analysis_tzl")]
    public class AnalysisTzlEntity : BaseExtensionEntity
    {
        /// <summary>
        /// 设备名称
        /// </summary>
        /// <returns></returns>
        public string EquipmentName { get; set; }
        /// <summary>
        /// 实际机组数值
        /// </summary>
        /// <returns></returns>
        public string ActualUnitValue { get; set; }
        /// <summary>
        /// 差异
        /// </summary>
        /// <returns></returns>
        public string Difference { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        public string Remark { get; set; }
        /// <summary>
        /// 容忍度
        /// </summary>
        /// <returns></returns>
        public string ToleranceLevel { get; set; }
        /// <summary>
        /// 孪生机组数值
        /// </summary>
        /// <returns></returns>
        public string TwinUnitValue { get; set; }
    }
}
