
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtLogin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly string _username = "a";
        private readonly string _password = "a";
        private readonly string _adminUsername = "b";
        private readonly string _adminPassword = "b";
        private readonly string _secretKey = "this_is_my_custom_secret_key_for_jwt";


        private readonly string _issuer = "https://your-auth-server.com";
        private readonly string _audience = "https://your-api.com";

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel login)
        {
            if (login.Username == _username && login.Password == _password)
            {
                var token = GenerateJwtToken(login.Username, "User");
                return Ok(new { token, message = "Hoş geldiniz" +" "+ _username + "!" });
            }
            else if (login.Username == _adminUsername && login.Password == _adminPassword)
            {
                // Admin kullanıcı için token oluştur
                var token = GenerateJwtToken(login.Username, "Admin");
                return Ok(new { token, message = "Hoş geldiniz" +" "+ _adminUsername + "!" });
            }


            return Unauthorized("Geçersiz kullanıcı adı veya şifre.");
        }

        private string GenerateJwtToken(string username, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, role) // Role bilgisi ekliyoruz
                }),
                Expires = null,

                Issuer = _issuer, // Token'ı kim oluşturduğunu belirtir
                Audience = _audience, // Token'ın kimler tarafından kullanılabileceğini belirtir

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}