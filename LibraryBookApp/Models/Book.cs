using System;
using System.Collections.Generic;

namespace LibraryBookApp.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public bool IsBorrowed { get; set; }
        public DateTime? BorrowedDate { get; set; } // Nullable DateTime
        public string BorrowerId { get; set; } // ID of the user who borrowed the book
        public ApplicationUser Borrower { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
