namespace Dashboard.Models
{
    public class ProductSize
    {

        // Primary key
        public int Id { get; set; }

        // Foreign keys
        public int ProductId { get; set; }
        public int SizeId { get; set; }

        // Navigation properties
        public Product Product { get; set; }
        public Size Size { get; set; }
    }
}
