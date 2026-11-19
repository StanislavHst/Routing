using System.ComponentModel.DataAnnotations;

namespace ProductsWithRouting.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
