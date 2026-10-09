

using FluentAssertions;
using Products.Core.Queries.GetProductBySearch;
using Products.Core.Validators;

namespace Products.Core.Tests;
public class SearchQueryValidator
{
    [Theory]
    [InlineData("Laptop", 1, 10)]
    [InlineData("Mobile", 2, 20)]
    [InlineData("Samsung", 5, 50)]
    [InlineData("A", 1, 1)]
    [InlineData("Electronics", 10, 100)]
    [InlineData("123", 100, 25)]
    public void Validator_WhenValidRequest_ShouldReturnValid(string searchTerm,int pageNumber, int pageSize)
    {
        // Arrange
        var validator = new GetProductsBySearchValidator();
        var request = new GetProductBySearchQuery(searchTerm, pageNumber, pageSize);
        // Act
        var response = validator.Validate(request);
        // Assert
        response.IsValid.Should().BeTrue();
    }


    [Theory]
    [InlineData("", 1, 10, "SearchTerm", "Please enter search text")]
    [InlineData(" ", 1, 10, "SearchTerm", "Please enter search text")]
    [InlineData("Laptop", 0, 10, "PageNumber", "Page number should be greater than 0")]
    [InlineData("Laptop", -1, 10, "PageNumber", "Page number should be greater than 0")]
    [InlineData("Laptop", 1, 0, "PageSize", "Page size must be between 1 and 100")]
    [InlineData("Laptop", 1, -10, "PageSize", "Page size must be between 1 and 100")]
    [InlineData("Laptop", 1, 101, "PageSize", "Page size must be between 1 and 100")]
    [InlineData("Laptop", 1, 1000, "PageSize", "Page size must be between 1 and 100")]

    public void Validator_WhenValidRequest_ShouldReturnVInvalid(string searchTerm, int pageNumber, int pageSize,string field, string error)
    {
        // Arrange
        var validator = new GetProductsBySearchValidator();
        var request = new GetProductBySearchQuery(searchTerm, pageNumber, pageSize);
        // Act
        var response = validator.Validate(request);
        // Assert
        response.IsValid.Should().BeFalse();
        //response.Errors.Should().Contain(err => err.ErrorMessage == error);
        response.Errors.Should().ContainSingle()
        .Which.Should().Match<FluentValidation.Results.ValidationFailure>(
         err => err.PropertyName == field &&
                err.ErrorMessage == error);

    }
}
