using ISPLabs.Models.API;
using System.Collections.Generic;

namespace ISPLabs.Repositories.Interfaces
{
    public interface IPartitionRepository
    {
        HashSet<PartitionAPIModel> GetAll();
        PartitionAPIModel GetByIdWithoutChilds(int id);
        PartitionAPIModel Append(PartitionAPIModel partition);
        bool Update(PartitionAPIModel partition);
    }
}
