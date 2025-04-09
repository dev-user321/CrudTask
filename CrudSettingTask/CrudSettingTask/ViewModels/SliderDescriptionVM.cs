using System.ComponentModel.DataAnnotations;

namespace CrudSettingTask.ViewModels
{
    public class SliderDescriptionVM
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public IFormFile Photo { get; set; }
    }
}
