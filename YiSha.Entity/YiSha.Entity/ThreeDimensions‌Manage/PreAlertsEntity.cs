using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;

namespace YiSha.Entity.ThreeDimensions‌Manage
{
    /// <summary>
    /// 描 述：预报警列表实体类
    /// </summary>
    [Table("param_pre_alerts")]
    public class PreAlertsEntity : BaseExtensionEntity
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public string Name { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        /// <returns></returns>
        public string Param_Key { get; set; }
        /// <summary>
        /// 值(3K)
        /// </summary>
        /// <returns></returns>
        public string Param_Value { get; set; }
        /// <summary>
        /// 值(KNS)
        /// </summary>
        /// <returns></returns>
        public string Param_Value1 { get; set; }

        /// <summary>
        /// 所属系统
        /// </summary>
        /// <returns></returns>
        public string Source_SYS { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        public string Remark { get; set; }

        public int? AlarmLayer { get; set; }

        /// <summary>
        /// 状态（0:未确认；1:已确认报警；2:已萃取）
        /// </summary>
        public int Status { get; set; }
    }
}
