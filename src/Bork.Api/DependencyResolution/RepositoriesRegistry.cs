using StructureMap;

namespace Bork.Api.DependencyResolution
{
    public class RepositoriesRegistry : Registry
    {
        public RepositoriesRegistry()
        {
            Scan(a =>
            {
                a.TheCallingAssembly();
                a.IncludeNamespace("Bork.Api.Repositories");
                a.WithDefaultConventions();
            });
        }
    }
}
