using Domain.DTOs;

namespace Infrastructure
{
    public static class DataExtensions
    {
        public static PaginationViewDTO<TResult> ToPaginationViewDTO<TSource, TResult>
            (
            this IEnumerable<TSource> data,
            int pageIndex, int pageSize,
            Func<TSource, TResult> selector
            )
        {
            long count = data.LongCount();
            return new PaginationViewDTO<TResult>
            {
                Data = data.Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(selector),
                Count = count,
                LastPage = (int)Math.Ceiling((double)count / pageSize),
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }

    }
}
