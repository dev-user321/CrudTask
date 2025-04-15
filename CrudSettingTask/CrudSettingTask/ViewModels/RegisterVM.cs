using System.ComponentModel.DataAnnotations;

namespace CrudSettingTask.ViewModels
{
    public class RegisterVM
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifrələr uyğun gəlmir.")]
        public string ConfirmPassword { get; set; }
    }
}
