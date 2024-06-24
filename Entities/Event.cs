namespace Cross_WebApplication.Entities
{
    public class Event : BaseEntity
    {
        public string Description { get; set; }
        public bool IsProcessed { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
