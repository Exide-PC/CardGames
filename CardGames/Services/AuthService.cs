﻿using CardGames.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CardGames.Services
{
    public class AuthData
    {
        public string Token { get; set; }
        public long Expires { get; set; }
    }

    public class AuthService
    {
        readonly string jwtSecret;
        readonly int jwtLifespan;

        public AuthService(string jwtSecret, int jwtLifespan)
        {
            this.jwtSecret = jwtSecret;
            this.jwtLifespan = jwtLifespan;
        }
        
        public AuthData JoinGame(string gameUid, int playerId, bool isAdmin)
        {
            List<Claim> claims = new List<Claim> {
                new Claim("GameUid", gameUid),
                new Claim("PlayerId", playerId.ToString()),
            };
            
            if (isAdmin)
                claims.Add(new Claim(ClaimTypes.Role, Role.Admin.ToString()));

            return CreateToken(claims);
        }

        public AuthData CreateToken(IEnumerable<Claim> claims)
        {
            var expirationTime = DateTime.UtcNow.AddSeconds(jwtLifespan);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expirationTime,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
            string token = tokenHandler.WriteToken(securityToken);

            return new AuthData
            {
                Token = token,
                Expires = ((DateTimeOffset)expirationTime).ToUnixTimeSeconds()
            };
        }
    }
}