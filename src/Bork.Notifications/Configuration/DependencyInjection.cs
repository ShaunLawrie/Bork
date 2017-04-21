using Microsoft.Extensions.Configuration;
using StructureMap;
using System.IO;

namespace Bork.Notifications.Configuration
{
    public static class DependencyInjection
    {
        private static IContainer _container;

        public static IContainer Configure()
        {
            // Scan for registries only
            _container = new Container(config =>
            {
                // Load configuration first
                config.For<IConfiguration>().Use(c => new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("AppSettings.json", false, true)
                   .Build())
                   .Singleton();

                // Scan for registries
                config.Scan(a =>
                {
                    a.TheCallingAssembly();
                    a.LookForRegistries();
                });
            });

            return _container;
        }
    }
}
