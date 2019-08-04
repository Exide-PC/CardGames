using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CardGames.Services;
using CardGames.Models;
using CardGames.Core;
using System.ComponentModel.DataAnnotations;
using CardGames.Core.Presenters;
using Microsoft.AspNetCore.Http;

namespace CardGames.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class LobbyController : ControllerBase
    {
        private readonly GameLobbyService _lobbyService;
        private readonly AuthService _authService;

        public LobbyController(GameLobbyService lobbyService, AuthService authService)
        {
            _lobbyService = lobbyService;
            _authService = authService;
        }

        // lobby/list
        [HttpGet("list")]
        public IEnumerable<LobbyInfo> GetLobbies()
        {
            return _lobbyService.GetLobbies();
        }

        // lobby/create?type=durak
        [HttpPost("create")]
        public void Create([FromQuery] GameType value)
        {
            
        }

        // lobby/join?uid=UID&name=NAME
        [HttpPut("join")]
        public IActionResult JoinGame(
            [FromQuery][Required] string uid, 
            [FromQuery][Required] string name)
        {
            var game = _lobbyService.GetByUid<DurakPresenter>(uid);
            var result = game.AddPlayer(name);
            
            if (!result.Success)
                return BadRequest(result);

            AuthData authData = _authService.CreatePlayerToken(uid, result.Id.Value, false);

            return Ok(new
            {
                authData = authData,
                actionResult = result
            });

        }
    }
}