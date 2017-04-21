using System;

namespace Bork.Api.Services
{
    public interface IQueuingService : IDisposable
    {
        bool Send(object message);
    }
}