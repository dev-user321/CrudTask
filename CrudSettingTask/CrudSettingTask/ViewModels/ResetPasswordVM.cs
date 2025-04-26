using System.ComponentModel.DataAnnotations;

namespace CrudSettingTask.ViewModels
{
    public class ResetPasswordVM
    {
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }  
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password must be same")]
        public string? ConfirmPassword { get; set; }
        public string? UserId { get; set; }
        public string? Token { get; set; }
    }
}
