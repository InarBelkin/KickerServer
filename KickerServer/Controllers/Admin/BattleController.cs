using BLL.Dtos.Lobby;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KickerServer.Controllers.Admin;

[Route("api/[controller]")]
[ApiController]
public class BattleController : ControllerBase
{
    private readonly IBattleService _battleService;

    public BattleController(IBattleService battleService)
    {
        _battleService = battleService;
    }

    [HttpGet("{stringId}")]
    public async Task<ActionResult<LobbyItemM>> GetBattle(string stringId)
    {
        var id = Guid.Parse(stringId);
        var res = await _battleService.GetBattle(id);
        return Ok(res);
    }
}