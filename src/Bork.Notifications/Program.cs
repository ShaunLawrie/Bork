using System;
using System.Runtime.Loader;
using System.Threading;
using Bork.Notifications.Services;
using Bork.Notifications.Configuration;
using Microsoft.Extensions.Configuration;
using NLog;

namespace Bork.Notifications
{
    internal class Program
    {
        private static bool _shutdown;
        private static ILogger _logger;
        private static IQueuingService _queuingService;

        // ReSharper disable once UnusedMember.Local
        private static void Main()
        {
            AssemblyLoadContext.Default.Unloading += Shutdown;
            Console.CancelKeyPress += (s, e) => Shutdown();

            try
            {
                _logger = LogManager.GetCurrentClassLogger();
                var container = DependencyInjection.Configure();
                var config = container.GetInstance<IConfiguration>();

                _queuingService = container.GetInstance<IQueuingService>();
                
                //logger.Info($"Messages in queue: {queuingService.Count()}");
                _queuingService.ConfigureEventHandlers(int.Parse(config["RabbitMQ:Concurrency"]));

                // EventHandlers are now running

                _logger.Info("Running");

                // Waiting for shutdown on main thread
                SpinWait.SpinUntil(() => _shutdown);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unhandled Exception: {e}");
            }
        }

        private static void Shutdown(AssemblyLoadContext obj = null)
        {
            _logger.Info("Shutting down notifications service");
            
            // Die now
            _queuingService.Shutdown();

            _logger.Info("Notification service has shutdown");

            _shutdown = true;
        }
    }
}