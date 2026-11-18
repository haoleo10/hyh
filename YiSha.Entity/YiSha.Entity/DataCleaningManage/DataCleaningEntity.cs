using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;

namespace YiSha.Entity.DataCleaningManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2025-05-14 14:23
    /// 描 述：数据清洗实体类
    /// </summary>
    [Table("data_cleaning")]
    public class DataCleaningEntity : BaseExtensionEntity
    {
        /// <summary>
        /// 设备名称
        /// </summary>
        /// <returns></returns>
        public string DeviceName { get; set; }
        /// <summary>
        /// 设备类型
        /// </summary>
        /// <returns></returns>
        public string DeviceType { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        public string Remark { get; set; }
        /// <summary>
        /// 清洗后的值
        /// </summary>
        /// <returns></returns>
        public string SanitizedValue { get; set; }
        /// <summary>
        /// 系统
        /// </summary>
        /// <returns></returns>
        public string System { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime? Time { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        /// <returns></returns>
        public string Value { get; set; }
    }
}
