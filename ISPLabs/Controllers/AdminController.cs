using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISPLabs.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        //private IRoleRepository roles;
        //public AdminController(IRoleRepository roles) => this.roles = roles;

        public IActionResult Users()
        {
            //ViewBag.Roles = roles.GetAll().Select(x => new SelectListItem(x.Name, x.Name)).ToList().AsEnumerable();
            return View();
        }

        public IActionResult Partitions() => View();
    }
}