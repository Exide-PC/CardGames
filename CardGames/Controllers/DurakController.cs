using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CardGames.Services;

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
    }
}