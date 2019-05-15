using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ISPLabs.Models;
using ISPLabs.Services;
using Oracle.ManagedDataAccess.Client;
using ISPLabs.Manager;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ISPLabs.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private OracleConnection _conn;
        private UserManager _users;

        public UserController(OracleSession session)
        {
            _users = new UserManager(session.Connection);
        }

        [HttpGet]
        public async Task<ICollection<User>> GetAll() => await _users.GetAllAsync();

        //    [HttpGet("{id}", Name = "GetUser")]
        //    public ActionResult<UserAPIModel> GetById(int id) => users.GetById(id);

        //    [HttpPost]
        //    public ActionResult Create(UserAPIModel model)
        //    {
        //        var user = new User
        //        {
        //            Login = model.Login,
        //            Email = model.Email,
        //            RegistrationDate = DateTime.Now,
        //            Role = roles.GetByName(model.Role),
        //            Password = model.Password
        //        };
        //        user = users.Append(user);
        //        return CreatedAtRoute("GetUser", new { id = user.Id }, new UserAPIModel(user));
        //    }

        //    [HttpPut("{id}")]
        //    public IActionResult Update(int id, UserAPIModel model)
        //    {
        //        var user = new User
        //        {
        //            Id = id,
        //            Login = model.Login,
        //            Email = model.Email,
        //            Role = roles.GetByName(model.Role),
        //            Password = model.Password
        //        };
        //        if(users.Update(user))
        //            return NoContent();
        //        return BadRequest();

        //    }
        //    [HttpDelete("{id}")]
        //    public IActionResult Delete(int id)
        //    {
        //        using(NHibernate.ISession session = nHibernateHelper.OpenSession())
        //        {
        //            using(ITransaction transaction = session.BeginTransaction())
        //            {
        //                var user = session.Query<User>().FirstOrDefault(x => x.Id == id);
        //                if (user == null)
        //                    return NotFound();
        //                session.Delete(user);
        //                transaction.Commit();
        //                return NoContent();
        //            }
        //        }
        //    }
    }
}