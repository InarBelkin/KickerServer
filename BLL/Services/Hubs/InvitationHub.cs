using BLL.Dtos.Lobby.Messages;
using BLL.Interfaces;
using BLL.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Services.Hubs;

public class InvitationHub : Hub<ITypedInvitationHub>
{
    private readonly ILobbyUtilService _utilService;
    protected HttpContext HttpContext { get; }

    protected ILobbyMessagesService LobbyMessagesService =>
        _lobbyMessagesService ??= HttpContext.RequestServices.GetService<ILobbyMessagesService>()!;

    private ILobbyMessagesService? _lobbyMessagesService;


    public InvitationHub(IHttpContextAccessor accessor, ILobbyUtilService utilService)
    {
        _utilService = utilService;
        HttpContext = accessor.HttpContext!;
    }

    public override async Task OnConnectedAsync()
    {
        var strId = Context.UserIdentifier;
        var id = Guid.Parse(strId!);
        await _utilService.AddUserOnline(id);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var strId = Context.UserIdentifier;
        var id = Guid.Parse(strId!);
        await _utilService.DeleteUserOnline(id);
    }

    public async Task AnswerToInvite(InviteAnswer answer)
    {
        await LobbyMessagesService.AnswerToInvite(answer);
    }
    
    
}