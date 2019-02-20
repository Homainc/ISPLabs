using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ISPLabs.Models.API;
using ISPLabs.Services;
using ISPLabs.Models;
using ISPLabs.Repositories.Interfaces;

namespace ISPLabs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        //private IRoleRepository roles;
        //public RoleController(IRoleRepository roles) => this.roles = roles;

        //[HttpGet]
        //public ActionResult<ISet<RoleAPIModel>> GetAll() => roles.GetAll();
    }
}