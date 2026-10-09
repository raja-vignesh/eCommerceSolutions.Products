

using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using FluentAssertions;
using Moq;
using Products.Core.Dtos;
using Products.Core.Queries.GetProductBySearch;
using Products.Domain.Entities;
using Products.Domain.RepositoryContracts;
using System.Threading.Tasks;

namespace Products.Core.Tests;
public class SearchQueryHandlerTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IProductsRepository> _productRespositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetProductBySearchQueryHandler _sut;
    public SearchQueryHandlerTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _mapperMock = _fixture.Freeze<Mock<IMapper>>();
        _productRespositoryMock = _fixture.Freeze<Mock<IProductsRepository>>();
        _sut = _fixture.Create<GetProductBySearchQueryHandler>();
    }

    [Fact]
    public async Task SearchHandler_WhenValidSearchTerm_ShouldReturnResponse()
    {
        // Arrange
        var request = _fixture.Build<GetProductBySearchQuery>().With(p => p.SearchTerm,"laptop").With(p => p.PageNumber,1).With(p => p.PageSize,10).Create();
        var products = _fixture.CreateMany<Product>(5).ToList();
        var searchedProducts = _fixture.CreateMany<ProductsResponseDto>(5).ToList();
        var totalCount = products.Count();
        _productRespositoryMock.Setup(p => p.SearchProducts(request.SearchTerm,request.PageSize,request.PageNumber,It.IsAny<CancellationToken>())).ReturnsAsync((totalCount,products));
        _mapperMock.Setup(p => p.Map<IEnumerable<ProductsResponseDto>>(products)).Returns(searchedProducts);
        var pagedResult = new PagedResult<ProductsResponseDto>(searchedProducts, totalCount, request.PageNumber, request.PageSize);
        // Act
        var response = await _sut.Handle(request,CancellationToken.None);
        // Assert
        response.Should().BeEquivalentTo(pagedResult);

        _productRespositoryMock.Verify(p => p.SearchProducts(request.SearchTerm, request.PageSize, request.PageNumber, CancellationToken.None), Times.Once);
        _mapperMock.Verify(m => m.Map<IEnumerable<ProductsResponseDto>>(products), Times.Once);
    }

    [Fact]
    public async Task SearchHandler_WhenValidSearchTermNotMatching_ShouldReturnEmptyResponse()
    {
        // Arrange
        var request = _fixture.Build<GetProductBySearchQuery>().With(p => p.SearchTerm, "laptop").With(p => p.PageNumber, 1).With(p => p.PageSize, 10).Create();
        var products = _fixture.CreateMany<Product>(5).ToList();
        var searchedProducts = _fixture.CreateMany<ProductsResponseDto>(0).ToList();
        var totalCount = 0;
        _productRespositoryMock.Setup(p => p.SearchProducts(request.SearchTerm, request.PageSize, request.PageNumber, It.IsAny<CancellationToken>())).ReturnsAsync((totalCount, products));
        _mapperMock.Setup(p => p.Map<IEnumerable<ProductsResponseDto>>(products)).Returns(searchedProducts);
        var pagedResult = new PagedResult<ProductsResponseDto>(searchedProducts, totalCount, request.PageNumber, request.PageSize);
        // Act
        var response = await _sut.Handle(request, CancellationToken.None);
        // Assert
        response.Should().BeEquivalentTo(pagedResult);
        response.TotalResultsCount.Should().Be(0);
        _productRespositoryMock.Verify(p => p.SearchProducts(request.SearchTerm, request.PageSize, request.PageNumber, CancellationToken.None), Times.Once);
        _mapperMock.Verify(m => m.Map<IEnumerable<ProductsResponseDto>>(products), Times.Once);
    }
}
