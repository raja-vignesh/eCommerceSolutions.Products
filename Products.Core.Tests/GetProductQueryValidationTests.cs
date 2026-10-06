
using FluentAssertions;
using Products.Core.Queries.GetProducts;
using Products.Core.Validators;

namespace Products.Core.Tests;
public class GetProductQueryValidationTests
{
    [Theory]
    [InlineData(1,10)]
    [InlineData(10, 10)]
    [InlineData(1, 100)]
    [InlineData(10, 50)]
    public void Validate_WhenValidRequest_ShouldValidate(int pageNumber, int pageSize)
    {
        // Arrange
        var productQueryVaidator = new GetProductsQueryValidator();
        var getProductQuery = new GetProductsQuery(pageNumber: pageNumber, pageSize: pageSize);

        // Act
        var result = productQueryVaidator.Validate(getProductQuery);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(-1, 10)]
    [InlineData(0, 0)]
    [InlineData(1000, 100000)]
    [InlineData(10, -5)]
    public void Validate_WhenInValidRequest_ShouldFailValidation(int pageNumber, int pageSize)
    {
        // Arrange
        var productQueryVaidator = new GetProductsQueryValidator();
        var getProductQuery = new GetProductsQuery(pageNumber: pageNumber, pageSize: pageSize);

        // Act
        var result = productQueryVaidator.Validate(getProductQuery);

        // Assert
        result.IsValid.Should().BeFalse();
    }

}
