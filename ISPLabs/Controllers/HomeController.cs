using System;
using Microsoft.AspNetCore.Mvc;
using ISPLabs.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace ISPLabs.Controllers
{
    public class HomeController : Controller
    {
        //private ICategoryRepository categories;
        //private ITopicRepository topics;
        //public HomeController(ICategoryRepository categories, ITopicRepository topics)
        //{
        //    this.categories = categories;
        //    this.topics = topics;
        //}
        private OracleConnection _conn;

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
            //ViewBag.catId = id;
            //ViewBag.catName = categories.GetByIdWithoutChilds(id).Name;
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
            OracleCommand cmd = new OracleCommand("login", _conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;
            cmd.Parameters.Add("result", OracleDbType.Int32).Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add("pass_login", OracleDbType.Varchar2, 255).Value = "spritefok";
            cmd.Parameters.Add("pass_password", OracleDbType.Varchar2, 255).Value = "12345";
            OracleParameter errorMsg = new OracleParameter("er", OracleDbType.Varchar2, 255);
            errorMsg.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(errorMsg);
            cmd.ExecuteNonQuery();
            var r = Int32.Parse(cmd.Parameters["result"].Value.ToString());
            return Content(r.ToString());
        }
        ~HomeController()
        {
            _conn.Close();
            _conn.Dispose();
        }
    }
}
