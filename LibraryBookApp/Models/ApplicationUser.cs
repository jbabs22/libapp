using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace LibraryBookApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        // ICollection is generally preferred for navigation properties in EF Core
        public ICollection<Book> BorrowedBooks { get; set; } = new List<Book>();
    }
}

