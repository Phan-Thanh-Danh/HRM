using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRM.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PostId { get; set; }
        [ForeignKey("PostId")]
        public virtual Post? Post { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        public int AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public virtual Employee? Author { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
