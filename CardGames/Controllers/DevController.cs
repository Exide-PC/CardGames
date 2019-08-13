using Microsoft.AspNetCore.Mvc;
using CardGames.Services;

namespace CardGames.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DevController : ControllerBase
    {
        private readonly GameLobbyService _service;

        public DevController(GameLobbyService service)
        {
            _service = service;
        }

        // DELETE api/dev/delete?uid=<UID>
        [HttpDelete("delete")]
        public void DeleteById([FromQuery]string uid)
        {
            _service.DeleteLobby(uid);
        }
    }
}