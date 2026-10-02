
namespace Products.Core.Dtos;
public class PagedResult<T>
{
    public PagedResult(IEnumerable<T> items, int totalResultsCount, int pageNumber, int pageSize) {
        Items = items;
        TotalResultsCount = totalResultsCount ;
        ItemsFrom = totalResultsCount == 0 ? 0 : ((pageNumber - 1) * pageSize) + 1;
        ItemsTo = Math.Min((ItemsFrom + pageSize) - 1, totalResultsCount);
        PageSize = pageSize;
        TotalPages = (int) Math.Ceiling( totalResultsCount / (double) PageSize);
        PageNumber = pageNumber;
    }
    public IEnumerable<T> Items { get; }
    public int TotalResultsCount { get; } 

    public int ItemsFrom { get;  } 

    public int ItemsTo { get;  } 

    public int PageSize { get;  } 

    public int TotalPages {  get; } 

    public int PageNumber { get; } 
}
