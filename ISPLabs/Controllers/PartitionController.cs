using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NHibernate;
using ISPLabs.Services;
using ISPLabs.Models.API;
using ISPLabs.Models;

namespace ISPLabs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartitionController : ControllerBase
    {
        private NHibernateHelper nHibernateHelper;
        public PartitionController(NHibernateHelper nHibernateHelper)
        {
            this.nHibernateHelper = nHibernateHelper;
        }
        [HttpGet]
        public ActionResult<ISet<PartitionAPIModel>> GetAll()
        {
            using (NHibernate.ISession session = nHibernateHelper.OpenSession())
            {
                return session.Query<Partition>().Select(x => new PartitionAPIModel(x)).ToHashSet();
            }
        }
        [HttpGet("{id}", Name = "GetPartition")]
        public ActionResult<PartitionAPIModel> GetById(int id)
        {
            using(NHibernate.ISession session = nHibernateHelper.OpenSession())
            {
                var part = session.Query<Partition>().Single(x => x.Id == id);
                return new PartitionAPIModel(part);
            }
        }
        [HttpPost]
        public ActionResult Create(PartitionAPIModel part)
        {
            using (NHibernate.ISession session = nHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    var addedPart = new Partition
                    {
                        Name = part.Name
                    };
                    session.SaveOrUpdate(addedPart);
                    transaction.Commit();
                    var dbPart = session.Query<Partition>().Single(x => x.Name == addedPart.Name);
                    return CreatedAtRoute("GetPartition", new { id = dbPart.Id }, new PartitionAPIModel(dbPart));
                }
            }
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, PartitionAPIModel part)
        {
            using(NHibernate.ISession session = nHibernateHelper.OpenSession())
            {
                using(ITransaction transaction = session.BeginTransaction())
                {
                    var addedPart = session.Query<Partition>().Single(x => x.Id == id);
                    addedPart.Name = part.Name;
                    session.Update(addedPart);
                    transaction.Commit();
                    return NoContent();
                }
            }
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using(NHibernate.ISession session = nHibernateHelper.OpenSession())
            {
                using(ITransaction transaction = session.BeginTransaction())
                {
                    var part = session.Query<Partition>().FirstOrDefault(x => x.Id == id);
                    if (part == null)
                        return NotFound();
                    session.Delete(part);
                    transaction.Commit();
                    return NoContent();
                }
            }
        }
    }
}