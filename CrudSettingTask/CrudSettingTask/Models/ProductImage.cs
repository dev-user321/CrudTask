using System.ComponentModel.DataAnnotations;

namespace CrudSettingTask.Models
{
    public class ProductImage : BaseEntity
    {
        [Required]
        public string Image { get; set; }
        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
