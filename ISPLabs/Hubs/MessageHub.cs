using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using ISPLabs.Services;
using ISPLabs.Models;
using ISPLabs.Manager;

namespace ISPLabs.Hubs
{
    public class MessageHub : Hub
    {
        private ForumMessageManager _messages;
        private UserManager _users;

        public MessageHub(OracleSession session)
        {
            _messages = new ForumMessageManager(session.Connection);
            _users = new UserManager(session.Connection);
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

        [Authorize]
        public async Task DeleteMessage(int id, int topicId)
        {
            if (await _messages.DeleteAsync(id))
                await Clients.All.SendAsync($"Deleted{topicId}", id);
        }

        [Authorize]
        public async Task EditMessage(int id, string text, int topicId)
        {
            if (await _messages.UpdateAsync(new ForumMessage { Id = id, Text = text }))
                await Clients.All.SendAsync($"Changed{topicId}", id, text);
        }
    }
}
