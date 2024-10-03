using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryBookApp.Data;
using LibraryBookApp.Models;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LibraryBookApp.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly LibraryContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BooksController(LibraryContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var books = await _context.Books.Include(b => b.Borrower).Include(b => b.Comments).ThenInclude(c => c.User).ToListAsync();
            return View(books);
        }


        [HttpGet]
        public IActionResult Donate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Donate(Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Books.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        public async Task<IActionResult> Borrow(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null || book.IsBorrowed)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            book.IsBorrowed = true;
            book.BorrowerId = user.Id;
            book.BorrowedDate = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Return(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id && b.BorrowerId == user.Id);
            if (book == null)
            {
                return NotFound();
            }

            book.IsBorrowed = false;
            book.BorrowerId = null;
            book.BorrowedDate = null;

            _context.Books.Update(book);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Comment(int bookId, string text)
        {
            var user = await _userManager.GetUserAsync(User);
            var book = await _context.Books.Include(b => b.Comments).FirstOrDefaultAsync(b => b.Id == bookId);

            if (book == null || user == null)
            {
                return NotFound();
            }

            var comment = new Comment
            {
                Text = text,
                CreatedAt = DateTime.Now,
                UserId = user.Id,
                BookId = book.Id
            };

            book.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var book = await _context.Books.Include(b => b.Comments).ThenInclude(c => c.User).FirstOrDefaultAsync(b => b.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index)); // Redirect to Book Inventory after deletion
        }

    }
}

