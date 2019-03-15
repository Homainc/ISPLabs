using System.Collections.Generic;

namespace ISPLabs.Models
{
    public class Partition
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Category> Categories { get; set; }

        public Partition()
        {
            Categories = new List<Category>();
        }

        public Partition(int id, string name)
        {
            this.Id = id;
            this.Name = name;
            Categories = new List<Category>();
        }
    }
}
