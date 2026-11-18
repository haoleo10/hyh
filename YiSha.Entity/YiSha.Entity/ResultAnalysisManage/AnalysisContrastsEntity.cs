using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;

namespace YiSha.Entity.ResultAnalysisManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2024-08-19 15:07
    /// 描 述：结果分析(状态对比分析)实体类
    /// </summary>
    [Table("result_analysis_contrasts")]
    public class AnalysisContrastsEntity : BaseExtensionEntity
    {
        /// <summary>
        /// 偏差率
        /// </summary>
        /// <returns></returns>
        public string DeviationValue { get; set; }
        /// <summary>
        /// 主要设备参数/状态
        /// </summary>
        /// <returns></returns>
        public string EquipmentParameters { get; set; }
        /// <summary>
        /// 是否满足
        /// </summary>
        /// <returns></returns>
        public string IsSatisfy { get; set; }
        /// <summary>
        /// FSS模型状态/参数值
        /// </summary>
        /// <returns></returns>
        public string ModelParameter { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        public string Remark { get; set; }
        /// <summary>
        /// 参考机状态/参数值
        /// </summary>
        /// <returns></returns>
        public string UnitParameters { get; set; }
    }
}
