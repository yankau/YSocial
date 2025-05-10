using Microsoft.AspNetCore.SignalR;

namespace YSocial.Hubs;

public class ChatHub : Hub
{
    public async Task SendMessage(int senderId, int receiverId, string content)
    {
        await Clients.User(receiverId.ToString()).SendAsync("ReceiveMessage", senderId, content);
        await Clients.User(senderId.ToString()).SendAsync("ReceiveMessage", senderId, content);
    }
}