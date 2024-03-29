﻿using BLL.Dtos.Stats;
using BLL.Interfaces;
using BLL.Models.Stats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KickerServer.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private readonly IStatsService _statsService;

        public StatsController(IStatsService statsService)
        {
            _statsService = statsService;
        }

        [HttpGet]
        public async Task<ActionResult<LeaderboardWrapper>> GetLeaders()
        {
            return Ok(await _statsService.GetLeadersList());
        }

        [HttpGet("userDetails/{stringId}")]
        public async Task<ActionResult<UserDetailsDto>> GetUserDetailsById(string stringId)
        {
            var guid = Guid.Parse(stringId);
            var userDto = await _statsService.GetUserDetails(guid);


            return userDto == null ? NotFound() : Ok(userDto);
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<MyPageDto>> GetMyPage()
        {
            var pageDto = await _statsService.GetMyPage();
            return Ok(pageDto);
        }
    }
}