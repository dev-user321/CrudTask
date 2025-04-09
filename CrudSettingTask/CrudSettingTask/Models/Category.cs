using System.ComponentModel.DataAnnotations;

namespace CrudSettingTask.Models
{
    public class Category : BaseEntity
    {
        [Required]
        public string CategoryName { get; set; }
    }
}
