using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiSha.Model.SoketData
{
    public class SocketDataInfo
    {
        public Header header { get; set; }
        public Body body { get; set; }
    }

    public class SocketDataPut
    {
        public Header header { get; set; }
        public BodyPut bodyPut { get; set; }

        public object TOJSON()
        {
            throw new NotImplementedException();
        }
    }

    // 对应header部分的实体类
    public class Header
    {       
        public long packet_mask { get; set; }
        public long trans_id { get; set; }
        public int has_checksum { get; set; }
        public long checksum_code { get; set; }
        public long packet_len { get; set; }
    }

    // 对应body部分的实体类
    public class Body
    {
        public long DataType { get; set; }
        public long TotalSize { get; set; }
        public long PacketSerNo { get; set; }
        public long TimeStamp { get; set; }
        public long FSS_Time { get; set; }
        public long SimState { get; set; }
        public long CurrentIC { get; set; }
        public long CommandID { get; set; }
        public long ResponseID { get; set; }
        public long ArrSize { get; set; }
        public long CmdSize { get; set; }      
        public List<double> ArrData { get; set; }
        public string CmdData { get; set; }
    }
    public class BodyPut
    {
        public long DataType { get; set; }
        public long TotalSize { get; set; }
        public long PacketSerNo { get; set; }
        public long TimeStamp { get; set; }
        public long FSS_Time { get; set; }
        public long SimState { get; set; }
        public long CurrentIC { get; set; }
        public long CommandID { get; set; }    
        public long ResponseID { get; set; }
        public string PointName { get; set; }
        public double PointValue { get; set; }
    }

}
