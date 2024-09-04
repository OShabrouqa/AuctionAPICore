using AuctionAPI.Models;
using Microsoft.AspNetCore.Mvc;
using static AuctionAPI.Models.AuctionServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuctionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionController : ControllerBase
    {
        private readonly IAuctionService _auctionService;

        public AuctionController(IAuctionService auctionService)
        {
            _auctionService = auctionService;
        }

        [HttpGet("properties")]
        public async Task<IActionResult> GetAllProperties(
       string? name = null,
       string? description = null,
       decimal? startingPrice = null,
       DateTime? auctionEndTime = null,
       string? sortBy = null,  // Property to sort by
       string? sortDirection = "asc") // asc or desc
        {
            var properties = await _auctionService.GetAllPropertiesAsync(name, description, startingPrice, auctionEndTime, sortBy, sortDirection);
            return Ok(properties);
        }

        [HttpGet("properties/{propertyId}")]
        public async Task<IActionResult> GetPropertyById(int propertyId)
        {
            var property = await _auctionService.GetPropertyByIdAsync(propertyId);
            if (property == null)
            {
                return NotFound();
            }
            return Ok(property);
        }
        [HttpGet("Bids")]
        public async Task<IActionResult> GetAllBids(
      string? name = null,
      decimal? bidAmount = null,
      DateTime? bidTime = null,
      string? sortBy = null,
      string? sortDirection = "asc")
        {
            var properties = await _auctionService.GetAllBidsAsync(name, bidAmount, bidTime, sortBy, sortDirection);
            return Ok(properties);
        }

      

        [HttpPost("properties/{propertyId}/bid")]
        public async Task<IActionResult> PlaceBid(int propertyId, [FromBody] PlaceBidDTO placeBidDto)
        {
            if (propertyId != placeBidDto.PropertyId)
            {
                return BadRequest("PropertyId in the route and the body must match.");
            }

            if (placeBidDto.BidAmount <= 0)
            {
                return BadRequest("Bid amount must be greater than zero.");
            }

            try
            {
                var bidder = new Bidder
                {
                    BidAmount = placeBidDto.BidAmount,
                    BidTime = DateTime.Now,
                    PropertyId = placeBidDto.PropertyId,
                    Name = placeBidDto.Name
                };

                await _auctionService.PlaceBidAsync(placeBidDto.PropertyId, bidder);

                return Ok("Bid placed successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
