using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CardGames.Services;
using CardGames.Models;
using System.ComponentModel.DataAnnotations;
using CardGames.Core.Presenters;
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
        public IEnumerable<LobbyInfo> GetList()
        {
            #if DEBUG
            // TODO: Remove, just for debug
            var token1 = _authService.CreatePlayerToken("test", 0, true);
            var token2 = _authService.CreatePlayerToken("test", 1, false);
            var token3 = _authService.CreatePlayerToken("test", 2, false);
            #endif

            return _lobbyService.GetLobbies();
        }

        // lobby/create?type=durak
        [HttpPost("create")]
        public IActionResult Create([FromQuery][Required]string name)
        {
            string uid = _lobbyService.CreateLobby();

            var game = _lobbyService.GetByUid<DurakPresenter>(uid);
            int playerId = game.AddPlayer(name);
            
            var authData = _authService.CreatePlayerToken(uid, playerId, true);
            return Ok(new
            {
                gameUid = uid,
                playerId = playerId,
                auth = authData
            });
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
                    auth = authData
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