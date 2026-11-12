using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductHub.Web.Data;
using ProductHub.Web.Models;
using System.Drawing.Printing;

namespace ProductHub.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly AppDbContext _db;

        public CategoriesController(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 5;
            int totalCategory = await _db.Categories.CountAsync();
            int totalPage = (int)Math.Ceiling(totalCategory / (double)pageSize);

            page = totalPage > 0 ? Math.Min(page, totalPage) : 1;

            var category_list = await _db.Categories
                .OrderBy(c => c.id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.FormType = "Create";
            ViewBag.category = null;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = totalPage;
            return View(category_list);
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("name")]Category category)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Category name is required (max 100 characters).";
                return RedirectToAction(nameof(Index));
            }
            var cname = category.name.Trim();
            if (await _db.Categories.AnyAsync(c => c.name == cname))
            {
                TempData["Error"] = "Category already Exists.";
                return RedirectToAction(nameof(Index));
            }
            _db.Categories.Add(category);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Category added.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id, int page = 1)
        {
            int pageSize = 5;
            var category = await _db.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            int totalCategory = await _db.Categories.CountAsync();
            int totalPage = (int)Math.Ceiling(totalCategory / (double)pageSize);
            var category_list = await _db.Categories
                .OrderBy(c => c.id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            ViewBag.FormType = "Edit";
            ViewBag.category = category;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = totalPage;

            return View("Index", category_list);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category category, int page = 1)
        {
            var iscategoryExist = await _db.Categories.FindAsync(category.id);
            if (iscategoryExist == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Category name is required.";
                return RedirectToAction(nameof(Index));
            }
            iscategoryExist.name = category.name.Trim();
            await _db.SaveChangesAsync();

            TempData["Success"] = "Category updated successfully.";
            return RedirectToAction(nameof(Index), new { page });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, int page = 1)
        {
            var category =  await _db.Categories.FindAsync(id);
            if(category == null) return NotFound();

            if (await _db.Products.AnyAsync(p => p.categoryId == id))
            {
                TempData["Error"] = "Cannot delete a category that still has products.";
                return RedirectToAction(nameof(Index));
            }
            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Category deleted.";
            return RedirectToAction(nameof(Index), new { page });
        }
    }
}
