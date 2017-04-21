using Shouldly;
using Bork.Notifications.Configuration;
using Bork.Notifications.Services;
using Xunit;
using System.Diagnostics;
using Bork.Contracts;

namespace Bork.Notifications.Tests
{
    public class EmailServiceTests
    {
        [Fact]
        public void EmailSendShouldTakeLongerThanOrEqualTo500Ms()
        {
            var container = DependencyInjection.Configure();
            var service = container.GetInstance<IEmailService>();
            var sw = new Stopwatch();
            var message = new NotificationMessage
            {
                To = "asd",
                From = "asd",
                Subject = "asd",
                Body = "asd"
            };

            sw.Start();
            service.SendEmail(message);
            sw.Stop();

            sw.ElapsedMilliseconds.ShouldBeGreaterThanOrEqualTo(500);
        }
    }
}
