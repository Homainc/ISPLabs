using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using NHibernate;
using ISPLabs.Services;
using ISPLabs.Models;
using ISPLabs.Models.API;
using Oracle.ManagedDataAccess.Client;
using ISPLabs.Manager;

namespace ISPLabs.Hubs
{
    public class MessageHub : Hub
    {
        private OracleConnection _conn;
        private ForumMessageManager _messages;
        private UserManager _users;

        public MessageHub()
        {
            _conn = OracleHelper.GetDBConnection();
            _messages = new ForumMessageManager(_conn);
            _users = new UserManager(_conn);
        }

        [Authorize]
        public async Task Send(string message, int topicId)
        {
            var user = await _users.GetByEmailAsync(Context.User.Identity.Name);
            var msg = new ForumMessage(message, topicId, user.Id);
            string error;
            if(_messages.Create(msg, out error))
            {
                await Clients.All.SendAsync($"Receive{topicId}", msg);
            }
        }

        //[Authorize]
        //public async Task DeleteMessage(int id)
        //{
        //    using (ISession session = nHibernateHelper.OpenSession())
        //    {
        //        var msg = session.Query<ForumMessage>().Single(x => x.Id == id);
        //        var topicId = msg.Topic.Id;
        //        if (Context.User.Identity.Name == msg.User.Email || Context.User.IsInRole("admin"))
        //        {
        //            using (ITransaction transaction = session.BeginTransaction())
        //            {
        //                session.Delete(msg);
        //                transaction.Commit();
        //                await Clients.All.SendAsync($"Deleted{topicId}", id);
        //            }
        //        }
        //    }
        //}
        //[Authorize]
        //public async Task EditMessage(int id, string text)
        //{
        //    using (ISession session = nHibernateHelper.OpenSession())
        //    {
        //        var msg = session.Query<ForumMessage>().Single(x => x.Id == id);
        //        var topicId = msg.Topic.Id;
        //        if (Context.User.Identity.Name == msg.User.Email || Context.User.IsInRole("admin"))
        //        {
        //            using (ITransaction transaction = session.BeginTransaction())
        //            {
        //                msg.Text = text;
        //                session.SaveOrUpdate(msg);
        //                transaction.Commit();
        //                await Clients.All.SendAsync($"Changed{topicId}", id, text);
        //            }
        //        }
        //    }
        //}

        ~MessageHub()
        {
            _conn.Close();
            _conn.Dispose();
        }
    }
}
