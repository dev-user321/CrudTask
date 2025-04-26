using System.ComponentModel.DataAnnotations;

namespace CrudSettingTask.ViewModels
{
    public class ForgetPasswordVM
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }   
    }
}
