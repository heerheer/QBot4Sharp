using System.Collections.Generic;

namespace QBot4Sharp.Model
{
    public class OpCode_ReadyEventContent
    {
        public int version { get; set; }
        public string session_id { get; set; }
        public OpCode_ReadyEvent_User user { get; set; }
        public List<int> shard { get; set; }
    }
    public class OpCode_ReadyEvent_User
    {
        public string id { get; set; }
        public string username { get; set; }
        public bool bot { get; set; }
    }
}