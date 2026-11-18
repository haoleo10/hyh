using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;

namespace YiSha.Entity.SystemManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2024-10-31 14:56
    /// 描 述：数据通信实体类
    /// </summary>
    [Table("fssocket")]
    public class FSSocketEntity : BaseExtensionEntity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Jsons { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Remark { get; set; }
    }
}
