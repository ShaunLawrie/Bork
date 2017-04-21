using Shouldly;
using Bork.Notifications.Configuration;
using Bork.Notifications.Services;
using Xunit;

namespace Bork.Notifications.Test
{
    public class ServicesRegistryTests
    {
        [Fact]
        public void ContainerShouldProvideServiceInstances()
        {
            var container = DependencyInjection.Configure();

            var service = container.GetInstance<IEmailService>();

            service.ShouldNotBeNull();
        }
    }
}
