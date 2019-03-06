using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ISPLabs.Models;
using ISPLabs.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Oracle.ManagedDataAccess.Client;
using ISPLabs.Services;
using System.Data;

namespace ISPLabs.Controllers
{
    public class AccountController : Controller
    {
        private OracleConnection _conn;

        public AccountController()
        {
            _conn = OracleHelper.GetDBConnection();
            _conn.Open();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid) {
                OracleCommand cmd = new OracleCommand("login", _conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.BindByName = true;
                OracleParameter result = new OracleParameter("result", OracleDbType.Boolean);
                result.Direction = ParameterDirection.ReturnValue;
                cmd.Parameters.Add(result);
                cmd.Parameters.Add("pass_login", OracleDbType.Varchar2, 255).Value = "spritefok@gmail.com";
                cmd.Parameters.Add("pass_password", OracleDbType.Varchar2, 255).Value = "123456";
                OracleParameter errorMsg = new OracleParameter("er", OracleDbType.Varchar2);
                errorMsg.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(errorMsg);
                cmd.ExecuteNonQuery();
                string r = "";
                if (result.Value != DBNull.Value)
                {
                    r = result.Value.GetType().ToString();
                }
                return Content(r);
                //await Authenticate(user);
                //return RedirectToAction("Index", "Home");
                //ModelState.AddModelError("", "Incorrect login/password");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        var u = new User
            //        {
            //            Login = model.Login,
            //            Email = model.Email,
            //            Password = model.Password,
            //            RegistrationDate = DateTime.Now
            //        };
            //        u = users.Append(u);
            //        await Authenticate(u);
            //        return RedirectToAction("Index", "Home");
            //    }
            //    catch(Exception)
            //    {
            //        ModelState.AddModelError("", "Incorrect login/email");
            //    }
                 
            //}
            return View(model);
        }
        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        ~AccountController()
        {
            _conn.Close();
            _conn.Dispose();
        }
    }
}