using Microsoft.AspNetCore.SignalR;

namespace KickerServer.Hubs;

public class ChatHub : Hub
{
    public async Task Send(string message)
    {
        await this.Clients.All.SendAsync("send", message);
    }
    
}