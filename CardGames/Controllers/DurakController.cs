using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CardGames.Services;
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
        public DurakPresenter.GameStateHolder GetState()
        {
            (DurakPresenter game, int playerId) = GetPlayerData();

            return game.GetState(playerId);
        }
        

        // durak/turn
        [HttpPost("turn")]
        public IActionResult MakeTurn([FromBody] Card card, [FromBody] Card target = null)
        {
            (DurakPresenter game, int playerId) = GetPlayerData();

            try
            {
                game.Turn(playerId, card, target);
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

        [HttpPost("status")]
        public void SetPlayerStatus([FromQuery]bool isReady)
        {
            (DurakPresenter game, int playerId) = GetPlayerData();
            game.SetPlayerReady(playerId, isReady);
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