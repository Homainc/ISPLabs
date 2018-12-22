using ISPLabs.Models;
using ISPLabs.Models.API;
using ISPLabs.ViewModels;
using System.Collections.Generic;

namespace ISPLabs.Repositories.Interfaces
{
    public interface ITopicRepository
    {
        HashSet<TopicAPIModel> GetAll();
        TopicAPIModel GetById(int id);
        Topic GetByIdWithCategory(int id);
        Topic GetByIdWithUser(int id);
        Topic Append(Topic topic);
        bool Update(TopicAPIModel topic);
        bool Remove(int id);
    }
}
