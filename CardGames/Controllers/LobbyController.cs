using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CardGames.Services;

namespace CardGames.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LobbyController : ControllerBase
    {
        private readonly GameLobbyService _lobbyService;

        public LobbyController(GameLobbyService lobbyService)
        {
            _lobbyService = lobbyService;
        }

    }
}