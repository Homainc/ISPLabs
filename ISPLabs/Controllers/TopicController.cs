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

namespace ISPLabs.Controllers
{
    [Authorize]
    public class TopicController : Controller
    {
        private NHibernateHelper nHibernateHelper;
        public TopicController(NHibernateHelper nHibernateHelper)
        {
            this.nHibernateHelper = nHibernateHelper;
        }
        [HttpPost]
        public IActionResult SendMessage(SendForumMessageModel model)
        {
            if (ModelState.IsValid)
            {
                using (ISession session = nHibernateHelper.OpenSession())
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        var topic = session.Query<Topic>().FirstOrDefault(x => x.Id == model.TopicId);
                        if (topic.IsClosed)
                            return StatusCode(403);
                        var user = session.Query<User>().FirstOrDefault(x => x.Email == User.Identity.Name);
                        var msg = new ForumMessage
                        {
                            Text = model.Text,
                            Topic = topic,
                            User = user,
                            Date = DateTime.Now
                        };
                        session.SaveOrUpdate(msg);
                        transaction.Commit();
                    }

                }
                return RedirectToAction("Topic", "Home", new { id = model.TopicId });
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult DeleteMessage(int id)
        {
            using (ISession session = nHibernateHelper.OpenSession())
            {
                var msg = session.Query<ForumMessage>().FirstOrDefault(x => x.Id == id);
                if (msg == null)
                    return StatusCode(403);
                if(User.Identity.Name == msg.User.Email || User.IsInRole("admin"))
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        var topic = msg.Topic;
                        session.Delete(msg);
                        transaction.Commit();
                        return RedirectToAction("Topic", "Home", new { id = topic.Id });
                    }
                }
            }
            return StatusCode(403);
        }
        [HttpGet]
        public IActionResult EditMessage(int id)
        {
            using (ISession session = nHibernateHelper.OpenSession())
            {
                var msg = session.Query<ForumMessage>().FirstOrDefault(x => x.Id == id);
                if (User.Identity.Name == msg.User.Email || User.IsInRole("admin"))
                {
                    ViewBag.Topic = msg.Topic.Name;
                    return View(new SendForumMessageModel { Text = msg.Text , Id = msg.Id , TopicId = msg.Topic.Id });
                }
            }
            return StatusCode(403);
        }
        [HttpPost]
        public IActionResult EditMessage(SendForumMessageModel model)
        {
            if (ModelState.IsValid)
            {
                using (ISession session = nHibernateHelper.OpenSession())
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        var msg = session.Query<ForumMessage>().FirstOrDefault(x => x.Id == model.Id);
                        var user = session.Query<User>().FirstOrDefault(x => x.Email == User.Identity.Name);
                        msg.Text = model.Text;
                        session.Update(msg);
                        transaction.Commit();
                        return RedirectToAction("Topic", "Home", new { id = msg.Topic.Id });
                    }
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult New(int id)
        {
            using(ISession session = nHibernateHelper.OpenSession())
            {
                var user = session.Query<User>().FirstOrDefault(x => x.Email == User.Identity.Name);
                var cat = session.Query<Category>().FirstOrDefault(x => x.Id == id);
                if (cat == null)
                    return StatusCode(403);
                ViewBag.Category = cat;
            }
            return View();
        }
        [HttpPost]
        public IActionResult New(NewTopicModel model)
        {
            if (ModelState.IsValid)
            {
                using (ISession session = nHibernateHelper.OpenSession())
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        var user = session.Query<User>().FirstOrDefault(x => x.Email == User.Identity.Name);
                        var cat = session.Query<Category>().FirstOrDefault(x => x.Id == model.CategoryId);
                        var topic = new Topic
                        {
                            Name = model.Name,
                            Date = DateTime.Now,
                            User = user,
                            Category = cat,
                            IsClosed = false
                        };
                        var initMsg = new ForumMessage
                        {
                            User = user,
                            Topic = topic,
                            Date = DateTime.Now,
                            Text = model.InitialText
                        };
                        session.SaveOrUpdate(topic);
                        session.SaveOrUpdate(initMsg);
                        transaction.Commit();
                        return RedirectToAction("Topic", "Home", new { id = topic.Id });
                    }
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Remove(int id)
        {
            using (ISession session = nHibernateHelper.OpenSession()) {
                var topic = session.Query<Topic>().FirstOrDefault(x => x.Id == id);
                var catId = topic.Category.Id;
                if (topic != null && (User.Identity.Name == topic.User.Email || User.IsInRole("admin")))
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Delete(topic);
                        transaction.Commit();
                        return RedirectToAction("Category", "Home", new { id = catId });
                    }
                }
            }
            return StatusCode(403);
        }
        public IActionResult Close(int id)
        {
            using (ISession session = nHibernateHelper.OpenSession())
            {
                var topic = session.Query<Topic>().FirstOrDefault(x => x.Id == id);
                if (topic != null && (User.Identity.Name == topic.User.Email || User.IsInRole("admin")))
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        topic.IsClosed = true;
                        session.SaveOrUpdate(topic);
                        transaction.Commit();
                        return RedirectToAction("Topic", "Home", new { id = topic.Id });
                    }
                }
            }
            return StatusCode(403);
        }
    }
}