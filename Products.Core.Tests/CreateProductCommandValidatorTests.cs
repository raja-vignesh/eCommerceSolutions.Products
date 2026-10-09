

using FluentAssertions;
using Products.Core.Commands.CreateProduct;
using Products.Core.enums;
using Products.Core.Validators;
using Products.Domain.Entities;

namespace Products.Core.Tests;
public class CreateProductCommandValidatorTests
{
    [Theory]
    [InlineData("Laptop", 999.99, 10, CategoryOptions.Electronics)]
    [InlineData("Mobile", 599.50, 5, CategoryOptions.Electronics)]
    [InlineData("Washing Machine", 450.00, 3, CategoryOptions.HomeAppliances)]
    [InlineData("Office Chair", 150.75, 20, CategoryOptions.Furniture)]
    [InlineData("Dining Table", 799.99, 2, CategoryOptions.Furniture)]
    [InlineData("USB Cable", 9.99, 100, CategoryOptions.Accessories)]
    [InlineData("Mouse", 0.01, 1, CategoryOptions.Accessories)]
    public void Validator_WhenValidRequest_ShouldValidate(string productName,decimal unitPrice,int quantityInStrock, CategoryOptions category)
    {
        // Arrange
        var product = new CreateProductCommand { ProductName = productName, UnitPrice = unitPrice,QuantityInStock = quantityInStrock, Category = category };
        var validator = new CreateProductCommandValidator();
        // Act
        var response = validator.Validate(product);
        // Assert
        response.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("", 100, 10, CategoryOptions.Electronics, "Product Name is required")]
    [InlineData("Laptop", 0, 10, CategoryOptions.Electronics, "price should be greater than 0")]
    [InlineData("Laptop", -100, 10, CategoryOptions.Electronics, "price should be greater than 0")]
    [InlineData("Laptop", 100, 0, CategoryOptions.Electronics, "Qty should be greater than 0")]
    [InlineData("Laptop", 100, -5, CategoryOptions.Electronics, "Qty should be greater than 0")]
    [InlineData("Laptop", 100, 10, (CategoryOptions)10, "Invalid prod category")]
    public void Validator_WhenInvalidRequest_ShouldReturnValidationError(string productName, decimal unitPrice, int quantityInStrock, CategoryOptions category, string expectedError)
    {
        // Arrange 
        var product = new CreateProductCommand() { ProductName = productName,UnitPrice = unitPrice, Category = category, QuantityInStock = quantityInStrock};
        var validator = new CreateProductCommandValidator();
        // Act
        var result =  validator.Validate(product);
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(error => error.ErrorMessage == expectedError);
    }
}
