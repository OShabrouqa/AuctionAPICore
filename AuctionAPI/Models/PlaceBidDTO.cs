namespace AuctionAPI.Models
{
    public class PlaceBidDTO
    {
        public string Name { get; set; }
        public decimal BidAmount { get; set; }
        public int PropertyId { get; set; }
    }
}
