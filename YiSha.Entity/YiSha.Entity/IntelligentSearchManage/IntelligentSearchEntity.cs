using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;

namespace YiSha.Entity.IntelligentSearchManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2024-08-20 14:39
    /// 描 述：智能搜索实体类
    /// </summary>
    [Table("intelligent_search")]
    public class IntelligentSearchEntity : BaseExtensionEntity
    {
        /// <summary>
        /// 特征量值
        /// </summary>
        /// <returns></returns>
        public string CharacteristicValue { get; set; }
        /// <summary>
        /// 主要设备列表
        /// </summary>
        /// <returns></returns>
        public string Device { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        public string Remark { get; set; }
        /// <summary>
        /// 特征量名称
        /// </summary>
        /// <returns></returns>
        public string TName { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        /// <returns></returns>
        public string Type { get; set; }
    }
}
