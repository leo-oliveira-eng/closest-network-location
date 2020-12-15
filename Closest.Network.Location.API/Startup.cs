using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Closest.Network.Location.API.Data.Mapping;
using Closest.Network.Location.API.Settings;
using Microsoft.Extensions.Options;
using Closest.Network.Location.API.Settings.Contracts;

namespace Closest.Network.Location.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddMvc();

            services.Configure<ClosestNetworkLocationSettings>(Configuration.GetSection(nameof(ClosestNetworkLocationSettings)));

            services.AddSingleton<IClosestNetworkLocationSettings>(sp =>
                sp.GetRequiredService<IOptions<ClosestNetworkLocationSettings>>().Value);

            GasStationMapping.MapGasStation();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
