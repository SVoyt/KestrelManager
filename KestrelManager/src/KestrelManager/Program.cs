using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace KestrelManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("hosting.json", optional: true)
                .Build();

            var host = new WebHostBuilder()
            .UseKestrel()
            .UseConfiguration(config)
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseStartup<Startup>()
            .Build();

            host.Run();
        }
    }
}
