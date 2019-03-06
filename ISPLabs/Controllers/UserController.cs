using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ISPLabs.Models;
using ISPLabs.Services;
using Microsoft.AspNetCore.Authorization;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace ISPLabs.Controllers
{
    //[Authorize(Roles = "admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private OracleConnection _conn;

        public UserController()
        {
            _conn = OracleHelper.GetDBConnection();
            _conn.Open();
        }

        [HttpGet]
        public ActionResult<ICollection<User>> GetAll() {
            OracleCommand cmd = new OracleCommand("get_users", _conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;
            OracleParameter resItems = cmd.Parameters.Add("result_users", OracleDbType.RefCursor);
            resItems.Direction = ParameterDirection.Output;
            OracleDataReader reader;
            List<User> list = new List<User>();
            try
            {
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var uId = decimal.ToInt32((decimal)reader["user_id"]);
                    var uEmail = reader["user_email"] as string;
                    var uLogin = reader["user_login"] as string;
                    var uRegDate = (DateTime)reader["user_reg_date"];
                    var uRId = decimal.ToInt32((decimal)reader["role_id"]);
                    var uRName = reader["role_name"] as string;
                    list.Add(new User(uId, uLogin, uEmail, uRegDate, new Role(uRId, uRName)));
                }
                return new JsonResult(list);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

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
        ~UserController()
        {
            _conn.Close();
            _conn.Dispose();
        }
    }
}