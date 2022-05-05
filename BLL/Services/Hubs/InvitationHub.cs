using BLL.Dtos.Lobby.Messages;
using BLL.Interfaces;
using BLL.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Services.Hubs;

public class InvitationHub : Hub<ITypedInvitationHub>
{
    protected HttpContext HttpContext { get; }

    protected ILobbyMessagesService LobbyMessagesService =>
        _lobbyMessagesService ??= HttpContext.RequestServices.GetService<ILobbyMessagesService>()!;

    private ILobbyMessagesService? _lobbyMessagesService;


    public InvitationHub(IHttpContextAccessor accessor)
    {
        HttpContext = accessor.HttpContext!;
    }

    public async override Task OnConnectedAsync()
    {
    }

    public async override Task OnDisconnectedAsync(Exception? exception)
    {
    }

    public async Task AnswerToInvite(InviteAnswer answer)
    {
        await LobbyMessagesService.AnswerToInvite(answer);
    }
}