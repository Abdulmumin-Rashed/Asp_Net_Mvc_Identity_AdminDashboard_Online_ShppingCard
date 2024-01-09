using Dashboard.Data;
using Dashboard.Data.Repository;
using Dashboard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Super_Admin")]
    public class ProductController : Controller
    {

        private IUnitOfWork _unitOfWork;
        private ApplicationDbContext _context;
        public ProductController(IUnitOfWork unitOfWork, ApplicationDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;

        }
        private SelectList GetCategories()
        {
            return new SelectList((System.Collections.IEnumerable)_unitOfWork.Category, "Id", "Name");
        }
        #region APICALL
        public IActionResult AllProducts()
        {
            var products = _unitOfWork.Product.GetAll(includeProperities: "Category");
            return Json(new { data = products });
        }
        #endregion
        public IActionResult Index()
        {
            IEnumerable<Product> products = _unitOfWork.Product.GetAll(includeProperities: "Category");
            return View(products);
            //Product vm = new Product();
            //var p= _unitOfWork.Product.GetAll();
            //return View(p);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var allCategories = _unitOfWork.Category.GetAll(); // Retrieve all categories

            var trueCategories = allCategories.Where(category => category.Status == true).ToList();

            ViewData["CategoryId"] = new SelectList(trueCategories, "Id", "Name");


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (product.CategoryId == 0)
            {
                TempData["CategoryId"] = "You Must Select a category!?";
            }

            // product.Category = _context.Categories.Find(product.CategoryId);


            if (ModelState.IsValid)
            {

                _unitOfWork.Product.Add2(product);
                TempData["success"] = "Product has Created Successfully";
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View();
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


        public IActionResult Edit(int? id)
        {
            var product = _unitOfWork.Product.GetT(x => x.Id == id);
            var allCategories = _unitOfWork.Category.GetAll(); // Retrieve all categories

            var trueCategories = allCategories.Where(category => category.Status == true).ToList();

            ViewData["CategoryId"] = new SelectList(trueCategories, "Id", "Name");


            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {

            // product.Category = _context.Categories.Find(product.CategoryId);


            if (ModelState.IsValid)
            {

                _unitOfWork.Product.Update(product);
                TempData["success"] = "Product has Created Successfully";
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            var product = _unitOfWork.Product.GetT(x => x.Id == id);
            if (product == null)
            {
                return View("Error");
            }
            _unitOfWork.Product.Delete(product);
            _unitOfWork.Save();
            TempData["success"] = "Product Deleted Successfully";
            return Json(new { success = true });
        }
    }
}
