using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CardGames.Services;
using static CardGames.Core.Durak.Card;
using CardGames.Core.Durak;
using CardGames.Models;
using CardGames.Core.Presenters;

namespace CardGames.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DurakController : ControllerBase
    {
        private readonly GameLobbyService _lobbyService;
        
        public DurakController(GameLobbyService lobbyService)
        {
            _lobbyService = lobbyService;
        }

        [HttpGet("state")]
        public DurakPresenter.PlayerState GetState()
        {
            (DurakPresenter game, int playerId) = GetPlayerData();

            return game.GetState(playerId);
        }
        

        // durak/turn
        [HttpPost("turn")]
        public IActionResult MakeTurn([FromBody] Card card)
        {
            (DurakPresenter game, int playerId) = GetPlayerData();

            try
            {
                game.Turn(playerId, card);
                return Ok();
            }
            catch (GameException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // durak/skip
        [HttpPost("skip")]
        public IActionResult SkipTurn()
        {
            (DurakPresenter game, int playerId) = GetPlayerData();

            try
            {
                game.SkipTurn(playerId);
                return Ok();
            }
            catch (GameException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        (DurakPresenter game, int playerId) GetPlayerData()
        {
            int playerId = int.Parse(this.GetClaimValue(GameClaim.PlayerId));
            string gameUid = this.GetClaimValue(GameClaim.GameUid);

            var game = _lobbyService.GetByUid<DurakPresenter>(gameUid);

            return (game, playerId);
        }

        string GetClaimValue(GameClaim claim)
        {
            return this.User.Claims.First(c => c.Type == claim.ToString()).Value;
        }
    }
}