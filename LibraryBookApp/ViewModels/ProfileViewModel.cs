using System.Collections.Generic;
using LibraryBookApp.Models;

namespace LibraryBookApp.ViewModels
{
    public class ProfileViewModel
    {
        public string Email { get; set; } = string.Empty; // Default value to avoid nullability warning
        public string FirstName { get; set; } = string.Empty; // Default value to avoid nullability warning
        public string LastName { get; set; } = string.Empty; // Default value to avoid nullability warning
        public List<Book> BorrowedBooks { get; set; } = new List<Book>(); // Default value to avoid nullability warning
    }
}