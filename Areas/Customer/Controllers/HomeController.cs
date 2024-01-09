using Dashboard.Data.Repository;
using Dashboard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Dashboard.Areas.Customer.Controllers
{
    [Area("Customer")]
  //  [Authorize(Roles = "Admin,Super_Admin,Client")]

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IUnitOfWork _unitOfWork;
        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var products =_unitOfWork.Product.GetAllProductsWithDetails().ToList();
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult Details(int? id)
        {
            var details = _unitOfWork.Product.GetOne(id);
            if (details == null)
            {
                return NotFound();
            }
            return View(details);
        }

    }
}