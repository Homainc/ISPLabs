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

namespace ISPLabs.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private NHibernateHelper nHibernateHelper;
        public AdminController(NHibernateHelper nHibernateHelper)
        {
            this.nHibernateHelper = nHibernateHelper;
        }
        public IActionResult Users()
        {
            using (ISession session = nHibernateHelper.OpenSession())
            {
                ViewBag.Roles =  session.Query<Role>().Select(x => new SelectListItem(x.Name, x.Name)).ToList().AsEnumerable();
                return View();
            }
        }
        public IActionResult Partitions()
        {
            return View();
        }
    }
}