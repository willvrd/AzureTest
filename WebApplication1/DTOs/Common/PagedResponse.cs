using System.Text.Json.Serialization;

namespace WebApplication1.DTOs.Common
{
    public class PagedResponse<T> : ResponseWrapper<IEnumerable<T>>
    {
        [JsonPropertyOrder(2)]
        public PaginationMetadata Pagination { get; set; }

        public PagedResponse(IEnumerable<T> data, int pageNumber, int pageSize, int totalRecords)
            : base(data) // Sends data to the parent (ResponseWrapper)
        {
            Pagination = new PaginationMetadata
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize)
            };
        }
    }

    public class PaginationMetadata
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
    }



}