using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ISPLabs.Models;
using NHibernate;
using ISPLabs.Services;
using ISPLabs.ViewModels;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace ISPLabs.Controllers
{
    public class HomeController : Controller
    {
        private NHibernateHelper nHibernateHelper;
        public HomeController(NHibernateHelper nHibernateHelper)
        {
            this.nHibernateHelper = nHibernateHelper;
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
        public IActionResult Index()
        {
            //nHibernateHelper.AddTest();
            using (NHibernate.ISession session = nHibernateHelper.OpenSession())
            {
                return View();
            }
        }
        public IActionResult Category(int id)
        {
            using (NHibernate.ISession session = nHibernateHelper.OpenSession())
            {
                ViewBag.catId = id;
                ViewBag.catName = session.Query<Category>().Single(x => x.Id == id).Name;
                return View();
            }
        }
        public IActionResult Topic(int id)
        {
            using (NHibernate.ISession session = nHibernateHelper.OpenSession())
            {
                var topic = session.Query<Topic>().Single(x => x.Id == id);
                ViewBag.TopicId = topic.Id;
                ViewBag.TopicName = topic.Name;
                ViewBag.TopicOwner = topic.User.Email;
                return View();
            }
        }
        [Authorize]
        [HttpGet]
        public IActionResult RemoveTopic(int id)
        {
            using (NHibernate.ISession session = nHibernateHelper.OpenSession())
            {
                var topic = session.Query<Topic>().FirstOrDefault(x => x.Id == id);
                var catId = topic.Category.Id;
                if (topic != null && (User.Identity.Name == topic.User.Email || User.IsInRole("admin")))
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Delete(topic);
                        transaction.Commit();
                        return RedirectToAction("Category", "Home", new { id = catId });
                    }
                }
            }
            return StatusCode(403);
        }
    }
}
