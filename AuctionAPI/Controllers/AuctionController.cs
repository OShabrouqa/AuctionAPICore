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
        public async Task<IActionResult> GetAllProperties()
        {
            var properties = await _auctionService.GetAllPropertiesAsync();
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

        [HttpPost("properties/{propertyId}/bid")]
        public async Task<IActionResult> PlaceBid(int propertyId, [FromBody] Bidder bidder)
        {
            try
            {
                await _auctionService.PlaceBidAsync(propertyId, bidder);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
