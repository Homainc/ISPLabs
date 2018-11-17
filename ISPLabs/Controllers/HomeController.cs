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
                var parts = session.Query<Partition>().ToList();
                return View(parts);
            }
        }
        public IActionResult Category(int id)
        {
            using (NHibernate.ISession session = nHibernateHelper.OpenSession())
            {
                ViewBag.Category = session.Query<Category>().Where(x => x.Id == id).First();
                var parts = session.Query<Topic>().Where(x => x.Category.Id == id).ToList();
                return View(parts);
            }
        }
        public IActionResult Topic(int id)
        {
            using (NHibernate.ISession session = nHibernateHelper.OpenSession())
            {
                var topic = session.Query<Topic>().Where(x => x.Id == id).First();
                ViewBag.Topic = topic;
                ViewBag.TopicOwner = topic.User.Email;
                var parts = session.Query<ForumMessage>().Where(x => x.Topic.Id == id).Select(x =>
                    new ForumMessageModel
                    {
                        Id = x.Id,
                        Date = x.Date,
                        Text = x.Text,
                        UserLogin = x.User.Login,
                        UserMessages = x.User.Messages.Count,
                        IsTopicOwner = x.User.Email == User.Identity.Name,
                        UserRole = x.User.Role.Name
                    }    
                ).ToList();
                return View(parts);
            }
        }
    }
}
