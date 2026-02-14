using AdvancedDevSample.Application.Exceptions;
using AdvancedDevSample.Application.Services;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedDevSample.Tests.Application
{
    public class ProductServiceTests
    {
        [Fact]
        public async Task GetProductByIdAsync_WhenNotFound_ShouldThrowNotFoundException()
        {
            // Arrange
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Product?)null);
            var service = new ProductService(mockRepo.Object);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => service.GetProductByIdAsync(Guid.NewGuid()));
        }
    }
}
