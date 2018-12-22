using ISPLabs.Models;
using ISPLabs.Models.API;
using System.Collections.Generic;

namespace ISPLabs.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Role GetByName(string name);
        HashSet<RoleAPIModel> GetAll();
    }
}
