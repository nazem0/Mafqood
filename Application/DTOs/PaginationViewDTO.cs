namespace Domain.DTOs
{
    public class PaginationViewDTO<T>
    {
        public required IEnumerable<T> Data { get; set; }
        public required int PageSize { get; set; }
        public required int PageIndex { get; set; }
        public required long Count { get; set; }
        public required int LastPage { get; set; }
    }
}
