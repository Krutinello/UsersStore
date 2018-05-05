using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using UsersStore.Dal.Abstract;
using UsersStore.Dal.Entities;
using UsersStore.Web.TokenApp;

namespace UsersStore.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        private readonly IUsersRepository _usersRepository;

        public AccountController(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        /// <summary>
        /// Register user
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        [HttpPut("{login}/{password}/{firstname?}/{lastname?}")]
        public IActionResult Register(string login, string password, string firstName, string lastName)
        {
            if (String.IsNullOrWhiteSpace(login) || String.IsNullOrWhiteSpace(password))
                return BadRequest();

            User user = _usersRepository.Find(u => u.Login == login && u.Password == password).FirstOrDefault();
            if (user != null)
                return BadRequest();

            User newUser = new User
            {
                Login = login,
                Password = password,
                FirstName = firstName,
                LastName = lastName,
                Role = "user",
                CreatedDay = DateTime.Now,
                IsActive = true,
            };
            _usersRepository.Add(newUser);

            ClaimsIdentity identity = GetClaimsIdentity(newUser);

            return Ok(new AuthenticatedUserInfoJsonModel
            {
                Login = newUser.Login,
                Token = GetJwtToken(identity)
            });
        }


        /// <summary>
        /// GetToken
        /// </summary>
        /// <returns>GetJwtToken</returns>
        [HttpPost("/gettoken")]
        public IActionResult GetToken()
        {
            var login = Request.Form["login"];
            var password = Request.Form["password"];

            User user = _usersRepository.Find(u => u.Login == login && u.Password == password).FirstOrDefault();

            if (user == null)
                return BadRequest();

            ClaimsIdentity identity = GetClaimsIdentity(user);

            return Ok(new AuthenticatedUserInfoJsonModel
            {
                Login = user.Login,
                Token = GetJwtToken(identity)
            });
        }

        private ClaimsIdentity GetClaimsIdentity(User user)
        {
            Claim[] claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token");

            return claimsIdentity;
        }

        private string GetJwtToken(ClaimsIdentity identity)
        {
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: AuthJwtTokenOptions.Issuer,
                audience: AuthJwtTokenOptions.Audience,
                notBefore: DateTime.UtcNow,
                claims: identity.Claims,
                // our token will live 1 hour, but you can change you token lifetime here
                expires: DateTime.UtcNow.Add(TimeSpan.FromHours(1)),
                signingCredentials: new SigningCredentials(AuthJwtTokenOptions.GetSecurityKey(), SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }
    }
}