using System.ComponentModel.DataAnnotations;

namespace CrudSettingTask.Models
{
    public class Product : BaseEntity
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public List<ProductImage> Images { get; set; }
        public Category Category { get; set; }
        [Required]
        public DateTime CreatedTime { get; set; }

    }
}
