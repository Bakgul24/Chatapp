using ChatApp.Services;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp
{
    public class ChatHub : Hub
    {
        private readonly ChatService _chatService;

        public ChatHub(ChatService chatService)
        {
            _chatService = chatService;
        }

        private string UserConnectionId => Context.ConnectionId;
        private string UserName => Context.GetHttpContext().Request.Query["userName"];

        public override async Task OnConnectedAsync()
        {
            _chatService.BeOnline(UserConnectionId, UserName);
            _chatService.logOnlineUsers();

            await SendOnlineUsersToEveryOne();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _chatService.BeOffline(UserName);

            await SendOnlineUsersToEveryOne();

            await base.OnDisconnectedAsync(exception);
        }

        private async Task SendOnlineUsersToEveryOne()
        {
            var onlineUsers = _chatService.GetOnlineUsers(UserName);
            await Clients.Caller.SendAsync("SetOnlineUsers", onlineUsers);

            var allUsers = _chatService.GetAllOnlineUsers();
            foreach (var user in allUsers)
            {
                var onlineUsersExcept = allUsers.Where(e => e.ConnectionId != user.ConnectionId).ToList();
                await Clients.Client(user.ConnectionId).SendAsync("SetOnlineUsers", onlineUsersExcept);
            }
        }

        public async Task Send(string message, string toMail)
        {
            var toUser = _chatService.GetOnlineUsers(UserName).Where(e=>e.Email==toMail).FirstOrDefault();
            var fromUser = _chatService.GetAllOnlineUsers().Where(e => e.Email == UserName).FirstOrDefault();
            
            //await Clients.All.SendAsync("MessageRecieved", fromUser.FirstName + " " + fromUser.LastName, message);
            await Clients.Client(toUser!.ConnectionId!).SendAsync("MessageRecieved", fromUser.FirstName + " " + fromUser.LastName, message, fromUser.Email, toUser.Email);
            await Clients.Client(fromUser!.ConnectionId!).SendAsync("MessageRecieved", fromUser.FirstName + " " + fromUser.LastName, message, fromUser.Email, toUser.Email);
        }
    }
}
