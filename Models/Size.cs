namespace Dashboard.Models
{
    public class Size
    {
        public int Id { get; set; }
        public string SizeValue { get; set; }

        // Many-to-many relationship with Product through ProductSize
        public ICollection<ProductSize> ProductSizes { get; set; }
    }
}

