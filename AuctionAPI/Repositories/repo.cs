using AuctionAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionAPI.Repositories
{
    public interface repo
    {
        public interface IPropertyRepository
        {
            Task<Property> GetPropertyByIdAsync(int propertyId);
            Task<List<Property>> GetAllPropertiesAsync(
                 string? name = null,
                 string? description = null,
                 decimal? startingPrice = null,
                 DateTime? auctionEndTime = null,
                 string? sortBy = null,
                 string? sortDirection = "asc");
      

            Task AddPropertyAsync(Property property);
        }

        public interface IBidderRepository
        {
            public  Task<List<Bidder>> GetAllBidsAsync(
string? name = null,
decimal? bidAmount = null,
DateTime? bidTime = null,
string? sortBy = null,
string? sortDirection = "asc");

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
                return await _context.Properties.FirstOrDefaultAsync(p => p.PropertyId == propertyId);
            }

            public async Task<List<Property>> GetAllPropertiesAsync(
        string? name = null,
        string? description = null,
        decimal? startingPrice = null,
        DateTime? auctionEndTime = null,
        string? sortBy = null,
        string? sortDirection = "asc")
            {
                var query = _context.Properties.AsQueryable();

                // Apply filters
                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(p => p.Name.Contains(name));
                }

                if (!string.IsNullOrEmpty(description))
                {
                    query = query.Where(p => p.Description.Contains(description));
                }

                if (startingPrice.HasValue)
                {
                    query = query.Where(p => p.StartingPrice >= startingPrice.Value);
                }

                if (auctionEndTime.HasValue)
                {
                    query = query.Where(p => p.AuctionEndTime <= auctionEndTime.Value);
                }

                // Apply sorting
                if (!string.IsNullOrEmpty(sortBy))
                {
                    query = sortDirection.ToLower() == "desc"
                        ? query.OrderByDescending(p => EF.Property<object>(p, sortBy))
                        : query.OrderBy(p => EF.Property<object>(p, sortBy));
                }

                return await query.ToListAsync();
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
            public async Task<List<Bidder>> GetAllBidsAsync(
 string? name = null,
 decimal? bidAmount = null,
 DateTime? bidTime = null,
 string? sortBy = null,
 string? sortDirection = "asc")
            {
                var query = _context.Bidders.AsQueryable();

                // Apply filters
                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(b => b.Name.Contains(name));
                }

                if (bidAmount.HasValue)
                {
                    query = query.Where(b => b.BidAmount >= bidAmount.Value);
                }

                if (bidTime.HasValue)
                {
                    query = query.Where(b => b.BidTime <= bidTime.Value);
                }

                // Apply sorting
                if (!string.IsNullOrEmpty(sortBy))
                {
                    query = sortDirection.ToLower() == "desc"
                        ? query.OrderByDescending(b => EF.Property<object>(b, sortBy))
                        : query.OrderBy(b => EF.Property<object>(b, sortBy));
                }

                return await query.ToListAsync();
            }
            public async Task AddBidderAsync(Bidder bidder)
            {
                _context.Bidders.Add(bidder);
                await _context.SaveChangesAsync();
            }
        }

    }
}
