using BLL.Dtos.Lobby;
using BLL.Dtos.Messages;
using BLL.Interfaces;
using BLL.Services.Hubs;
using BLL.Util;
using DAL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace KickerServer.Controllers.Admin;

[Route("api/[controller]")]
[ApiController]
public class LobbyController : ControllerBase
{
    private readonly IHubContext<InvitationHub, ITypedInvitationHub> _invitationHubContext;
    private readonly ILobbyService _lobby;

    public LobbyController(IHubContext<InvitationHub, ITypedInvitationHub> invitationHubContext, ILobbyService lobby)
    {
        _invitationHubContext = invitationHubContext;
        _lobby = lobby;
    }

    [Authorize]
    [HttpGet("lobby")]
    public async Task<ActionResult<LobbyItemM?>> GetLobby()
    {
        return Ok(await _lobby.GetMyLobby());
    }

    [HttpGet]
    public async Task<ActionResult<List<LobbyItemM>>> GetLobbys()
    {
        var ret = await _lobby.GetLobbyList();
        return Ok(ret);
    }

    [HttpPost]
    public async Task<ActionResult<MessageBaseDto>> CreateLobby(LobbyItemM dto)
    {
        var answer = await _lobby.StartLobby(dto);
        return Ok(answer);
    }

    [HttpPut]
    public async Task<ActionResult<MessageBaseDto>> UpdateLobby(LobbyItemM dto)
    {
        return Ok(await _lobby.UpdateLobby(dto));
    }


    [HttpDelete("{userId}")]
    public async Task<ActionResult<MessageBaseDto>> DeleteLobby(string userId)
    {
        var guid = Guid.Parse(userId);
        return Ok(await _lobby.DeleteLobby(guid));
    }
}