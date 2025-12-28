namespace EazyTicket.Core.Entities
{
    public class Ticket : BaseEntity
    {
        public string Number { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Customer { get; set; } = string.Empty;
    }
}
