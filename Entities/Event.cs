namespace Cross_WebApplication.Entities
{
    public class Event : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsProcessed { get; set; }
        public string ProcessedBy { get; set; }
        public DateTime? ProcessedAt { get; set; }
    }
}
