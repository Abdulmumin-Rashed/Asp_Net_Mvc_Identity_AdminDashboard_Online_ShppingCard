using Dashboard.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dashboard.Data.ModelViews
{
    public class BuyViewModel
    {
        public List<Cart>? CartItems { get; set; }
        public string BillingAddress { get; set; }
        public string PaymentMethod { get; set; }

    }

}
