using KestrelManager.Engine;
using KestrelManager.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KestrelManager
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        readonly IHostingEnvironment _hostingEnvironment;

        public Startup(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;

            var builder = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var settings = new Settings();
            Configuration.Bind(settings);
            services.AddSingleton<IAppReporitory>(new AppRepository(settings.ApplicationParameters));


            var jsonOutputSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            };
            var jsonOutputFormatter = new Microsoft.AspNetCore.Mvc.Formatters.JsonOutputFormatter(jsonOutputSettings, ArrayPool<char>.Shared);

            services.AddMvc(
                options =>
                {
                    options.OutputFormatters.Clear();
                    options.OutputFormatters.Insert(0, jsonOutputFormatter);
                }
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseMvc();
        }
    }
}
