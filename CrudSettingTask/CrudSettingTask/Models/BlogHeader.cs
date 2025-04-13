using System.ComponentModel.DataAnnotations;

namespace CrudSettingTask.Models
{
    public class BlogHeader : BaseEntity
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; } 
    }
}
