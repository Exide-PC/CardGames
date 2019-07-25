﻿using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CardGames.Services
{
    public interface IAuthService
    {
        AuthData Login(string login, string password);
    }

    public class AuthData
    {
        public string Token { get; set; }
        public long Expires { get; set; }
        public string Name { get; set; }
    }

    public class AuthService: IAuthService
    {
        readonly string jwtSecret;
        readonly int jwtLifespan;

        public AuthService(string jwtSecret, int jwtLifespan)
        {
            this.jwtSecret = jwtSecret;
            this.jwtLifespan = jwtLifespan;
        }
        public AuthData Login(string login, string password)
        {
            #if DEBUG
            var devUsers = new Dictionary<string, string>()
            {
                ["rib0"] = "rib0",
                ["exide"] = "exide"
            };

            if (devUsers.All(u => u.Key != login) || devUsers[login] != password)
                return null;
                
            #elif RELEASE
            return null; // will be implemented later
            #endif 


            // At this moment we know the authentification is successful

            // TODO: get claims from database (admin or user)
            var expirationTime = DateTime.UtcNow.AddSeconds(jwtLifespan);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Role, "admin")
                }),
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
                Expires = ((DateTimeOffset)expirationTime).ToUnixTimeSeconds(),
                Name = login
            };
        }
    }
}