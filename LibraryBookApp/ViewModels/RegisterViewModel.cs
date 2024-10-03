using System.ComponentModel.DataAnnotations;

namespace LibraryBookApp.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty; // Default value to avoid nullability warning

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty; // Default value to avoid nullability warning

        [Required]
        public string FirstName { get; set; } = string.Empty; // Default value to avoid nullability warning

        [Required]
        public string LastName { get; set; } = string.Empty; // Default value to avoid nullability warning

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty; // Default value to avoid nullability warning
    }
}
