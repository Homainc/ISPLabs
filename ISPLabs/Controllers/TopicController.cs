using ISPLabs.Manager;
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

        //[HttpGet("{id}", Name = "GetTopic")]
        //public ActionResult<TopicAPIModel> GetById(int id) => topics.GetById(id);

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
                string error;
                if (_topics.Create(topic, msg, out error))
                    return Ok(topic);
                else
                {
                    ModelState.AddModelError("", error);
                    return BadRequest(model);
                }
            }
            return BadRequest(model);
        }

        //[Authorize]
        //[HttpPut("{id}")]
        //public IActionResult Update(int id, TopicAPIModel topic)
        //{
        //    var dbTopic = topics.GetByIdWithUser(id);
        //    topic.Id = id;
        //    if (User.Identity.Name == dbTopic.User.Email || User.IsInRole("admin"))
        //    {
        //        if(topics.Update(topic))
        //            return NoContent();
        //        return BadRequest();
        //    }
        //    return StatusCode(403);
        //}

        ~TopicController()
        {
            _conn.Close();
            _conn.Dispose();
        }
    }
}