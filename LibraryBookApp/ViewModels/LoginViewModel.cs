using System.ComponentModel.DataAnnotations;

namespace LibraryBookApp.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty; // Default value to avoid nullability warning

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty; // Default value to avoid nullability warning

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
