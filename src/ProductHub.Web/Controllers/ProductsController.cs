using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProductHub.Web.Data;
using ProductHub.Web.Models;

namespace ProductHub.Web.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly AppDbContext _db;
        public ProductsController(AppDbContext db)
        {
            _db = db;
        }

        public void LoadCategories(int? selectedId = null)
        {
            ViewBag.Categories = new SelectList(
                    _db.Categories.OrderBy(p => p.name),
                    "id",
                    "name",
                    selectedId
                );
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 5;
            int totalProd = await _db.Products.CountAsync();
            int totalPage = (int)Math.Ceiling(totalProd / (double)pageSize);

            page = totalPage > 1 ? Math.Min(page,totalPage) : 1;

            ViewBag.CurrentPage = page;
            ViewBag.totalPage = totalPage;

            var product_list = await _db.Products
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Include(p => p.category)
                    .OrderBy(p=> p.name)
                    .ToListAsync();
            return View(product_list);
        }

        public IActionResult Create()
        {
            LoadCategories();
            return View(new Product());
        }
        [HttpPost]
        public async Task<IActionResult> Create([Bind("name,description,price,stock,categoryId")] Product product)
        {
            /*
            foreach (var key in ModelState.Keys.ToList())
            {
                var entry = ModelState[key];

                if (entry?.Errors.Count > 0 &&
                    entry.Errors.Any(e => e.ErrorMessage == "The value '' is invalid."))
                {
                    ModelState[key].Errors.Clear();

                    var message = key switch
                    {
                        "price" => "Price Value is required.",
                        "stock" => "Stock Value is required.",
                        "categoryId" => "Category Value is required.",
                        _ => "Invalid value."
                    };

                    ModelState.AddModelError(key, message);
                }
            }
            */
            if (!ModelState.IsValid)
            {
                LoadCategories(product.categoryId);
                return View(product);
            }

            product.createdAtUtc = DateTime.UtcNow;
            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Product created.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, int page = 1)
        {
            var prod = await _db.Products.FindAsync(id);
            if (prod != null)
            {
                _db.Products.Remove(prod);
                await _db.SaveChangesAsync();
                TempData["Success"] = "Product deleted.";
            }
            return RedirectToAction(nameof(Index),new {page});
        }


    }
}
