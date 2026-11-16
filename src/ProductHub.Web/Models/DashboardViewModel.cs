namespace ProductHub.Web.Models
{
    public class DashboardViewModel
    {
        public int TotalProducts { get; set; }
        public int TotalCategories { get; set; }
        public int TotalStockUnits { get; set; }
        public decimal InventoryValue { get; set; }
        public int LowStockCount { get; set; }
        public int OutOfStockCount { get; set; }

        public List<CategoryStat> CategoryStats { get; set; } = new();
        public List<Product> LowStockProducts { get; set; } = new();
        public List<Product> RecentProducts { get; set; } = new();
    }
    public class CategoryStat
    {
        public string Name { get; set; } = "";
        public int Count { get; set; }
    }
}
