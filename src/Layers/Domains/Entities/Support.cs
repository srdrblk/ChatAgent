namespace Entities
{
    public class Support: BaseEntity
    {
        public User User { get; set; }
        public string Subject { get; set; }
    }
}
