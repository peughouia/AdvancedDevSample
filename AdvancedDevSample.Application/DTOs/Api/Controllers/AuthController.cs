using AdvancedDevSample.Application.DTOs.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedDevSample.Application.DTOs.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            // 1. Vérification de l'utilisateur fictif
            if (loginDto.Username != "admin" || loginDto.Password != "admin")
            {
                // On force le code 401 avec notre objet JSON personnalisé
                return StatusCode(401, new { erreur = "Identifiants incorrects." });
            }

            // 2. Récupération de la clé secrète depuis appsettings.json
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // 3. Création des informations contenues dans le token (Claims)
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, loginDto.Username),
            new Claim(ClaimTypes.Role, "Admin"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            // 4. Fabrication du Token (Valable 2 heures)
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // 5. On renvoie le token au client
            return Ok(new { Token = tokenString });
        }

    }
}
