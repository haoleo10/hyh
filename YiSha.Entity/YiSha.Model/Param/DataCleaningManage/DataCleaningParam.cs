using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using YiSha.Util;

namespace YiSha.Model.Param.DataCleaningManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2025-05-14 14:23
    /// 描 述：数据清洗实体查询类
    /// </summary>
    public class DataCleaningListParam
    {
        /// <summary>
        /// 系统
        /// </summary>
        public string System { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        public string DeviceType { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// 起始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

    }
}
