using LibraryApp.Controllers;
using LibraryApp.Models;
using LibraryApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LibraryApp.Tests
{
    public class BooksControllerTests
    {
        [Fact]
        public void Create_Post_ValidBook_CallsAddAndRedirects()
        {
            // Arrange – create a fake (mock) repository
            var mockRepo = new Mock<IBookRepository>();
            mockRepo.Setup(r => r.Add(It.IsAny<Book>())).Verifiable();

            var controller = new BooksController(mockRepo.Object);
            var book = new Book { Title = "Test Book", Author = "Test", Year = 2025 };

            // Act – call the Create method
            var result = controller.Create(book);

            // Assert – check that Add was called once and we redirect to Index
            mockRepo.Verify(r => r.Add(It.IsAny<Book>()), Times.Once());
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }
    }
}