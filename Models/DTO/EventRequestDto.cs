namespace CrossApplication.API.Models.DTO
{
    public class EventRequestDto
    {
        public string SearchQuery { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortOrder { get; set; } = "createdAt";
        public Boolean SortByAsc { get; set; } = true;
    }
}
