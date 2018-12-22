using ISPLabs.Models;
using ISPLabs.Models.API;
using System.Collections.Generic;

namespace ISPLabs.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        HashSet<Category> GetAll();
        HashSet<Category> GetAllWithoutChilds();
        CategoryAPIModel GetById(int id);
        Category GetByIdWithoutChilds(int id);
        CategoryAPIModel Append(CategoryAPIModel category);
        bool Update(CategoryAPIModel category);
        bool Remove(int id);
    }
}
