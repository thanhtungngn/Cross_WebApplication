namespace Cross_WebApplication.Entities
{
    public class Role : BaseEntity
    {
        public Role(string name)
        {
            this.Name = name;
        }
        public string Name { get; set; }
    }
}
