using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRM.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        public int AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public virtual Employee? Author { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
