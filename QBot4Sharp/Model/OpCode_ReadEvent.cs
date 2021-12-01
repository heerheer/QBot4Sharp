using System.Collections.Generic;

namespace QBot4Sharp.Model
{
    public class OpCode_ReadEvent
    {
        public int op { get; set; }
        public int s { get; set; }
        public string t { get; set; }
        public OpCode_ReadEvent_D d { get; set; }
    }
    public class OpCode_ReadEvent_D
    {
        public int version { get; set; }
        public string session_id { get; set; }
        public OpCode_ReadEvent_User user { get; set; }
        public List<int> shard { get; set; }
    }
    public class OpCode_ReadEvent_User
    {
        public string id { get; set; }
        public string username { get; set; }
        public bool bot { get; set; }
    }
}