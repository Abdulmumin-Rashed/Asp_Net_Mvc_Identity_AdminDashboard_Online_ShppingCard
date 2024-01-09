using Dashboard.Data.Repository;
using Dashboard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.Areas.Admin.Controllers
{
    [Area("Admin")]
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

        public IActionResult Index()
        {
            var orders = _orderRepository.GetOrdersWithDetailsAsync().Result;

            return View(orders);
        }

        public IActionResult Details(int id)
        {
            var order = _orderRepository.GetOrderWithDetailsAsync(id).Result;

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

    }
}
