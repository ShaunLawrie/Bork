using NLog;
using System;
using System.Threading;
using Bork.Contracts;

namespace Bork.Notifications.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger _logger;

        public EmailService(ILogger logger)
        {
            _logger = logger;
        }

        public void SendEmail(NotificationMessage message)
        {
            var r = new Random(Thread.CurrentThread.ManagedThreadId);
            var milliseconds = r.Next(1500) + 5000;

            // Purposefully blocking
            Thread.Sleep(milliseconds); // Derp, sending email

            _logger.Info($"Sent email '{message.Subject}' to '{message.To}' and took {milliseconds}ms");
        }
    }
}
