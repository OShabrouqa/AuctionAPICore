using AuctionAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionAPI.Repositories
{
    public interface repo
    {
        public interface IPropertyRepository
        {
            Task<Property> GetPropertyByIdAsync(int propertyId);
            Task<List<Property>> GetAllPropertiesAsync();
            Task AddPropertyAsync(Property property);
        }

        public interface IBidderRepository
        {
            Task AddBidderAsync(Bidder bidder);
        }

        public class PropertyRepository : IPropertyRepository
        {
            private readonly AuctionContext _context;

            public PropertyRepository(AuctionContext context)
            {
                _context = context;
            }

            public async Task<Property> GetPropertyByIdAsync(int propertyId)
            {
                return await _context.Properties.Include(p => p.Bidders).FirstOrDefaultAsync(p => p.PropertyId == propertyId);
            }

            public async Task<List<Property>> GetAllPropertiesAsync()
            {
                return await _context.Properties.Include(p => p.Bidders).ToListAsync();
            }

            public async Task AddPropertyAsync(Property property)
            {
                _context.Properties.Add(property);
                await _context.SaveChangesAsync();
            }
        }

        public class BidderRepository : IBidderRepository
        {
            private readonly AuctionContext _context;

            public BidderRepository(AuctionContext context)
            {
                _context = context;
            }

            public async Task AddBidderAsync(Bidder bidder)
            {
                _context.Bidders.Add(bidder);
                await _context.SaveChangesAsync();
            }
        }

    }
}
