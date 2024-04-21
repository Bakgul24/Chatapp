using ChatApp.AppData;
using System.Collections.Concurrent;

namespace ChatApp.Services
{
    public class ChatService
    {
        private readonly ConcurrentDictionary<int, User> _onlineUsers = new();
        private readonly IServiceScopeFactory _scopeFactory;

        public ChatService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public void BeOnline(string connectionId, string userName)
        {
            var userId = getUserByUserName(userName).Id;

            _onlineUsers.TryGetValue(userId, out var user);
            if (user == null)
            {
                using var scope = _scopeFactory.CreateScope();
                var dataContext = scope.ServiceProvider.GetRequiredService<ChatAppContext>();

                var dbUser = dataContext.Users.FirstOrDefault(u => u.Id == userId)!;

                dbUser.ConnectionId = connectionId;

                _onlineUsers.TryAdd(userId, user = dbUser);
            }
            else
            {
                user.ConnectionId = connectionId;
            }

            logOnlineUsers();
        }

        public void BeOffline(string userName)
        {
            var userId = getUserByUserName(userName).Id;

            _onlineUsers.TryRemove(userId, out _);

            logOnlineUsers();
        }

        public void SendMessage(int toUserId, int fromUserId)
        {
            _onlineUsers.TryGetValue(toUserId, out var toUser);

            _onlineUsers.TryGetValue(fromUserId, out var fromUser);

            // kaydetme işleri
        }

        public List<User> GetOnlineUsers(string userName)
        {
            if(_onlineUsers.Values.Count > 1)
                return _onlineUsers.Values.Where(u => u.Email != userName).ToList();

            return new List<User>();
        }

        public List<User> GetAllOnlineUsers()
        {
            return _onlineUsers.Values.ToList();
        }

        private User getUserByUserName(string userName)
        {
            using var scope = _scopeFactory.CreateScope();
            var dataContext = scope.ServiceProvider.GetRequiredService<ChatAppContext>();

            return dataContext.Users.FirstOrDefault(u => u.Email == userName)!;
        }

        public void logOnlineUsers()
        {
            Console.WriteLine("---------------------------------------------------------------------");
            foreach (var u in _onlineUsers.Values)
            {
                Console.WriteLine($"Email = {u.Email} ConnectionId={u.ConnectionId}");
            }
            Console.WriteLine("---------------------------------------------------------------------");
        }
    }
}
