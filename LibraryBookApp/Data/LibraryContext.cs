using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LibraryBookApp.Models;

namespace LibraryBookApp.Data
{
    public class LibraryContext : IdentityDbContext<ApplicationUser>
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Additional configurations if necessary

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Borrower)
                .WithMany()
                .HasForeignKey(b => b.BorrowerId)
                .OnDelete(DeleteBehavior.SetNull); // Ensures BorrowerId can be null
        }
    }
}