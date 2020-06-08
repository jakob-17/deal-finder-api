using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealFinderApi.Models;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

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
        public async Task<IActionResult> PutPriceListing(long id, PriceListing priceListing)
        {
            if (id != priceListing.Id)
            {
                return BadRequest();
            }

            var priceListingDb = await _context.PriceListings.FindAsync(id);
            if (priceListingDb == null)
            {
                return NotFound();
            }

            priceListingDb.Url = priceListing.Url;
            priceListingDb.ItemPrice = priceListing.ItemPrice;

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
        public async Task<ActionResult<PriceListing>> PostPriceListing(PriceListing priceListing)
        {
            string htmlData = null;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(priceListing.Url);
            request.UserAgent = @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream recieveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (string.IsNullOrWhiteSpace(response.CharacterSet))
                {
                    readStream = new StreamReader(recieveStream);
                }
                else
                {
                    readStream = new StreamReader(recieveStream, Encoding.GetEncoding(response.CharacterSet));
                }

                htmlData = readStream.ReadToEnd();

                response.Close();
                readStream.Close();
            }

            if (string.IsNullOrWhiteSpace(htmlData))
            {
                throw new System.Exception("Error: Could not connect to web page");
            }

            string titlePattern = @"<title>(.*?)</title>";
            string pricePattern = @"<(.*?)price(.*?)>(.*?).(\d{2})</(.*?)>";
            string pricePatternAlt = "price\":\"(.*?).(\\d{2})\"";

            var title = Regex.Match(htmlData, titlePattern);
            var price = Regex.Matches(htmlData.ToLower(), pricePattern);

            if (string.IsNullOrEmpty(title.Value))
            {
                throw new System.Exception("Title not found");
            }
            if (price.Count < 1)
            {
                price = Regex.Matches(htmlData.ToLower(), pricePatternAlt);
                if (price.Count < 1) {
                    throw new System.Exception("Price not found");
                }            
            }

            PriceListing pl = new PriceListing();
            pl.Url = priceListing.Url;
            pl.ItemTitle = title.Value;
            pl.ItemPrice = "";
            foreach (var p in price)
            {
                pl.ItemPrice += p.ToString();
            }

            _context.PriceListings.Add(pl);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPriceListing), new { id = pl.Id }, pl);
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
