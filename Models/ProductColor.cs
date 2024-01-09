namespace Dashboard.Models
{
    public class ProductColor
    { 
        // Primary key
        public int Id { get; set; }

        // Foreign keys
        public int ProductId { get; set; }
        public int ColorId { get; set; }

        // Navigation properties
        public Product? Product { get; set; }
        public Color? Color { get; set; }
    }
}
