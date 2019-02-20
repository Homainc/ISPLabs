using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISPLabs.Repositories.Interfaces
{
    interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        void Update(T item);
        void Create(T item);
        void Delete(int id);
    }
}
