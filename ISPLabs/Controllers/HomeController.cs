using System;
using Microsoft.AspNetCore.Mvc;
using ISPLabs.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Oracle.ManagedDataAccess.Client;
using ISPLabs.Manager;
using System.Threading.Tasks;

namespace ISPLabs.Controllers
{
    public class HomeController : Controller
    {
        private TopicManager _topics;

        public HomeController(OracleSession session)
        {
            _topics = new TopicManager(session.Connection);
            if (User.Identity.IsAuthenticated)
                session.AddLoginContext(User.Identity.Name);
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

        public async Task<IActionResult> Topic(int id)
        {
            var topic = await _topics.GetByIdWithUserAsync(id);
            ViewBag.TopicId = topic.Id;
            ViewBag.TopicName = topic.Name;
            ViewBag.TopicOwner = topic.User.Email;
            ViewBag.CurrentLogin = topic.User.Login;
            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> RemoveTopic(int id)
        {
            var topic = await _topics.GetByIdWithUserAsync(id);
            if (await _topics.DeleteAsync(id))
                return RedirectToAction("Category", "Home", new { id = topic.CategoryId });
            return BadRequest(_topics.LastError);
        }

        public IActionResult Test()
        {
            OracleHelper.InitFunctions();
            return Content("");
        }
    }
}
