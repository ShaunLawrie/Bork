using Bork.Api.Services;
using Bork.Contracts;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace Bork.Api.Controllers
{
    [Route("[controller]")]
    public class NotificationsController : Controller
    {
        
        private readonly ILogger _logger;
        private readonly IQueuingService _queuingService;

        public NotificationsController(ILogger logger,
            IQueuingService queuingService)
        {
            _logger = logger;
            _queuingService = queuingService;
        }

        // POST notifications
        [HttpPost]
        public IActionResult Post([FromBody]NotificationMessage message)
        {
            _queuingService.Send(message);
            return Ok($"Sent '{message.Subject}' to '{message.To}'");
        }
    }
}
