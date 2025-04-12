using CrudSettingTask.Models;
using System.ComponentModel.DataAnnotations;

namespace CrudSettingTask.ViewModels
{
    public class ProductCreateVM
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public List<IFormFile> Photos { get; set; }
       
        public List<ProductImage> Images { get; set; }
    
        
    }
}
