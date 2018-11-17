using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ISPLabs.Models;
using ISPLabs.Services;
using ISPLabs.ViewModels;
using NHibernate;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace ISPLabs.Controllers
{
    public class AccountController : Controller
    {
        private NHibernateHelper nHibernateHelper;
        public AccountController(NHibernateHelper nHibernateHelper)
        {
            this.nHibernateHelper = nHibernateHelper;
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
            if (ModelState.IsValid)
            {
                using(ISession session = nHibernateHelper.OpenSession())
                {
                    User user = session.Query<User>().FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);
                    if (user != null)
                    {
                        await Authenticate(user);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                        ModelState.AddModelError("", "Incorrect login/password");
                }
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
            if (ModelState.IsValid)
            {
                using(ISession session = nHibernateHelper.OpenSession())
                {
                    User user = session.Query<User>().FirstOrDefault(u => u.Email == model.Email || u.Login == model.Login);
                    if (user == null)
                    {
                        using (ITransaction transaction = session.BeginTransaction())
                        {
                            var userRole = session.Query<Role>().FirstOrDefault(x => x.Name == "user");
                            var aUser = new User
                            {
                                Role = userRole,
                                Login = model.Login,
                                Email = model.Email,
                                Password = model.Password,
                                RegistrationDate = DateTime.Now
                            };
                            session.SaveOrUpdate(aUser);
                            transaction.Commit();
                            await Authenticate(aUser);
                        }
                        return RedirectToAction("Index", "Home");
                    }
                    else
                        ModelState.AddModelError("", "Incorrect password/login");
                }
            }
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
    }
}