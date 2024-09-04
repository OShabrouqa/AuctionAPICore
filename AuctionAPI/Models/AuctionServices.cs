using static AuctionAPI.Repositories.repo;

namespace AuctionAPI.Models
{
    public interface AuctionServices
    {
        public interface IAuctionService
        {
            Task<List<Property>> GetAllPropertiesAsync();
            Task<Property> GetPropertyByIdAsync(int propertyId);
            Task PlaceBidAsync(int propertyId, Bidder bidder);
        }

        public class AuctionService : IAuctionService
        {
            private readonly IPropertyRepository _propertyRepository;
            private readonly IBidderRepository _bidderRepository;

            public AuctionService(IPropertyRepository propertyRepository, IBidderRepository bidderRepository)
            {
                _propertyRepository = propertyRepository;
                _bidderRepository = bidderRepository;
            }

            public async Task<List<Property>> GetAllPropertiesAsync()
            {
                return await _propertyRepository.GetAllPropertiesAsync();
            }

            public async Task<Property> GetPropertyByIdAsync(int propertyId)
            {
                return await _propertyRepository.GetPropertyByIdAsync(propertyId);
            }

            public async Task PlaceBidAsync(int propertyId, Bidder bidder)
            {
                var property = await _propertyRepository.GetPropertyByIdAsync(propertyId);
                if (property == null || property.AuctionEndTime <= DateTime.Now)
                {
                    throw new InvalidOperationException("Cannot place bid on this property.");
                }

                bidder.PropertyId = propertyId;
                await _bidderRepository.AddBidderAsync(bidder);
            }
        }

    }
}
