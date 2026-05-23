namespace EventMiniApp.Dtos.TicketDto
{
    public class TicketGetDto
    {
        public int Id { get; set; }

        public string Type { get; set; } = null!;

        public decimal Price { get; set; }

        public int QuantityAvailable { get; set; }

        public int EventId { get; set; }
    }
}
