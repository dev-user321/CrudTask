using System.ComponentModel.DataAnnotations;

namespace CrudSettingTask.ViewModels
{
    public class LoginVM
    {
        [Required]
        public string UserNameOrEmail { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }    
    }
}
