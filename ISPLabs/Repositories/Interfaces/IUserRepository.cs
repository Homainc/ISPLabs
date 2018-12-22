using ISPLabs.Models;
using ISPLabs.Models.API;
using System.Collections.Generic;

namespace ISPLabs.Repositories.Interfaces
{
    public interface IUserRepository
    {
        UserAPIModel GetById(int id);
        User GetByEmail(string email);
        HashSet<UserAPIModel> GetAll();
        User Append(User user);
        bool Update(User user);
        User Login(string email, string password);
        int GetMessagesCount(int id);
    }
}
