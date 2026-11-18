using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiSha.Model.IotDBData
{
    public class TimeseriesData
    {
        [JsonProperty("expressions")]
        public List<string> Expressions { get; set; }

        [JsonProperty("column_names")]
        public object ColumnNames { get; set; }

        [JsonProperty("data_types")]
        public List<string> DataTypes { get; set; }

        [JsonProperty("timestamps")]
        public List<long> Timestamps { get; set; }

        [JsonProperty("values")]
        public List<List<List<string>>> Values { get; set; }
    }


    public class IotDBinfo
    {
        public List<string> expressions { get; set; }
        public List<string> column_names { get; set; }
        public List<string> data_types { get; set; }
        public List<long> timestamps { get; set; }
        public List<List<List<string>>> values { get; set; }
    }

    public class IotDBinfoLow
    {
        public List<string> expressions { get; set; }
        public List<string> column_names { get; set; }
        public List<string> data_types { get; set; }
        public List<long> timestamps { get; set; }
        public List<List<string>> values { get; set; }
    }



    public class iotData
    {
        public long? Id { get; set; }
        public string Column_name { get; set; }

        public string Data_type { get; set; }

        public float? DataValue { get; set; }
    }

    public class iotDataState { 
        public string IsIotDB { get; set; }
        public string IsSimulator { get; set; }
        public string IsTwin { get; set; }


    }

    public class SYSNewAtler 
    {
        public string Name { get; set; }
        public int number { get; set; }
        public int addnumber { get; set; }

    }

}
