using Dashboard.Data.Repository;
using Dashboard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Super_Admin")]
    public class CategoryController : Controller
    {

        private IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var cats = _unitOfWork.Category.GetAll();
            return View(cats);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        public IActionResult Edit(int? id)
        {
            var cat = _unitOfWork.Category.GetT(x =>x.Id==id);
            return View(cat);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Edit(Category cat)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update2(cat);
                TempData["success"] = "Category Created Successfully";
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Create(Category cat)
        {
            if (ModelState.IsValid)
            {
              _unitOfWork.Category.Add(cat);
              TempData["success"] = "Category Created Successfully";
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View();
        }



       


       
        public IActionResult Delete(int? id)
        {
            var category = _unitOfWork.Category.GetT(x => x.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Delete(category);
            _unitOfWork.Save();
            TempData["success"] = "Category Deleted Successfully";
            return Json(new { success = true });

            
        }

        public IActionResult Details(int? id)
        {
            var category = _unitOfWork.Category.GetT(x => x.Id == id);
            if (category == null)
            {
                return NotFound();
            }
          return View(category);

        }
    }
}
