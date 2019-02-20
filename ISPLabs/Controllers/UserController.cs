using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ISPLabs.Models;
using ISPLabs.Services;
using NHibernate;
using ISPLabs.Models.API;
using Microsoft.AspNetCore.Authorization;
using ISPLabs.Repositories.Interfaces;

namespace ISPLabs.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
    //    private NHibernateHelper nHibernateHelper;
    //    private IUserRepository users;
    //    private IRoleRepository roles;
    //    public UserController(NHibernateHelper nHibernateHelper, IUserRepository users, IRoleRepository roles)
    //    {
    //        this.nHibernateHelper = nHibernateHelper;
    //        this.users = users;
    //        this.roles = roles;
    //    }

    //    [HttpGet]
    //    public ActionResult<ISet<UserAPIModel>> GetAll() => users.GetAll();

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