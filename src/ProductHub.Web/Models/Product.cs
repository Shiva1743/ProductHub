using System.ComponentModel.DataAnnotations;

namespace ProductHub.Web.Models
{
    public class Product
    {
        public int id { get; set; } // PK

        [Display(Name = "Product Name")]
        public string name { get; set; } = string.Empty;

        [StringLength(100, MinimumLength = 3,ErrorMessage = "Product Name must be between 3 and 100 characters.")]
        public string? description { get; set; }

        [Display(Name = "Price")]
        [Required(ErrorMessage ="Price Value is required.")]
        [Range(0, 999, ErrorMessage = "price in range of 1 to 999.")]
        public decimal? price { get; set; }

        [Display(Name = "Stock Quantity")]
        [Required(ErrorMessage = "Stock Value is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative.")]
        public int? stock { get; set; }

        [Display(Name = "Category")]
        [Required(ErrorMessage = "Category Value is required.")]
        public int? categoryId { get; set; } // FK

        public Category? category { get; set; } // Enable to access category object

        [DataType(DataType.DateTime)]
        public DateTime createdAtUtc { get; set; } = DateTime.UtcNow;

        [Display(Name = "Last Updated")]
        public DateTime lastUpdatedAtUtc { get; set; }

    }
}
