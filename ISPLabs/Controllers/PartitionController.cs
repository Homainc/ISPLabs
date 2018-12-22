using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NHibernate;
using ISPLabs.Services;
using ISPLabs.Models.API;
using ISPLabs.Models;
using ISPLabs.Repositories.Interfaces;

namespace ISPLabs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartitionController : ControllerBase
    {
        private NHibernateHelper nHibernateHelper;
        private IPartitionRepository partitions;
        public PartitionController(NHibernateHelper nHibernateHelper, IPartitionRepository partitions)
        {
            this.nHibernateHelper = nHibernateHelper;
            this.partitions = partitions;
        }

        [HttpGet]
        public ActionResult<ISet<PartitionAPIModel>> GetAll() => partitions.GetAll();

        [HttpGet("{id}", Name = "GetPartition")]
        public ActionResult<PartitionAPIModel> GetById(int id) => partitions.GetByIdWithoutChilds(id);

        [HttpPost]
        public ActionResult Create(PartitionAPIModel part)
        {
            var dbPart = partitions.Append(part);
            if (dbPart == null)
                return BadRequest();
            return CreatedAtRoute("GetPartition", new { id = dbPart.Id }, dbPart);

        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, PartitionAPIModel part)
        {
            part.Id = id;
            if (partitions.Update(part))
                return NoContent();
            return BadRequest();
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