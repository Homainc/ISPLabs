using ISPLabs.Models;
using ISPLabs.Services;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace ISPLabs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartitionController : ControllerBase
    {
        private OracleConnection _conn;

        public PartitionController()
        {
            _conn = OracleHelper.GetDBConnection();
            _conn.Open();
        }

        [HttpGet]
        public ActionResult<ICollection<Partition>> GetAll()
        {
            OracleCommand cmd = new OracleCommand("get_partition_eager", _conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;
            OracleParameter resItems = cmd.Parameters.Add("resultItems", OracleDbType.RefCursor);
            resItems.Direction = ParameterDirection.Output;
            OracleDataReader reader;
            Dictionary<int,Partition> dict = new Dictionary<int, Partition>();
            try
            {
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var pid = decimal.ToInt32((decimal)reader["partition_id"]);
                    Partition partition;
                    if (!dict.TryGetValue(pid, out partition)) {
                        partition = new Partition(pid, reader["partition_name"] as string);
                        dict.Add(pid, partition);
                    }
                    var cid = decimal.ToInt32((decimal)reader["category_id"]);
                    var cname = reader["category_name"] as string;
                    var cdesc = reader["category_description"] as string;
                    var ctcount = decimal.ToInt32((decimal)reader["topic_count"]);
                    partition.Categories.Add(new Category(cid, cname, cdesc, ctcount, partition));
                }
                return new JsonResult(dict.Values);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

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