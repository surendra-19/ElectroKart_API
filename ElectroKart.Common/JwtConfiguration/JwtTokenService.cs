using ElectroKart.Common.Configuration;
using ElectroKart.Common.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ElectroKart.Common.JwtConfiguration
{
    public class JwtTokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<JwtTokenService> _logger;
        public JwtTokenService(IOptions<JwtSettings> jwtSettings,ILogger<JwtTokenService> logger)
        {
            _jwtSettings = jwtSettings.Value;
            _logger = logger;
        }

        public string GenerateToken(Customer customer)
        {
            try
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
                var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, customer.Cust_Id.ToString()),
            new Claim(ClaimTypes.Name, customer.FirstName + " " + customer.LastName),
            new Claim(ClaimTypes.Email, customer.Email),
            new Claim(ClaimTypes.Role, customer.Role)
        };

                var token = new JwtSecurityToken(
                    issuer: _jwtSettings.Issuer,
                    audience: _jwtSettings.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(_jwtSettings.ExpirationMinutes),
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while generating JWT token for customer: {customer.Cust_Id}");

                throw new ApplicationException("Token generation failed.");
            }
        }

    }
}
