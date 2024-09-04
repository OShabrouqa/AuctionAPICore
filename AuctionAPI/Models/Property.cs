namespace AuctionAPI.Models
{
    public class Property
    {
        public int PropertyId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal StartingPrice { get; set; }
        public DateTime AuctionEndTime { get; set; }
        public List<Bidder> Bidders { get; set; } = new List<Bidder>();
    }
}
