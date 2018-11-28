using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ISPLabs.ViewModels;
using ISPLabs.Services;
using NHibernate;
using ISPLabs.Models;
using Microsoft.AspNetCore.Authorization;
using ISPLabs.Models.API;

namespace ISPLabs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicController : Controller
    {
        private NHibernateHelper nHibernateHelper;
        public TopicController(NHibernateHelper nHibernateHelper)
        {
            this.nHibernateHelper = nHibernateHelper;
        }
        [HttpGet]
        public ActionResult<ISet<TopicAPIModel>> GetAll()
        {
            using (NHibernate.ISession session = nHibernateHelper.OpenSession())
            {
                return session.Query<Topic>().Select(x => new TopicAPIModel(x, false)).ToHashSet();
            }
        }
        [HttpGet("{id}", Name = "GetTopic")]
        public ActionResult<TopicAPIModel> GetById(int id)
        {
            using (NHibernate.ISession session = nHibernateHelper.OpenSession())
            {
                var topic = session.Query<Topic>().Single(x => x.Id == id);
                if (topic == null)
                    return NotFound();
                return new TopicAPIModel(topic, true);
            }
        }
        [Authorize]
        [HttpPost]
        public ActionResult Create(NewTopicModel topic)
        {
            using (NHibernate.ISession session = nHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    var aTopic = new Topic
                    {
                        Category = session.Query<Category>().Single(x => x.Id == topic.CategoryId),
                        Name = topic.Name,
                        Date = DateTime.Now,
                        User = session.Query<User>().Single(x => x.Email == User.Identity.Name),
                        IsClosed = false,
                    };
                    var initMsg = new ForumMessage
                    {
                        Text = topic.InitialText,
                        User = aTopic.User,
                        Date = aTopic.Date,
                        Topic = aTopic,
                    };
                    aTopic.Messages.Add(initMsg);
                    session.SaveOrUpdate(aTopic);
                    session.SaveOrUpdate(initMsg);
                    transaction.Commit();
                    var dbTopic = session.Query<Topic>().Single(x => x.Name == aTopic.Name && x.Date == aTopic.Date);
                    return CreatedAtRoute("GetTopic", new { id = dbTopic.Id }, new TopicAPIModel(dbTopic));
                }
            }
        }
        [Authorize]
        [HttpPut("{id}")]
        public IActionResult Update(int id, TopicAPIModel topic)
        {
            using (ISession session = nHibernateHelper.OpenSession())
            {
                var dbtopic = session.Query<Topic>().Single(x => x.Id == id);
                dbtopic.Name = topic.Name;
                dbtopic.IsClosed = topic.IsClosed;
                if (User.Identity.Name == dbtopic.User.Email || User.IsInRole("admin"))
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.SaveOrUpdate(dbtopic);
                        transaction.Commit();
                        return NoContent();
                    }
                }
            }
            return StatusCode(403);
        }
    }
}