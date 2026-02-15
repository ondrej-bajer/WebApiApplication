namespace WebApiApplication.DTOs
{
    public sealed record PagedResponse<T>
    (
    IReadOnlyList<T> Items,
    int Page,
    int PageSize,
    int TotalCount)
    {
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
