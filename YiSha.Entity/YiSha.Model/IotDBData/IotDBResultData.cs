using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiSha.Model.IotDBData
{
    public class IotDBResultData
    {
        public long? Id { get; set; }
        public string Column_name { get; set; }

        public string Data_type { get; set; }

        public string DataValue { get; set; }
    }

    public class IotDBResultDataCleaning
    {
        public long? Id { get; set; }

        public string Column_name { get; set; }

        public string Data_type { get; set; }

        public string DataValue { get; set; }


    }
}
