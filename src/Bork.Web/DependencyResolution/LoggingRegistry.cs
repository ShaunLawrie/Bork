using NLog;
using StructureMap;

namespace Bork.Web.DependencyResolution
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
