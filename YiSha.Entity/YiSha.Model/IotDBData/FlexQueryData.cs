using Newtonsoft.Json;
using System.Collections.Generic;

namespace YiSha.Model.IotDBData
{
    /// <summary>
    /// 三维灵活查询请求体（一组 = 点名 + 来源 + 时间段，可传多组）
    /// </summary>
    public class FlexQueryRequest
    {
        [JsonProperty("queries")]
        public List<FlexQueryGroup> Queries { get; set; }
    }

    /// <summary>
    /// 单组查询条件
    /// </summary>
    public class FlexQueryGroup
    {
        /// <summary>
        /// 点名，多个用逗号分隔
        /// </summary>
        [JsonProperty("pointNames")]
        public string PointNames { get; set; }

        /// <summary>
        /// 来源（0:KNS；1:FSS）
        /// </summary>
        [JsonProperty("source")]
        public string Source { get; set; }

        /// <summary>
        /// 开始时间（2026-11-01 00:00:00）
        /// </summary>
        [JsonProperty("startTime")]
        public string StartTime { get; set; }

        /// <summary>
        /// 结束时间（2026-11-20 00:00:00）
        /// </summary>
        [JsonProperty("endTime")]
        public string EndTime { get; set; }
    }

    /// <summary>
    /// 单组查询结果，与请求组顺序一一对应；查询失败或无数据时 Data 为 null
    /// </summary>
    public class FlexQueryResult
    {
        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("pointNames")]
        public string PointNames { get; set; }

        [JsonProperty("data")]
        public IotDBinfoLow Data { get; set; }
    }
}
