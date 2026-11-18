using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;

namespace YiSha.Entity.ResultAnalysisManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2024-09-09 09:21
    /// 描 述：DCS对比分析实体类
    /// </summary>
    [Table("result_analysis_dcs")]
    public class AnalysisDcsEntity : BaseExtensionEntity
    {
        /// <summary>
        /// 实际DCS(KNS服务器侧）信号
        /// </summary>
        /// <returns></returns>
        public string ActualDCSSignal { get; set; }
        /// <summary>
        /// 实际值
        /// </summary>
        /// <returns></returns>
        public string ActualValue { get; set; }
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
        /// 信号/孪生机DCS信号
        /// </summary>
        /// <returns></returns>
        public string TwinMachineDCSSignal { get; set; }
    }
}
