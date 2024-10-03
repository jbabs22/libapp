using Xunit;
using Moq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using LibraryBookApp.Controllers;
using LibraryBookApp.Models;
using LibraryBookApp.ViewModels;
using System.Threading.Tasks;

namespace LibraryBookApp.Tests
{
    public class AccountControllerTests
    {
        [Fact]
        public async Task Register_UserCreationSuccessful_RedirectsToHomeIndex()
        {
            // Arrange
            var mockUserManager = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

            var mockSignInManager = new Mock<SignInManager<ApplicationUser>>(
                mockUserManager.Object,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
                null, null, null, null);

            var mockLogger = new Mock<ILogger<AccountController>>();
            var controller = new AccountController(mockUserManager.Object, mockSignInManager.Object, mockLogger.Object);

            var model = new RegisterViewModel
            {
                Email = "test@example.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                FirstName = "Test",
                LastName = "User"
            };

            mockUserManager.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                           .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await controller.Register(model);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal("Home", redirectToActionResult.ControllerName);
        }
    }
}