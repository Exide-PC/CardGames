using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CardGames.Services;
using CardGames.Models;
using CardGames.Core;
using System.ComponentModel.DataAnnotations;
using CardGames.Core.Presenters;
using Microsoft.AspNetCore.Http;
using System;
using CardGames.Core.Durak;

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
            if (game == null)
                return NotFound($"There is no game with uid {uid}");

            try
            {
                int id = game.AddPlayer(name);
                AuthData authData = _authService.CreatePlayerToken(uid, id, false);

                return Ok(new
                {
                    playerId = id,
                    authData = authData
                });
            }
            catch (ArgumentException ex)
            {
                return Conflict(ex.Message);
            }
            catch (GameException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}