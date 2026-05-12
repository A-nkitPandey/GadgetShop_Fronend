using GadgetShop.Models;

namespace GadgetShop.Core.Mapping;

internal static class ApiResponseMappingExtensions
{
    public static ApiResponse<TOut> MapData<TIn, TOut>(this ApiResponse<TIn> response, Func<TIn, TOut> map)
        where TIn : class
        where TOut : class
    {
        return new ApiResponse<TOut>
        {
            Code = response.Code,
            Message = response.Message,
            Errors = response.Errors,
            Data = response.Data is null ? null : map(response.Data)
        };
    }

    public static PagedResult<TOut> ToPagedResult<TIn, TOut>(
        this BackendPaginationResponse<List<TIn>> page,
        int pageNo,
        int pageSize,
        Func<TIn, TOut> map)
    {
        var records = page.Records ?? new List<TIn>();
        var totalRecords = (int)page.TotalCount;
        var totalPages = pageSize <= 0 ? 1 : Math.Max(1, (int)Math.Ceiling(totalRecords / (double)pageSize));

        return new PagedResult<TOut>
        {
            Items = records.Select(map).ToList(),
            PageNo = pageNo,
            PageSize = pageSize,
            TotalRecords = totalRecords,
            TotalPages = totalPages
        };
    }
}
