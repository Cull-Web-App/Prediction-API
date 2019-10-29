using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Dapper;
using Dapper.FluentMap;
using Prediction_API.Models;
using Prediction_API.Services;

namespace Prediction_API
{
    public class Startup
    {
        public static IConfiguration Configuration { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            // Inject the historical prediction service into the app
            services.AddTransient<IHistoricalPredictionService, HistoricalPredictionService>();

            // Add each of the Typed Http clients for the services to DI -- this wires up both the interface and class for DI
            services.AddHttpClient<IStockTickerService, StockTickerService>(client =>
            {
                // Add all of this HTTP clients configurations here!
                client.BaseAddress = new Uri(Startup.Configuration.GetValue<string>(string.Format("API_URLS:{0}:FinancialDataAPI", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"))));
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints => 
            {
                endpoints.MapControllers();
            });

            this.ConfigureCustomDapperMappings();
        }

        public void ConfigureCustomDapperMappings()
        {
            // Use Dappers FluentMap to map the types to this
            FluentMapper.Initialize(config =>
            {
                // Configure all entities in the current assembly with an optional namespaces filter.
                config.AddConvention<PropertyTransformConvention>()
                      .ForEntitiesInCurrentAssembly("Prediction_API.Models");
            });
        }
    }
}
