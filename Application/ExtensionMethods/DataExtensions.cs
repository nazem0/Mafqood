using Domain.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Application.ExtensionMethods
{
    public static class DataExtensions
    {
        public static async Task<PaginationViewDTO<TResult>> ToPaginationViewDTOAsync<TSource, TResult>
            (
            this IQueryable<TSource> data,
            int pageIndex,
            int pageSize,
            Func<TSource, TResult> selector
            )
        {
            data = data.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            var count = await data.LongCountAsync();
            return new PaginationViewDTO<TResult>
            {
                Data =  data.Select(selector).ToList(),
                Count = count,
                LastPage = (int)Math.Ceiling((double)count / pageSize),
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }

    }
}
