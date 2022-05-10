using BLL.Dtos.Lobby;
using BLL.Dtos.Lobby.Messages;
using BLL.Dtos.Messages;
using BLL.Interfaces;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace KickerServer.Controllers.Admin;

[Route("api/[controller]")]
[ApiController]
public class LobbyMessagesController : ControllerBase
{
    private readonly ILobbyMessagesService _lobbyMessagesService;
    private readonly ILobbyService _lobbyService;

    public LobbyMessagesController(ILobbyMessagesService lobbyMessagesService, ILobbyService lobbyService)
    {
        _lobbyMessagesService = lobbyMessagesService;
        _lobbyService = lobbyService;
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


    [HttpPost("leave")]
    public async Task<ActionResult<MessageBaseDto>> LeaveBattle(LeaveBattleDto dto)
    {
        var rez = await _lobbyMessagesService.LeaveBattle(dto);
        return Ok(rez);
    }


    [HttpPost("end")]
    public async Task<ActionResult<BattleAnswerDto>> EndBattleAndWriteResults(LobbyItemM lobby)
    {
        var res = await _lobbyService.EndOfBattle(lobby);
        return Ok(res);
    }

    [HttpDelete("earlyend/{id}")]
    public async Task<ActionResult<MessageBaseDto>> EarlyEndBattle(string id)
    {
        var guid = Guid.Parse(id);
        var res = await _lobbyMessagesService.EndBattleEarly(guid);
        return Ok(res);
    }
}