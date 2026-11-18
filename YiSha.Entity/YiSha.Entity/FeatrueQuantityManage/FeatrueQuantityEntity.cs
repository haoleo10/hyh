using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;

namespace YiSha.Entity.FeatrueQuantityManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2024-08-20 11:09
    /// 描 述：特征量计算实体类
    /// </summary>
    [Table("featrue_quantity")]
    public class FeatrueQuantityEntity : BaseExtensionEntity
    {
        /// <summary>
        /// 特征量计算值
        /// </summary>
        /// <returns></returns>
        public string CalculatedValue { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        /// <returns></returns>
        public string Device { get; set; }
        /// <summary>
        /// 特征量参考值
        /// </summary>
        /// <returns></returns>
        public string ReferenceValue { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        public string Remark { get; set; }
        /// <summary>
        /// 设备类型

        /// </summary>
        /// <returns></returns>
        public string Type { get; set; }


        /// <summary>
        /// 系统分类
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// 下标
        /// </summary>
        public int? Subscript { get; set; }


        /// <summary>
        /// 点名
        /// </summary>
        public string ProtName { get; set; }

        /// <summary>
        /// 点名iotDB
        /// </summary>
        public string ProtNameIotDB { get; set; }
    }
}
