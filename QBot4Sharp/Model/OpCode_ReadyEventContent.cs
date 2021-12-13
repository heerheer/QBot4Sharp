namespace QBot4Sharp.Model;

public class OpCodeReadyEventContent
{
    public int version { get; set; }
    public string session_id { get; set; }
    public OpCodeReadyEventUser user { get; set; }
    public List<int> shard { get; set; }

    public class OpCodeReadyEventUser
    {
        public string id { get; set; }
        public string username { get; set; }
        public bool bot { get; set; }
    }
}