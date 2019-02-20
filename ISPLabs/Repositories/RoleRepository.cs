using ISPLabs.Models;
using ISPLabs.Repositories.Interfaces;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISPLabs.Repositories
{
    public class RoleRepository : IRepository<Role>
    {
        private OracleConnection _conn;

        public RoleRepository(OracleConnection conn) => _conn = conn;

        public void Create(Role item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Role Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Role> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(Role item)
        {
            throw new NotImplementedException();
        }
    }
}
