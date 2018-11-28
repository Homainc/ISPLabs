using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using NHibernate;
using ISPLabs.Services;
using ISPLabs.Models;
using ISPLabs.Models.API;

namespace ISPLabs.Hubs
{
    public class MessageHub : Hub
    {
        private NHibernateHelper nHibernateHelper;
        public MessageHub(NHibernateHelper nHibernateHelper)
        {
            this.nHibernateHelper = nHibernateHelper;
        }
        [Authorize]
        public async Task Send(string message, int topicId)
        {
            using (ISession session = nHibernateHelper.OpenSession())
            {
                var topic = session.Query<Topic>().Single(x => x.Id == topicId);
                if (!topic.IsClosed)
                {
                    var msg = new ForumMessage
                    {
                        User = session.Query<User>().Single(x => x.Email == Context.User.Identity.Name),
                        Date = DateTime.Now,
                        Text = message,
                        Topic = topic,
                    };
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        topic.Messages.Add(msg);
                        session.SaveOrUpdate(msg);
                        session.SaveOrUpdate(topic);
                        transaction.Commit();
                    }
                    var dbMsg = session.Query<ForumMessage>().Single(x => x.Date == msg.Date);
                    await Clients.All.SendAsync($"Receive{topicId.ToString()}", new MessageAPIModel(dbMsg));
                }
            }
        }
        [Authorize]
        public async Task DeleteMessage(int id)
        {
            using(ISession session = nHibernateHelper.OpenSession())
            {
                var msg = session.Query<ForumMessage>().Single(x => x.Id == id);
                var topicId = msg.Topic.Id;
                if (Context.User.Identity.Name == msg.User.Email || Context.User.IsInRole("admin"))
                {
                    using(ITransaction transaction = session.BeginTransaction())
                    {
                        session.Delete(msg);
                        transaction.Commit();
                        await Clients.All.SendAsync($"Deleted{topicId}", id);
                    }
                }
            }
        }
        [Authorize]
        public async Task EditMessage(int id, string text)
        {
            using (ISession session = nHibernateHelper.OpenSession())
            {
                var msg = session.Query<ForumMessage>().Single(x => x.Id == id);
                var topicId = msg.Topic.Id;
                if (Context.User.Identity.Name == msg.User.Email || Context.User.IsInRole("admin"))
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        msg.Text = text;
                        session.SaveOrUpdate(msg);
                        transaction.Commit();
                        await Clients.All.SendAsync($"Changed{topicId}", id, text);
                    }
                }
            }
        }
    }
}
