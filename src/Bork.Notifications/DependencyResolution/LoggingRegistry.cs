using NLog;
using StructureMap;

namespace Bork.Notifications.DependencyResolution
{
    public class LoggingRegistry : Registry
    {
        public LoggingRegistry()
        {
            For<ILogger>()
                .Use(ctx => LogManager.GetLogger(ctx.ParentType.ToString()))
                .AlwaysUnique();
        }
    }
}
