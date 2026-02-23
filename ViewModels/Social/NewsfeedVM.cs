using HRM.Models;
using System.ComponentModel.DataAnnotations;

namespace HRM.ViewModels.Social
{
    public class PostVM
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nội dung")]
        public string Content { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        public string? AuthorName { get; set; }
        public string? AuthorAvatar { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<CommentVM> Comments { get; set; } = new List<CommentVM>();
    }

    public class CommentVM
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string Content { get; set; } = string.Empty;
        public string? AuthorName { get; set; }
        public string? AuthorAvatar { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
