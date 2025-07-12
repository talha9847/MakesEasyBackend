using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MakesEasy.Models;
using System.Linq;

namespace MakesEasy.Services
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwtToken(UserModel user)
        {
            var role = user.Role.StartsWith("Admin") ? "Admin" : "User";
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Role, role),
                new Claim("villageId", user.VillageId.ToString()),
                new Claim("talukaId", user.TalukaId.ToString()),
                new Claim("distId", user.DistId.ToString()),
                new Claim("stateId", user.StateId.ToString()),
                new Claim("countryId", user.CountryId.ToString()),
            };
            System.Console.WriteLine(user.VillageId);
            if (user.Role == "Admin1")
            {

                claims.Add(new Claim("type", "village_id"));
                claims.Add(new Claim("id", user.VillageId.ToString()));
            }
            else if (user.Role == "Admin2")
            {
                claims.Add(new Claim("type", "taluka_id"));
                claims.Add(new Claim("id", user.TalukaId.ToString()));

            }
            else if (user.Role == "Admin3")
            {
                claims.Add(new Claim("type", "dist_id"));
                claims.Add(new Claim("id", user.DistId.ToString()));

            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        public string GenerateTempToken(int userId)
        {
            var claims = new[]
                {
                    new Claim("userId", userId.ToString()),
                    new Claim("type", "temporary") // used to distinguish this token type later
                };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(5),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
