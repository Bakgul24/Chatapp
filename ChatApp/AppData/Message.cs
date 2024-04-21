namespace ChatApp.AppData
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public User FromUser { get; set; }
        public int FromUserId { get; set; }
    }
}
