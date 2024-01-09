using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dashboard.Models
{
    public class Image
    {
        public int Id { get; set; }
     
        public string? FileName { get; set; }
      
        [ForeignKey("ProductId")]
        public int ProductId { get; set; }
        public  Product? Product { get; set; }
    }
}
