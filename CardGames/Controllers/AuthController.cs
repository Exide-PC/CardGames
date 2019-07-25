using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CardGames.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CardGames.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private IAuthService _authService;

        public class LoginParameters
        {
            public string Login { get; set; }
            public string Password { get; set; }
        }

        public AuthController(IAuthService authService)
        {
            this._authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public ActionResult<AuthData> Login([FromForm]LoginParameters loginData)
        {
            var token = _authService.Login(
                loginData.Login, 
                loginData.Password);

            if (token == null)
                return Unauthorized();

            return token;
        }
    }
}