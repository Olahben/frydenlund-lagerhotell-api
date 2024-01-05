using LagerhotellAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LagerhotellAPI.Services
{
    public class TokenService
    {
        public Jwt CreateJwt(string id, string phoneNumber)
        {
            // bitSecret should be fixed
            string bitSecret;
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                var secretKeyByteArray = new byte[32];
                rng.GetBytes(secretKeyByteArray);
                bitSecret = Convert.ToBase64String(secretKeyByteArray);
            }
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(bitSecret));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokeOptions = new JwtSecurityToken(
                issuer: "https://localhost:7272",
                audience: "https://localhost:5001",
                claims: new List<Claim>()
                {
            new Claim(ClaimTypes.Sid, id),
            new Claim(ClaimTypes.MobilePhone, phoneNumber)

                },
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return new Jwt { Token = tokenString };
        }
    }
}
