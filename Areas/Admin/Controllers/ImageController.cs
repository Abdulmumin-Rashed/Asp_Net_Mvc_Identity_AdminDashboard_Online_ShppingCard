using Dashboard.Data.Repository;
using Dashboard.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Dashboard.Models;
using Microsoft.AspNetCore.Authorization;

namespace Dashboard.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Super_Admin")]
    public class ImageController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private ApplicationDbContext _context;
        public ImageController(IUnitOfWork unitOfWork, ApplicationDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;

        }
       public async Task<IActionResult> Index()
        {
            var images = await _context.Images.Include(p => p.Product).ToListAsync();
            return View(images);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Images.FirstOrDefaultAsync(m => m.Id == id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        // GET: Images/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = GetProduct();
            return View();
        }
        private SelectList GetProduct()
        {
            return new SelectList(_context.Products, "Id", "Name");
        }
        // POST: Images/Create
        // GET: Images/Create


        // POST: Images/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Image image, IFormFile file)
        {
            if (image.ProductId == 0)
            {
                TempData["ProductId"] = "You Must Select a Product!";
            }

            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var extension = Path.GetExtension(fileName);

                    // Generate a unique ID for the image (you can adjust this based on your needs)
                    var imageId = Guid.NewGuid().ToString("N"); // Using a GUID for uniqueness

                    // Concatenate the ID to the end of the filename
                    var newFileName = $"{Path.GetFileNameWithoutExtension(fileName)}_{imageId}{extension}";

                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Product_Images", newFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    image.FileName = "/Product_Images/" + newFileName; // Save the relative path in the database
                    _context.Add(image);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(image);
        }


        //Edit

        public IActionResult Edit(int? id)
        {
           var image = _context.Images.FirstOrDefault(x => x.Id == id);
            ViewData["ProductId"] = GetProduct();
            return View(image);
        }
         [HttpPost]
        [ValidateAntiForgeryToken]


        public async Task<IActionResult> Edit(Image image, IFormFile file)
        {

            if (image.ProductId == 0)
            {
                TempData["ProductId"] = "You Must Select a Product!?";
            }

            if (ModelState.IsValid)
            {
                if (file != null)
                {

                    var fileName = Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Product_Images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    image.FileName = "/Product_Images/" + fileName; // Save the relative path in the database
                    _context.Update(image);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }


            }

            return View(image);
        }
        public IActionResult Delete(int? id)
        {
            var image = _context.Images.FirstOrDefault(x => x.Id == id);
            if (image == null)
            {
                return NotFound();
            }

            
                _context.Images.Remove(image);
                _context.SaveChanges();
            return Json(new { success = true });
           
        }

        //public IActionResult Details(int? id)
        //{
        //    var image =_context.Images.FirstOrDefault(x => x.Id == id);
        //    if (image == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(image);
        //}
    }
}
