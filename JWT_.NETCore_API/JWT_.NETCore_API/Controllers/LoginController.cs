using JWT_.NETCore_API.Models;
using Microsoft.AspNetCore.Authorization;
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

namespace JWT_.NETCore_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private IConfiguration _config;

        public LoginController(IConfiguration config)
        {
            _config = config;
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] UserModel login)
        {
            IActionResult response = Unauthorized();
            var user = AuthenticateUser(login);

            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                response = Ok(new { token = tokenString });
            }

            return response;
        }

        private string GenerateJSONWebToken(UserModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
        new Claim(JwtRegisteredClaimNames.Sub, userInfo.username),
        new Claim("Email", userInfo.email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserModel AuthenticateUser(UserModel login)
        {
            UserModel user = null;

            //Validate the User Credentials    
            //Demo Purpose, I have Passed HardCoded User Information    
            if (login.username == "sadman")
            {
                user = new UserModel { username = "sadman", email = "sadman@gmail.com" };
            }
            else
            {
                user = new UserModel { username = "admin", email = "admin@gmail.com" };

            }
            return user;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<string>> Result()
        {
            var currentUser = HttpContext.User;
            var rng = new Random();
            string EmaillVal = "";

            if (currentUser.HasClaim(c => c.Type == "Email"))
            {
                EmaillVal = currentUser.Claims.FirstOrDefault(c => c.Type == "Email").Value;
            }
            if (EmaillVal == "")
            {
                return new string[] { "empty" };

            }
            if (EmaillVal == "sadman@gmail.com")
            {
                return new string[] { "sadman" };

            }
            return new string[] { "not found" };




        }
    }
}
