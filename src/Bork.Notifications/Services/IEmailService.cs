using Bork.Contracts;

namespace Bork.Notifications.Services
{
    public interface IEmailService
    {
        void SendEmail(NotificationMessage message);
    }
}