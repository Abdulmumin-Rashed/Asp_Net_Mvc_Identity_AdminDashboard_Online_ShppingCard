using Dashboard.Data.ModelViews;
using Dashboard.Data.Repository;
using Dashboard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dashboard.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICartRepository _cartRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IOrderRepository _orderRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CartController(ICartRepository cartRepository, UserManager<AppUser> userManager, IUnitOfWork unitOfWork, IOrderRepository orderRepository, IHttpContextAccessor httpContextAccessor)
        {
            _cartRepository = cartRepository;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _orderRepository = orderRepository;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<IActionResult> Decrease(int cartItemId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }



            var cartItem = await _unitOfWork.Cart.GetCartByIdAsync(cartItemId);

            if ((cartItem == null) || (cartItem.AppUserId != user.Id))
            {
                return RedirectToAction("Cart");
            }
            if (cartItem.Quantity <= 1)
            {
                var deleted = await _cartRepository.DeleteCartItemAsync(cartItemId);

                if (deleted)
                {
                    return RedirectToAction("Cart");
                }


            }

            cartItem.Quantity -= 1;
            cartItem.Total = (decimal)(cartItem.Product.Price * cartItem.Quantity);
            await _unitOfWork.Cart.UpdateQuantityAsync(cartItemId, cartItem.Quantity);
            return RedirectToAction("Cart");


        }



        public async Task<IActionResult> Increase(int cartItemId)


        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                // Handle unauthenticated or invalid user
                return RedirectToAction("Login", "Account");
            }

            var cartItem = await _cartRepository.GetCartByIdAsync(cartItemId);

            if (cartItem == null || cartItem.AppUserId != user.Id)
            {
                // Handle invalid cart item or mismatched user
                return RedirectToAction("Cart");
            }

            cartItem.Quantity += 1; // Increase quantity by one
            cartItem.Total = (decimal)(cartItem.Product.Price * cartItem.Quantity);

            await _cartRepository.UpdateQuantityAsync(cartItem.Id, cartItem.Quantity); // Update quantity and total

            return RedirectToAction("Cart");
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                // Handle unauthenticated or invalid user
                return RedirectToAction("Login", "Account");
            }

            var added = await _cartRepository.AddToCartAsync(user.Id, productId, quantity);

            if (added)
            {
                return RedirectToAction("Cart");
            }
            else
            {
                // Handle error
                return RedirectToAction("Index", "Home", new { area = "Customer" });
            }
        }
        [HttpPost]

        public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantityChange)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                // Handle unauthenticated or invalid user
                return RedirectToAction("Login", "Account");
            }

            var newQuantity = quantityChange;

            var updated = await _cartRepository.UpdateQuantityAsync(cartItemId, newQuantity);

            if (updated)
            {
                return RedirectToAction("Cart");
            }
            else
            {
                // Handle error
                return RedirectToAction("Index", "Home");
            }
        }




        [HttpPost]
        public async Task<IActionResult> DeleteCartItem(int cartItemId)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                // Handle unauthenticated or invalid user
                return RedirectToAction("Login", "Account");
            }

            var deleted = await _cartRepository.DeleteCartItemAsync(cartItemId);

            if (deleted)
            {
                return RedirectToAction("Cart");
            }
            else
            {
                // Handle error
                return RedirectToAction("Cart");
            }
        }

        public async Task<IActionResult> Cart()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                // Handle unauthenticated or invalid user
                return RedirectToAction("Login", "Identity");
            }

            var cartItems = await _cartRepository.GetCartItemsAsync(user.Id);

            return View(cartItems);
        }
        [HttpGet]
        public IActionResult Buy()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var cartItems = _cartRepository.GetCartItemsAsync(userId).Result;

            // Create a BuyViewModel and populate it with cart items
            var viewModel = new BuyViewModel
            {
                CartItems = cartItems,

            };

            // Return the "Buy" view with the BuyViewModel
            return View("Buy", viewModel);
        }

        public string GetUserIdCart()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return userId;
        }

        //[HttpPost]
        //public IActionResult Buy(BuyViewModel model)
        //{
        //    model.CartItems= _cartRepository.GetCartItemsAsync(GetUserIdCart()).Result;

        //    if (ModelState.IsValid)
        //    {
        //        // Create and save the order here, including the BillingAddress and PaymentMethod
        //        // You can use model.CartItems, model.BillingAddress, and model.PaymentMethod to create and save the order

        //        // Assuming you have an order repository, create an Order object
        //        if (model.CartItems != null && model.CartItems.Any())
        //        {
        //            var order = new Order
        //            {
        //                AppUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
        //                OrderDate = DateTime.Now,
        //                TotalAmount = (decimal)model.CartItems.Sum(item => item.Quantity * item.Product.Price),
        //                PaymentMethod = model.PaymentMethod,
        //                BillingAddress = model.BillingAddress,
        //                // Set other order properties as needed
        //            };

        //            // Rest of your code for creating and saving the order
        //            var orderDetails = model.CartItems.Select(cartItem => new OrderDetail
        //            {
        //                ProductId = cartItem.Product.Id,
        //                Quantity = cartItem.Quantity,
        //                PricePerItem = (decimal)cartItem.Product.Price,
        //                // Set other order detail properties as needed
        //            }).ToList();

        //            // Save the order and order details using your repository methods
        //            _orderRepository.CreateOrderAsync(order, orderDetails);

        //            // Clear the user's cart (assuming you have a cart repository)
        //            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //            _cartRepository.ClearCartAsync(userId);

        //            // Redirect to a thank you page or order summary page
        //            return RedirectToAction("Cart");
        //        }

        //        // Assuming you have an order detail repository and access to the product details

        //    }

        //    // If ModelState is not valid, return to the Buy view with errors
        //    return View("Buy", model);
        //}






        //[HttpPost]
        //public IActionResult SubmitOrder(BuyViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Create and save the order here, including the BillingAddress and PaymentMethod
        //        // You can use model.CartItems, model.BillingAddress, and model.PaymentMethod to create and save the order

        //        // Assuming you have an order repository, create an Order object
        //        var order = new Order
        //        {
        //            AppUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
        //            OrderDate = DateTime.Now,
        //            TotalAmount = (decimal)model.CartItems.Sum(item => item.Quantity * item.Product.Price),
        //            PaymentMethod = model.PaymentMethod,
        //            BillingAddress = model.BillingAddress,
        //            // Set other order properties as needed
        //        };

        //        // Assuming you have an order detail repository and access to the product details
        //        var orderDetails = model.CartItems.Select(cartItem => new OrderDetail
        //        {
        //            ProductId = cartItem.Product.Id,
        //            Quantity = cartItem.Quantity,
        //            PricePerItem = (decimal)cartItem.Product.Price,
        //            // Set other order detail properties as needed
        //        }).ToList();

        //        // Save the order and order details using your repository methods
        //        _orderRepository.CreateOrderAsync(order, orderDetails);

        //        // Clear the user's cart (assuming you have a cart repository)
        //        var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //        _cartRepository.ClearCartAsync(userId);

        //        // Redirect to a thank you page or order summary page
        //        return RedirectToAction("OrderConfirmation");
        //    }

        //    // If ModelState is not valid, return to the Buy view with errors
        //    return View("Buy", model);
        //}


        private decimal CalculateTotalAmount(List<Cart> cartItems)
        {
            decimal total = 0;

            foreach (var cartItem in cartItems)
            {

                total += (decimal)(cartItem.Quantity * cartItem.Product.Price);
            }



           

            return total;

        }

        /////////////////////////////////////Create Orders///////////////
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Buy(Order order)
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

            return RedirectToAction("Cart", "Cart"); // Redirect to a success page or somewhere else
        }

    }
}
