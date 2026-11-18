using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;

namespace YiSha.Entity.SystemManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2024-12-17 15:20
    /// 描 述：动态数据组实体类
    /// </summary>
    [Table("dynamic_object_group")]
    public class DynamicObjectGroupEntity : BaseExtensionEntity
    {
        /// <summary>
        /// 点名
        /// </summary>
        /// <returns></returns>
        public string ProtName { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        /// <returns></returns>
        public string ProtVelue { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        /// <returns></returns>
        public string DataType { get; set; }
        /// <summary>
        /// 下标
        /// </summary>
        /// <returns></returns>
        public int? Subscript { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        public string Remake { get; set; }
    }
}
