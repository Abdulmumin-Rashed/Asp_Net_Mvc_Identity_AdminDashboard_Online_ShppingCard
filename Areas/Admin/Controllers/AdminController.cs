using Dashboard.Data.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Super_Admin")]
    public class AdminController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public AdminController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
