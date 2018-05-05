using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UsersStore.Dal.Abstract;
using UsersStore.Dal.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UsersStore.Web.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUsersRepository _usersRepository;

        public UsersController(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
        }

        // GET: api/<controller>
        /// <summary>
        /// The list of all people
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<User> Get()
        {
            List<User> users=_usersRepository.Get().ToList();
            return users;
         } 

        /// <summary>
        /// This will provide details for specific ID which is begin passed
        /// </summary>
        /// <param name="id">Mandatory</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            User user = _usersRepository.GetById(id);
            if (user != null)
                return Ok(user);

            return NotFound();
        }

        /// <summary>
        /// Adding new person into People list
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody]User user)
        {
            if (ModelState.IsValid)
            {
                if(user.Role!="admin" && String.IsNullOrWhiteSpace(user.Role))
                    user.Role = "user";
                _usersRepository.Add(user);
                return Ok();
            }

            return BadRequest();
        }

        /// <summary>
        /// Editing person by ID
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Put([FromBody]User user)
        {
            if (ModelState.IsValid)
            {
                User u = _usersRepository.GetById(user.Id);
                if (u != null)
                {
                    u.Login = user.Login;
                    u.Password = user.Password;
                    u.Role = user.Role;
                    u.FirstName = user.FirstName;
                    u.LastName = user.LastName;
                    u.CreatedDay = user.CreatedDay;
                    u.IsActive = user.IsActive;
                    _usersRepository.Update(u);
                    return Ok();
                }

                return NotFound();
            }

            return BadRequest();
        }

        /// <summary>
        /// Removeing person from people list
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            User u = _usersRepository.GetById(id);
            if (u != null)
            {
                _usersRepository.Delete(id);
                return Ok();
            }

            return NotFound();
        }
    }
}
