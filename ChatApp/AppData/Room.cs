namespace ChatApp.AppData
{
    public class Room
    {
        public int Id { get; set; }
        public string Users { get; set; }
        public List<Message> Messages { get; set; }
    }
}
