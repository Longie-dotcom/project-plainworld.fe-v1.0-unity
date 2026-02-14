using System;

namespace Assets.Network.DTO
{
    // Request DTO
    public class ChatSendDTO
    {
        public string Content { get; set; } = string.Empty;
    }

    // Response DTO
    public class ChatDTO
    {
        public Guid ID { get; set; }
        public Guid UserID { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; } = string.Empty;
        public int ChatType { get; set; }
        public DateTime SentAt { get; set; }
    }
}
