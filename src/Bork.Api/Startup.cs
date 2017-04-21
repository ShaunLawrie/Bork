using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using StructureMap;
using Bork.Api.Data;

namespace Bork.Api
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile(path: "AppSettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile(path: $"AppSettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            // Add framework services.
            services.AddDbContext<BorkDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddMvc().AddControllersAsServices();

            return ConfigureStructureMap(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory
                .AddDebug()
                .AddConsole();
            app.UseMvc();
        }

        private IServiceProvider ConfigureStructureMap(IServiceCollection services)
        {
            // Add the basic logging for debug/console out. Use NLog for controllers and services
            services.AddLogging();

            // Scan for registries only
            var container = new Container(config =>
            {
                // Configuration before registry scanning
                config.For<IConfiguration>()
                    .Use(Configuration)
                    .Singleton();

                // Scan for registries
                config.Scan(a =>
                {
                    a.TheCallingAssembly();
                    a.LookForRegistries();
                });
            });
            
            container.Populate(services);

            return container.GetInstance<IServiceProvider>();
        }
    }
}
