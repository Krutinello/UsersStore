using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UsersStore.Web.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        [Authorize]
        [HttpGet("/getlogin")]
        public IActionResult GetLogin()
        {
            return Ok($"Ваш id: {User.Identity.Name}");
        }
        [Authorize(Roles = "admin")]
        [HttpGet("/getrole")]
        public IActionResult GetRole()
        {
            return Ok("Ваша роль: администратор");
        }
    }
}
