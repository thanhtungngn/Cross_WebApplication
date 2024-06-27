namespace CrossApplication.API.Models.Helper
{
    public class PagedResponse<T>
    {
        public List<T> Data { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public long TotalRecords { get; set; }
        public int TotalPages { get; set; }

        public PagedResponse()
        {
        }

        public PagedResponse(List<T> data, int pageNumber, int pageSize, int totalRecords)
        {
            this.Data = data;
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.TotalRecords = totalRecords;
            this.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
        }
    }
}
