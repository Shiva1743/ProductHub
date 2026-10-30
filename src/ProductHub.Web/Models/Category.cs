using System.ComponentModel.DataAnnotations;

namespace ProductHub.Web.Models
{
    public class Category
    {
        public int id { get; set; }

        //[Required(ErrorMessage = "Category name is required.")]
        public string name { get; set; }

        //Relatioonship one to many
        public List<Product> Products { get; set; } = new();
    }
}
