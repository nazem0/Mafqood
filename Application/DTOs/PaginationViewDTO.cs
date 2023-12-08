namespace Domain.DTOs
{
    public class PaginationViewDTO<T>
    {
        public required IEnumerable<T> Data { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public long Count { get; set; }
        public int LastPage { get; set; }
    }
}
