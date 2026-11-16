using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductHub.Web.Data;
using ProductHub.Web.Models;
using System.Diagnostics;

namespace ProductHub.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private const int LowStockLimit = 5;
    private readonly AppDbContext _db;

    public HomeController(ILogger<HomeController> logger, AppDbContext db)
    {
        _logger = logger;
        _db = db;
    }
    [Authorize]
    public async Task<IActionResult> Index()
    {
        var vm = new DashboardViewModel
        {
            TotalProducts = await _db.Products.CountAsync(),
            TotalCategories = await _db.Categories.CountAsync(),
            TotalStockUnits = await _db.Products.SumAsync(p => (int?)p.stock) ?? 0,
            InventoryValue = await _db.Products.SumAsync(p => (decimal?)(p.stock * p.price)) ?? 0,
            LowStockCount = await _db.Products.CountAsync(p => p.stock > 0 && p.stock <= LowStockLimit),
            OutOfStockCount = await _db.Products.CountAsync(p => p.stock == 0),

            CategoryStats = await _db.Products.GroupBy(p => p.category.name)
                    .Select(g => new CategoryStat { Name = g.Key, Count = g.Count() })
                    .OrderByDescending(x => x.Count)
                    .ToListAsync(),
            LowStockProducts = await _db.Products.Include(p => p.category)
                     .Where(p => p.stock <= LowStockLimit)
                     .OrderBy(p => p.stock)
                    .Take(5)
                    .ToListAsync(),
            RecentProducts = await _db.Products.Include(p => p.category)
                     .OrderByDescending(p => p.createdAtUtc)
                     .Take(5)
                    .ToListAsync()

        };
        return View(vm);
    }
    [Authorize]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
