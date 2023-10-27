using DomainLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InfrastructureLayer.Helper
{
    public class JwtTokenHelper
    {
        private readonly string secretKey;
        private readonly string issuer;
        private readonly string audience;
        private readonly double tokenExpirationMinutes;

        public JwtTokenHelper(string secretKey, string issuer, string audience, double tokenExpirationMinutes)
        {
            this.secretKey = secretKey;
            this.issuer = issuer;
            this.audience = audience;
            this.tokenExpirationMinutes = tokenExpirationMinutes;

        }

        public string GenerateToken(ClaimsIdentity claimsIdentity, int? paramTokenExpirationMinutes = null)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claimsIdentity.Claims,
                expires: paramTokenExpirationMinutes == null ? DateTime.UtcNow.AddMinutes(tokenExpirationMinutes) : DateTime.UtcNow.AddMinutes(paramTokenExpirationMinutes.Value),
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
