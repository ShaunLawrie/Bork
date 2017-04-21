using StructureMap;

namespace Bork.Notifications.DependencyResolution
{
    public class ServicesRegistry : Registry
    {
        public ServicesRegistry()
        {
            Scan(a =>
            {
                a.TheCallingAssembly();
                a.IncludeNamespace("Bork.Notifications.Services");
                a.WithDefaultConventions();
            });
        }
    }
}
