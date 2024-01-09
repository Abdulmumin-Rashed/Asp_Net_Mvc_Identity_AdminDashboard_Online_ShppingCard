using Dashboard.Data.ModelViews;
using Dashboard.Data.Repository;
using Dashboard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dashboard.Areas.Customer.Controllers
{
        [Area("Customer")]
        public class OrderController : Controller
        {
            private readonly IOrderRepository _orderRepository;
            private readonly UserManager<AppUser> _userManager;
            private readonly ICartRepository _cartRepository;
            public OrderController(IOrderRepository orderRepository, UserManager<AppUser> userManager, ICartRepository cartRepository)
            {
                _orderRepository = orderRepository;
                _userManager = userManager;
                _cartRepository = cartRepository;
            }
        
       




       
            public async Task<IActionResult> Buy()
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                {
                    // Handle unauthenticated or invalid user
                    return RedirectToAction("Login", "Account");
                }

                var cartItems = await _cartRepository.GetCartItemsAsync(user.Id);

                return View(cartItems);
            }

     


            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Buy2(Order order)
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                {
                    // Handle unauthenticated or invalid user
                    return RedirectToAction("Login", "Account");
                }

                var cartItems = await _cartRepository.GetCartItemsAsync(user.Id);

           

          
                order.AppUserId = user.Id;
                order.OrderDate = DateTime.Now;
                // Calculate total amount from cart items
                order.TotalAmount = cartItems.Sum(item => item.Total);
                 //  order.PaymentMethod = "Credit Card"; // You might want to adjust this based on user input
                    //BillingAddress = user.Address // You might want to adjust this based on user input
            

                foreach (var cartItem in cartItems)
                {
                    var orderDetail = new OrderDetail
                    {
                        ProductId = cartItem.ProductId,
                        Quantity = cartItem.Quantity,
                        PricePerItem = (decimal)cartItem.Product.Price,
                        Total = cartItem.Quantity * cartItem.Product.Price
                        // You might want to adjust this based on user input
                    };

                    order.OrderDetails.Add(orderDetail);
                }

                await _orderRepository.AddAsync(order);

                // Clear the cart after creating the order
                await _cartRepository.ClearCartAsync(user.Id);

                return RedirectToAction("Index", "Home"); // Redirect to a success page or somewhere else
            }

            public async Task<IActionResult> Index()
            {
                var orders = await _orderRepository.GetOrdersWithDetailsAsync();
                return View(orders);
            }

       
       
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
                await _orderRepository.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
        }
}
