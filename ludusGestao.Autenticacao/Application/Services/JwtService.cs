using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ludusGestao.Autenticacao.Domain.Entities;
using Microsoft.Extensions.Options;

namespace ludusGestao.Autenticacao.Application.Services
{
    public class JwtSettings
    {
        public string Secret { get; set; } = default!;
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
        public int JwtExpiracaoMinutos { get; set; }
        public int RefreshExpiracaoDias { get; set; }
    }

    public class JwtService
    {
        private readonly string _jwtSecret;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _jwtExpiracaoMinutos;
        private readonly int _refreshExpiracaoDias;

        public JwtService(IOptions<JwtSettings> options)
        {
            var settings = options.Value;
            _jwtSecret = settings.Secret;
            _issuer = settings.Issuer;
            _audience = settings.Audience;
            _jwtExpiracaoMinutos = settings.JwtExpiracaoMinutos;
            _refreshExpiracaoDias = settings.RefreshExpiracaoDias;
        }

        public string GerarJwt(UsuarioAutenticacao usuario)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, usuario.Login),
                new Claim("tenantId", usuario.TenantId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtExpiracaoMinutos),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public int JwtExpiracaoMinutos => _jwtExpiracaoMinutos;

        public string GerarRefreshToken(UsuarioAutenticacao usuario)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, usuario.Login),
                new Claim("tenantId", usuario.TenantId.ToString()),
                new Claim("tipo", "refresh")
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(_refreshExpiracaoDias),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GerarRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public ClaimsPrincipal? ValidarJwt(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSecret);
            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _issuer,
                    ValidAudience = _audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out _);
                return principal;
            }
            catch
            {
                return null;
            }
        }

        public DateTime CalcularExpiracaoRefreshToken() => DateTime.UtcNow.AddDays(_refreshExpiracaoDias);
    }
} 