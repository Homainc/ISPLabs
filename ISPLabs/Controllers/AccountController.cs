using System.Collections.Generic;
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
using ISPLabs.Manager;

namespace ISPLabs.Controllers
{
    public class AccountController : Controller
    {
        private OracleConnection _conn;
        private UserManager _users;
        private RoleManager _roles;

        public AccountController()
        {
            _conn = OracleHelper.GetDBConnection();
            _conn.Open();
            _users = new UserManager(_conn);
            _roles = new RoleManager(_conn);
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid) {
                if (await _users.LoginAsync(model.Email, model.Password))
                {
                    await Authenticate(await _users.GetByEmailAsync(model.Email));
                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", _users.LastError);
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Login = model.Login,
                    Email = model.Email,
                    Password = model.Password,
                    Role = await _roles.GetByNameAsync("user")
                };
                if (await _users.RegistrationAsync(user))
                {
                    await Authenticate(user);
                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", _users.LastError);
            }
            return View(model);
        }
        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.Name)
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