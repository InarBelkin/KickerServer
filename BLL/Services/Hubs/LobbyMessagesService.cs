using BLL.Dtos.Lobby;
using BLL.Dtos.Lobby.Messages;
using BLL.Interfaces;
using BLL.Services.Hubs;
using BLL.Util;
using DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Services;

public class LobbyMessagesService : ILobbyMessagesService
{
    protected HttpContext HttpContext { get; }
    private readonly IHubContext<InvitationHub, ITypedInvitationHub> _hub;

    protected ILobbyService LobbyService => _lobbyService ??= HttpContext.RequestServices.GetService<ILobbyService>()!;
    private ILobbyService? _lobbyService;


    public LobbyMessagesService(IHubContext<InvitationHub, ITypedInvitationHub> invitationHubContext,
        IHttpContextAccessor accessor)
    {
        HttpContext = accessor.HttpContext!;
        _hub = invitationHubContext;
    }

    public async Task InviteOne(InviteMessage message, Guid InvitedId)
    {
        await _hub.Clients.User(InvitedId.ToString()).Invite(message);
    }

    public async Task InviteAll(InviteMessage message)
    {
        await _hub.Clients.All.Invite(message);
    }

    public async Task AnswerToInvite(InviteAnswer answer)
    {
        await LobbyService.ApplyUserInviteAnswer(answer);
        await _hub.Clients.User(answer.InitiatorId.ToString()).AnswerInvite(answer);
    }

    public async Task YourLobbyWasUpdated(LobbyItemM lobbyItem)
    {
        foreach (var user in lobbyItem.SideA.Concat(lobbyItem.SideB).Concat(new[] {lobbyItem.Initiator})
                     .Where(u => u.Id != null))
        {
            await _hub.Clients.User(user.Id.ToString()!).YourLobbyChanged(lobbyItem);
        }
    }
}