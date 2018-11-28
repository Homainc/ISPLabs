using System.ComponentModel.DataAnnotations;

namespace ISPLabs.ViewModels
{
    public class NewTopicModel
    {
        [Required]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "NameRequired")]
        public string Name { get; set; }

        [Required(ErrorMessage = "InitialMessageRequired")]
        public string InitialText { get; set; }
    }
}
