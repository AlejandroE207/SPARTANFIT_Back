using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace SPARTANFIT.Utilitys
{
    public class TokenUtility
    {
        private readonly string _secretKey;

        public TokenUtility(string secretKey)
        {
            _secretKey = secretKey; // Clave secreta desde la configuración
        }

        public string GenerarToken(string correo)
        {
            var securityKey = Encoding.ASCII.GetBytes(_secretKey);

            IEnumerable<Claim> claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, correo),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var tokenDescriptor = new JwtSecurityToken(
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddDays(3),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(securityKey), SecurityAlgorithms.HmacSha256)
            );

            var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            return token;
        }
    }
}
