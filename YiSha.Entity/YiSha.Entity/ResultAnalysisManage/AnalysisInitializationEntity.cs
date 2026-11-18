using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;

namespace YiSha.Entity.ResultAnalysisManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2024-08-19 15:00
    /// 描 述：结果分析(状态初始化)实体类
    /// </summary>
    [Table("result_analysis_initialization")]
    public class AnalysisInitializationEntity : BaseExtensionEntity
    {
        /// <summary>
        /// 设备
        /// </summary>
        /// <returns></returns>
        public string Device { get; set; }
        /// <summary>
        /// 是否一致
        /// </summary>
        /// <returns></returns>
        public string IsConsistent { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        public string Remark { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        /// <returns></returns>
        public string State { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        /// <returns></returns>
        public string Type { get; set; }
    }
}
