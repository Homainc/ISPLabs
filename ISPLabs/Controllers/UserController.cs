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

namespace ISPLabs.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private NHibernateHelper nHibernateHelper;
        public UserController(NHibernateHelper nHibernateHelper)
        {
            this.nHibernateHelper = nHibernateHelper;
        }
        [HttpGet]
        public ActionResult<ISet<UserAPIModel>> GetAll()
        {
            using(NHibernate.ISession session = nHibernateHelper.OpenSession())
            {
                return session.Query<User>().Select(x => new UserAPIModel(x)).ToHashSet();
            }
        }
        [HttpGet("{id}", Name = "GetUser")]
        public ActionResult<UserAPIModel> GetById(int id)
        {
            using (NHibernate.ISession session = nHibernateHelper.OpenSession())
            {
                var user = session.Query<User>().Single(x => x.Id == id);
                if (user == null)
                    return NotFound();
                return new UserAPIModel(user);
            }
        }
        [HttpPost]
        public ActionResult Create(UserAPIModel user)
        {
            using(NHibernate.ISession session = nHibernateHelper.OpenSession())
            {
                using(ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(new User
                    {
                        Login = user.Login,
                        Email = user.Email,
                        RegistrationDate = user.RegistrationDate,
                        Role = session.Query<Role>().Single(x => x.Name == user.Role),
                        Password = user.Password
                    });
                    transaction.Commit();
                    var dbUser = session.Query<User>().Single(x => x.Login == user.Login);
                    return CreatedAtRoute("GetUser", new { id = dbUser.Id }, new UserAPIModel(dbUser));
                }
            }
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, UserAPIModel user)
        {
            using(NHibernate.ISession session = nHibernateHelper.OpenSession())
            {
                var u = session.Query<User>().FirstOrDefault(x => x.Id == id);
                if (u == null)
                    return NotFound();
                u.Login = user.Login;
                u.Email = user.Email;
                u.Password = user.Password;
                u.Role = session.Query<Role>().Single(x => x.Name == user.Role);
                using(ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(u);
                    transaction.Commit();
                    return NoContent();
                }
            }
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using(NHibernate.ISession session = nHibernateHelper.OpenSession())
            {
                using(ITransaction transaction = session.BeginTransaction())
                {
                    var user = session.Query<User>().FirstOrDefault(x => x.Id == id);
                    if (user == null)
                        return NotFound();
                    session.Delete(user);
                    transaction.Commit();
                    return NoContent();
                }
            }
        }
    }
}