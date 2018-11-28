using System.ComponentModel.DataAnnotations;

namespace ISPLabs.ViewModels
{
    public class SendForumMessageModel
    {
        public int Id { get; set; }
        public int TopicId { get; set; }
        [Required(ErrorMessage = "TextRequired")]
        public string Text { get; set; }
    }
}
