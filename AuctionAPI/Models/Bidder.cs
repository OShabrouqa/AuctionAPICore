namespace AuctionAPI.Models
{
    public class Bidder
    {
        public int BidderId { get; set; }
        public string Name { get; set; }
        public decimal BidAmount { get; set; }
        public DateTime BidTime { get; set; }
        public int PropertyId { get; set; }
        public Property Property { get; set; }
    }
}
