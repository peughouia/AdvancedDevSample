using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedDevSample.Tests.Domain
{
    public class ProductTests
    {
        [Fact]
        public void CreateProduct_WithNegativePrice_ShouldThrowDomainException()
        {
            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => new Product("Test", -10m, 5));
            Assert.Equal("Le prix doit être strictement positif.", exception.Message);
        }
    }
}
