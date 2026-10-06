
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using FluentAssertions;
using Moq;
using Products.Core.Dtos;
using Products.Core.Queries.GetProducts;
using Products.Domain.Entities;
using Products.Domain.RepositoryContracts;

namespace Products.Core.Tests;
public class GetProductsQueryTests 
{
    private readonly IFixture _fixture;
    private readonly Mock<IProductsRepository> _productsRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetProductsQueryHandler _sut;

    public GetProductsQueryTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _productsRepositoryMock = _fixture.Freeze<Mock<IProductsRepository>>(); 
        _mapperMock = _fixture.Freeze<Mock<IMapper>>();
        _sut = _fixture.Create<GetProductsQueryHandler>();
    }

    [Fact]
    public async Task Handler_WhenValidRequest_ShouldReturnPagedResult()
    {
        // Arrange
        var request = _fixture.Create<GetProductsQuery>();
        var products = _fixture.CreateMany<Product>(3).ToList();
        var productResponseDtos = _fixture.CreateMany<ProductsResponseDto>(3).ToList();

        _productsRepositoryMock.Setup(p => p.GetProductsAsync(request.PageSize, request.PageNumber, It.IsAny<CancellationToken>())).ReturnsAsync((products.Count,products));
        _mapperMock.Setup(m => m.Map<IEnumerable<ProductsResponseDto>>(products)).Returns(productResponseDtos);
        // Act
        var result = await _sut.Handle(request,CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.TotalResultsCount.Should().Be(products.Count);
        result.PageNumber.Should().Be(request.PageNumber);
        result.PageSize.Should().Be(request.PageSize);

    }

    [Fact]
    public async Task Handler_WhenValidRequest_ShouldReturnEmptyPagedResult()
    {
        // Arrange
        var request = _fixture.Create<GetProductsQuery>();
        var products = _fixture.CreateMany<Product>(0).ToList();
        var productResponseDtos = _fixture.CreateMany<ProductsResponseDto>(0).ToList();

        _productsRepositoryMock.Setup(p => p.GetProductsAsync(request.PageSize, request.PageNumber, It.IsAny<CancellationToken>())).ReturnsAsync((products.Count, products));
        _mapperMock.Setup(m => m.Map<IEnumerable<ProductsResponseDto>>(products)).Returns(productResponseDtos);
        // Act
        var result = await _sut.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.TotalResultsCount.Should().Be(0);
        result.PageNumber.Should().Be(request.PageNumber);
        result.PageSize.Should().Be(request.PageSize);

    }
}
