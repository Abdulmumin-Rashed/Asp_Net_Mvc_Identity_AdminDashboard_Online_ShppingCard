using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

     
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

       
        public string AppUserId { get; set; }
        [ForeignKey("AppUserId")]
        public AppUser? AppUser { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Total is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total must be greater than 0")]
        public decimal Total { get; set; }
        [NotMapped]
        public ICollection<Image>? Images { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

}
