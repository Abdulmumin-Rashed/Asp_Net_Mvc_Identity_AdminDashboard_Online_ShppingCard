using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
{
    public class Order
    {
        public Order()
        {
            // Initialize OrderDetails as an empty list
            OrderDetails = new List<OrderDetail>();
        }
        [Key]
        public int Id { get; set; }

        public string? AppUserId { get; set; }
        [ForeignKey("AppUserId")]
        public AppUser? AppUser { get; set; }

        public DateTime OrderDate { get; set; }=DateTime.Now;

        public decimal TotalAmount { get; set; }

        public string? PaymentMethod { get; set; }

       


        public string ?BillingAddress { get; set; }

        public List<OrderDetail>? OrderDetails { get; set; }
    }

}
