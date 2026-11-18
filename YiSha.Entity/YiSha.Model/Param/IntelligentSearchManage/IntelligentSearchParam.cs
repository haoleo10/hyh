using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using YiSha.Util;

namespace YiSha.Model.Param.IntelligentSearchManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2024-08-20 14:39
    /// 描 述：智能搜索实体查询类
    /// </summary>
    public class IntelligentSearchListParam
    {
        public string KeyWorkName { get; set; }
        public string DoneWorkName { get; set; }

        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public int? ValueMin { get; set; }

        public int? ValueMax { get; set; }

        public int? State { get; set; }

        public string doneWorkName123 { get; set; }

    }
}
