using Xunit;
using Moq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LibraryBookApp.Controllers;
using LibraryBookApp.Models;
using LibraryBookApp.Data; 
using System.Threading.Tasks;

namespace LibraryBookApp.Tests
{
    public class BooksControllerTests
    {
        [Fact]
        public async Task Donate_ValidBook_RedirectsToIndex()
        {
            // Arrange
            var mockContext = new Mock<LibraryContext>(); //  LibraryContext is referenced 
            var mockUserManager = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

            var controller = new BooksController(mockContext.Object, mockUserManager.Object);
            var book = new Book
            {
                Title = "Test Book",
                Author = "Test Author",
                Genre = "Test Genre"
            };

            mockContext.Setup(ctx => ctx.Books.Add(It.IsAny<Book>()));
            mockContext.Setup(ctx => ctx.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var result = await controller.Donate(book);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task Borrow_AvailableBook_RedirectsToIndex()
        {
            // Arrange
            var mockContext = new Mock<LibraryContext>(); //  LibraryContext is referenced 
            var mockUserManager = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

            var controller = new BooksController(mockContext.Object, mockUserManager.Object);
            var book = new Book
            {
                Id = 1,
                Title = "Test Book",
                Author = "Test Author",
                Genre = "Test Genre",
                IsBorrowed = false
            };

            var user = new ApplicationUser { Id = "1", UserName = "testuser" };

            mockContext.Setup(ctx => ctx.Books.FindAsync(It.IsAny<int>())).ReturnsAsync(book);
            mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).ReturnsAsync(user);
            mockContext.Setup(ctx => ctx.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var result = await controller.Borrow(book.Id);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.True(book.IsBorrowed);
            Assert.Equal(user.Id, book.BorrowerId);
        }
    }
}