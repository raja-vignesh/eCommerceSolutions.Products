

using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using FluentAssertions;
using Moq;
using Products.Core.Commands.CreateProduct;
using Products.Core.Dtos;
using Products.Domain.Entities;
using Products.Domain.RepositoryContracts;
using System.Threading.Tasks;

namespace Products.Core.Tests;
public class CreateProductCommandHandlerTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IProductsRepository> _productRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CreateProductCommandHandler _sut;
    public CreateProductCommandHandlerTests() { 
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _productRepositoryMock = _fixture.Freeze<Mock<IProductsRepository>>();
        _mapperMock = _fixture.Freeze<Mock<IMapper>>();
        _sut = _fixture.Create<CreateProductCommandHandler>();
    }

    [Fact]
    public async Task CreateProduct_WhenValidRequest_ShouldCreateAndReturnProduct()
    {
        // Arrange
        var product = _fixture.Create<Product>();
        var request = _fixture.Create<CreateProductCommand>();
        var createdProductDto = _fixture.Create<ProductsResponseDto>();
        _mapperMock.Setup(m => m.Map<Product>(request)).Returns(product);
        _productRepositoryMock.Setup(p => p.AddProductAsync(product,It.IsAny<CancellationToken>())).ReturnsAsync(product);
        _mapperMock.Setup(m => m.Map<ProductsResponseDto>(product)).Returns(createdProductDto);
        // Act
        var result = await _sut.Handle(request,CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(createdProductDto);
        product.ProductId.Should().NotBe(Guid.Empty);

        _mapperMock.Verify(m => m.Map<ProductsResponseDto>(product), Times.Once());
        _productRepositoryMock.Verify(p => p.AddProductAsync(product,It.IsAny<CancellationToken>()), Times.Once());
    }
}
