using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealFinderApi.Models;

namespace DealFinderApi.Controllers
{
    [Route("api/PriceListings")]
    [ApiController]
    public class PriceListingsController : ControllerBase
    {
        private readonly PriceContext _context;

        public PriceListingsController(PriceContext context)
        {
            _context = context;
        }

        /*        // GET: api/PriceListings/register
                // We gonna turn this into the register page baby.
                [HttpPost]
                public async Task<ActionResult<IEnumerable<PriceListing>>> Register()
                {

                }

                // GET: api/PriceListings/signin
                // We gonna turn this into the signin page (ahem.. action) baby.
                [HttpPost]
                public async Task<ActionResult<IEnumerable<PriceListing>>> Signin()
                {

                }*/

        // gets all listings from the database
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PriceListing>>> GetPriceListings()
        {
            return await _context.PriceListings
                    .ToListAsync();
        }

        // GET: api/PriceListings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PriceListing>> GetPriceListing(long id)
        {
            var priceListing = await _context.PriceListings.FindAsync(id);

            if (priceListing == null)
            {
                return NotFound();
            }

            return priceListing;
        }

        // PUT: api/PriceListings/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPriceListing(long id, PriceListing priceListingDTO)
        {
            if (id != priceListingDTO.Id)
            {
                return BadRequest();
            }

            var priceListing = await _context.PriceListings.FindAsync(id);
            if (priceListing == null)
            {
                return NotFound();
            }

            priceListing.Url = priceListingDTO.Url;
            priceListing.ItemPrice = priceListingDTO.ItemPrice;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!PriceListingExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/PriceListings
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<PriceListing>> PostPriceListing(PriceListing priceListingDTO)
        {
            var priceListing = new PriceListing
            {
                Url = priceListingDTO.Url,
                ItemPrice = priceListingDTO.ItemPrice
            };

            _context.PriceListings.Add(priceListing);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPriceListing), new { id = priceListing.Id }, priceListing);
        }

        // DELETE: api/PriceListings/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PriceListing>> DeletePriceListing(long id)
        {
            var priceListing = await _context.PriceListings.FindAsync(id);
            if (priceListing == null)
            {
                return NotFound();
            }

            _context.PriceListings.Remove(priceListing);
            await _context.SaveChangesAsync();

            return priceListing;
        }

        private bool PriceListingExists(long id)
        {
            return _context.PriceListings.Any(e => e.Id == id);
        }

/*        private static PriceListing ListingToDTO(PriceListing priceListing) =>
            new PriceListing
            {
                Id = priceListing.Id,
                Url = priceListing.Url,
                ItemPrice = priceListing.ItemPrice
            };*/
    }
}
