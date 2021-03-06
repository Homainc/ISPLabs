﻿using ISPLabs.Manager;
using ISPLabs.Models;
using ISPLabs.Services;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISPLabs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartitionController : ControllerBase
    {
        private PartitionManager _partitions;

        public PartitionController(OracleSession session)
        {
            _partitions = new PartitionManager(session.Connection);
        }

        [HttpGet]
        public async Task<ICollection<Partition>> GetAll() => await _partitions.GetAllAsync();

        [HttpPost]
        public async Task<ActionResult> Create(Partition partition)
        {
            if (await _partitions.CreateAsync(partition, User.Identity.Name))
                return Ok(partition);
            return BadRequest(_partitions.LastError);
        }

        [HttpPut]
        public async Task<IActionResult> Update(Partition partition)
        {
            if (await _partitions.UpdateAsync(partition, User.Identity.Name))
                return Ok(partition);
            return BadRequest(_partitions.LastError);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _partitions.DeleteAsync(id, User.Identity.Name))
                return Ok(id);
            return BadRequest(_partitions.LastError);
        }
    }
}