using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is Required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 chars")]
        public string? Name { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
       
        public  Category? Category { get; set; }
       
        
        
        public string Description { get; set; }
        [Required(ErrorMessage = "Description is Required")]

        
        public float Price { get; set; }
        [Required(ErrorMessage = "Price is Required")]
        public DateTime CreateAt { get; set; } = DateTime.Now;
        //public ICollection<ProductColor> ?ProductColors { get; set; }
        public ICollection<Image>? Images { get; set; }
    }
}
