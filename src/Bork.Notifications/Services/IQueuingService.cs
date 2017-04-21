using System;

namespace Bork.Notifications.Services
{
    public interface IQueuingService : IDisposable
    {
        long Count();
        void ConfigureEventHandlers(int concurrency);
        void Shutdown();
    }
}