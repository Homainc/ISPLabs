using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ISPLabs.ViewModels
{
    public class PartitionModel
    {
        public int Id { get; set; }
        public int CatId { get; set; }
        [Required(ErrorMessage = "NameRequired")]
        public string Name { get; set; }
        public int Categories { get; set; }
    }
}
