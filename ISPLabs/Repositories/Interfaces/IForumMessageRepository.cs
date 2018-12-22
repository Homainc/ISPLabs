using ISPLabs.Models;
using ISPLabs.Models.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISPLabs.Repositories.Interfaces
{
    public interface IForumMessageRepository
    {
        ISet<MessageAPIModel> GetAllInTopic(int topicId);
        ForumMessage Append(ForumMessage message);
        bool Update(MessageAPIModel message);
        bool Remove(int id);
    }
}
