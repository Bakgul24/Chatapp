using Microsoft.EntityFrameworkCore;

namespace ChatApp.AppData
{
    public class ChatAppContext : DbContext
    {
        public ChatAppContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
