using System.Text.Json;

namespace QBot4Sharp.Model.Messages
{
    public class QBotMessageSend
    {
        public string content { get; set; }
        public string msg_id { get; set; }
        //public string image { get; set; } = "";

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}