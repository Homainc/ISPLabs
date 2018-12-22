using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ISPLabs.Services;
using ISPLabs.Models;
using NHibernate;
using Microsoft.AspNetCore.Mvc.Rendering;
using ISPLabs.Repositories.Interfaces;

namespace ISPLabs.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private IRoleRepository roles;
        public AdminController(IRoleRepository roles) => this.roles = roles;

        public IActionResult Users()
        {
            ViewBag.Roles = roles.GetAll().Select(x => new SelectListItem(x.Name, x.Name)).ToList().AsEnumerable();
            return View();
        }

        public IActionResult Partitions() => View();
    }
}