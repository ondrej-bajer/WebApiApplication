using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApiApplication.DTOs
{
    public sealed class PaginationQuery
    {
        [DefaultValue(1)]
        [Range(1, int.MaxValue)]
        public int Page { get; init; } = 1;

        [DefaultValue(10)]
        [Range(1, 100)]
        public int PageSize { get; init; } = 10;
    }
}
