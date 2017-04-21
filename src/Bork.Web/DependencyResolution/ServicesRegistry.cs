using StructureMap;

namespace Bork.Web.DependencyResolution
{
    public class ServicesRegistry : Registry
    {
        public ServicesRegistry()
        {
            Scan(a =>
            {
                a.TheCallingAssembly();
                a.IncludeNamespace("Bork.Web.Services");
                a.WithDefaultConventions();
            });
        }
    }
}
