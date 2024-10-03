using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryBookApp.Data;
using LibraryBookApp.Models;
using LibraryBookApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace LibraryBookApp.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly LibraryContext _context;

        public ProfileController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, LibraryContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Retrieve the borrowed books for the user
            var borrowedBooks = await _context.Books
                .Where(b => b.BorrowerId == user.Id && b.IsBorrowed)
                .ToListAsync();

            var model = new ProfileViewModel
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                BorrowedBooks = borrowedBooks // Assign the list of borrowed books

            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                user.FirstName = model.FirstName ?? user.FirstName; // Ensure non-null assignment
                user.LastName = model.LastName ?? user.LastName; // Ensure non-null assignment
                user.Email = model.Email ?? user.Email; // Ensure non-null assignment

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            model.BorrowedBooks = (await _userManager.GetUserAsync(User))?.BorrowedBooks.ToList() ?? new List<Book>();
            return View("Index", model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (user.BorrowedBooks.Any())
            {
                ModelState.AddModelError(string.Empty, "You cannot delete your account while you have borrowed books.");
                return View("Index", new ProfileViewModel
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    BorrowedBooks = user.BorrowedBooks.ToList()
                });
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Logout", "Account");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View("Index", new ProfileViewModel
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                BorrowedBooks = user.BorrowedBooks.ToList()
            });
        }
    }
}