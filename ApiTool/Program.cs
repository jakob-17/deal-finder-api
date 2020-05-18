using System;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

namespace ApiTool
{
    class Program
    {
        [Option]
        public bool GetAllListings { get; set; }

        public static int Main(string[] args)
        {
            var services = new ServiceCollection()
                .AddSingleton<IDealService, DealService>()
                .AddSingleton<IConsole>(PhysicalConsole.Singleton)
                .BuildServiceProvider();

            var app = new CommandLineApplication<Program>();
            app.Conventions
                .UseDefaultConventions()
                .UseConstructorInjection(services);

            return app.Execute(args);
        }

        private readonly IDealService _dealService;

        public Program(IDealService dealService)
        {
            _dealService = dealService;
        }

        private void OnExecute()
        {
            if (GetAllListings)
            {          
                _dealService.GetListings();
            }
            else
            {
                _dealService.Default();
            }
        }
    }
}
