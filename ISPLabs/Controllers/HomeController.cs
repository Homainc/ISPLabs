using System;
using Microsoft.AspNetCore.Mvc;
using ISPLabs.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using ISPLabs.Models;
using System.Collections.Generic;
using ISPLabs.Manager;
using Oracle.ManagedDataAccess.Types;
using System.Threading.Tasks;

namespace ISPLabs.Controllers
{
    public class HomeController : Controller
    {
        private OracleConnection _conn;
        private CategoryManager _categories;

        public HomeController()
        {
            _conn = OracleHelper.GetDBConnection();
            _conn.Open();
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

        public IActionResult Index() => View();

        public IActionResult Category(int id)
        {
            ViewBag.catId = id;
            return View();
        }

        public IActionResult Topic(int id)
        {
            //var topic = topics.GetByIdWithUser(id);
            //ViewBag.TopicId = topic.Id;
            //ViewBag.TopicName = topic.Name;
            //ViewBag.TopicOwner = topic.User.Email;
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult RemoveTopic(int id)
        {
            //var topic = topics.GetByIdWithCategory(id);
            //if (topic != null && (User.Identity.Name == topic.User.Email || User.IsInRole("admin")))
            //{
            //    if(!topics.Remove(id))
            //        return BadRequest();
            //    return RedirectToAction("Category", "Home", new { id = topic.Category.Id });
            //}
            return StatusCode(403);
        }

        public IActionResult Test()
        {
            OracleHelper.InitFunctions();
            return Content("");
        }

        ~HomeController()
        {
            _conn.Close();
            _conn.Dispose();
        }
    }
}
