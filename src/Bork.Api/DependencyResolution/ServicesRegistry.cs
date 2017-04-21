using StructureMap;

namespace Bork.Api.DependencyResolution
{
    public class ServicesRegistry : Registry
    {
        public ServicesRegistry()
        {
            Scan(a =>
            {
                a.TheCallingAssembly();
                a.IncludeNamespace("Bork.Api.Services");
                a.WithDefaultConventions();
            });
        }
    }
}
