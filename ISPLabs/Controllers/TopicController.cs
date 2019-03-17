﻿using ISPLabs.Manager;
using ISPLabs.Models;
using ISPLabs.Services;
using ISPLabs.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Threading.Tasks;

namespace ISPLabs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicController : Controller
    {
        private OracleConnection _conn;
        private TopicManager _topics;
        private UserManager _users;

        public TopicController()
        {
            _conn = OracleHelper.GetDBConnection();
            _conn.Open();
            _topics = new TopicManager(_conn);
            _users = new UserManager(_conn);
        }

        //[HttpGet]
        //public ActionResult<ISet<TopicAPIModel>> GetAll() => topics.GetAll();

        [HttpGet("{id}", Name = "GetTopic")]
        public async Task<ActionResult> GetById(int id)
        {
            var topic = await _topics.GetByIdAsync(id);
            if (topic == null)
                return BadRequest();
            return Ok(topic);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Create(NewTopicModel model)
        {
            if (ModelState.IsValid)
            {
                var topic = new Topic
                {
                    CategoryId = model.CategoryId,
                    Name = model.Name,
                    User = await _users.GetByEmailAsync(User.Identity.Name),
                    IsClosed = false,
                };
                var msg = new ForumMessage(model.InitialText, topic.Id, topic.User.Id);
                if (await _topics.CreateAsync(topic, msg))
                    return Ok(topic);
                else
                {
                    ModelState.AddModelError("", _topics.LastError);
                    return BadRequest(model);
                }
            }
            return BadRequest(model);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update(Topic topic)
        {
            if (await _topics.UpdateAsync(topic))
                return Ok(topic);
            return BadRequest();
        }

        ~TopicController()
        {
            _conn.Close();
            _conn.Dispose();
        }
    }
}