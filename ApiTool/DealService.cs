using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ApiTool
{
    class DealService : IDealService
    {
        private readonly IConsole _console;

        private readonly HttpClient client = new HttpClient();

        private readonly string PriceListingsUri = "https://localhost:44379/";

        public DealService(IConsole console)
        {
            _console = console;
        }

        private async Task<string> GetAllListings()
        {
            return await client.GetStringAsync("https://localhost:44379/" + "api/PriceListings");
        }  

        public void GetListings(int listing = -1)
        {
            _console.WriteLine("Calling api...");
            Task<string> result = GetAllListings();
            _console.WriteLine(result.Result);
        }

        public void Default()
        {
            _console.WriteLine("No arguments provided");
        }
    }
}
