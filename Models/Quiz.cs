using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRM.Models
{
    public class Quiz
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        public int PassScore { get; set; } = 5; // e.g. 5/10 to pass

        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}
