using System.ComponentModel.DataAnnotations;

namespace CrudSettingTask.ViewModels
{
    public class SliderCreateVM
    {
        [Required]
        public List<IFormFile> Photos { get; set; }
    }
}
