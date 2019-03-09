using ISPLabs.Manager;
using ISPLabs.Models;
using ISPLabs.Services;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ISPLabs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartitionController : ControllerBase
    {
        private OracleConnection _conn;
        private PartitionManager _partitions;

        public PartitionController()
        {
            _conn = OracleHelper.GetDBConnection();
            _conn.Open();
            _partitions = new PartitionManager(_conn);
        }

        [HttpGet]
        public async Task<ICollection<Partition>> GetAll() => await _partitions.GetAllAsync();

        //[HttpGet("{id}", Name = "GetPartition")]
        //public ActionResult<PartitionAPIModel> GetById(int id) => partitions.GetByIdWithoutChilds(id);

        //[HttpPost]
        //public ActionResult Create(PartitionAPIModel part)
        //{
        //    var dbPart = partitions.Append(part);
        //    if (dbPart == null)
        //        return BadRequest();
        //    return CreatedAtRoute("GetPartition", new { id = dbPart.Id }, dbPart);

        //}

        //[HttpPut("{id}")]
        //public IActionResult Update(int id, PartitionAPIModel part)
        //{
        //    part.Id = id;
        //    if (partitions.Update(part))
        //        return NoContent();
        //    return BadRequest();
        //}
        //[HttpDelete("{id}")]
        //public IActionResult Delete(int id)
        //{
        //    using(NHibernate.ISession session = nHibernateHelper.OpenSession())
        //    {
        //        using(ITransaction transaction = session.BeginTransaction())
        //        {
        //            var part = session.Query<Partition>().FirstOrDefault(x => x.Id == id);
        //            if (part == null)
        //                return NotFound();
        //            session.Delete(part);
        //            transaction.Commit();
        //            return NoContent();
        //        }
        //    }
        //}

        ~PartitionController()
        {
            _conn.Clone();
            _conn.Dispose();
        }
    }
}