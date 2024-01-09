using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is Required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 chars")]
        public string Name { get; set; }

        [Required]
        public bool Status { get; set; } = true;
        public DateTime CreateAt { get; set; } = DateTime.Now;
    }
}
