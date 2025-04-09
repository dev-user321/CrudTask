using System.ComponentModel.DataAnnotations;

namespace CrudSettingTask.Models
{
    public class SliderDescription : BaseEntity
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Image {  get; set; }
    }
}
