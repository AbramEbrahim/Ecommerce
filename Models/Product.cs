using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Price { get; set; }

        public string Description { get; set; }

        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]

        public Company Company { get; set; }

        public string ImagePath { get; set; }
        [NotMapped]
        public IFormFile ProductImage { get; set; }

    }
}
