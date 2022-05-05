using BLL.Dtos.Lobby.Messages;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace KickerServer.Controllers.Admin;

[Route("api/[controller]")]
[ApiController]
public class LobbyMessagesController : ControllerBase
{
    private readonly ILobbyMessagesService _lobbyMessagesService;

    public LobbyMessagesController(ILobbyMessagesService lobbyMessagesService)
    {
        _lobbyMessagesService = lobbyMessagesService;
    }

    [HttpPost("inviteOne/{invitedId}")]
    public async Task InviteOne(string invitedId, [FromBody] InviteMessage message)
    {
        var guid = Guid.Parse(invitedId);
        await _lobbyMessagesService.InviteOne(message, guid);
    }

    [HttpPost("inviteAll")]
    public async Task InviteAll(InviteMessage message)
    {
        await _lobbyMessagesService.InviteAll(message);
    }

    [HttpPost("answerToInvite")]
    public async Task AnswerToInvite(InviteAnswer answer)
    {
        await _lobbyMessagesService.AnswerToInvite(answer);
    }
}