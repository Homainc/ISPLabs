using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ISPLabs.Models.API;
using ISPLabs.Services;
using ISPLabs.Models; 

namespace ISPLabs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private NHibernateHelper nHibernateHelper;
        public RoleController(NHibernateHelper nHibernateHelper)
        {
            this.nHibernateHelper = nHibernateHelper;
        }
        [HttpGet]
        public ActionResult<ISet<RoleAPIModel>> GetAll()
        {
            using (NHibernate.ISession session = nHibernateHelper.OpenSession())
            {
                return session.Query<Role>().Select(x => new RoleAPIModel(x)).ToHashSet();
            }
        }
    }
}