
using System.ComponentModel.DataAnnotations.Schema;

namespace CrudSettingTask.Models
{
    public class Slider : BaseEntity
    {
        public string Image { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
