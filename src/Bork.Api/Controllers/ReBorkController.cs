using Bork.Api.Repositories;
using Bork.Api.Services;
using Bork.Contracts;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System;

namespace Bork.Api.Controllers
{
    [Route("[controller]")]
    public class ReBorkController : Controller
    {
        
        private readonly ILogger _logger;
        private readonly IBorkRepository _borkRepo;
        private readonly IQueuingService _queuingService;
        
        public ReBorkController(ILogger logger,
            IBorkRepository borkRepo,
            IQueuingService queuingService)
        {
            _logger = logger;
            _borkRepo = borkRepo;
            _queuingService = queuingService;
        }

        // GET /bork
        [HttpGet]
        public IActionResult Index()
        {
            _logger.Info("Getting top reborks");
            var topBorks = _borkRepo.TopReBorks(20);
            return Ok(topBorks);
        }

        // GET /rebork/{borkid}
        [HttpGet]
        [Route("{borkId}")]
        public IActionResult GetSpecificReBork([FromRoute] int borkId)
        {
            _logger.Info($"Getting specific rebork with id '{borkId}'");
            var bork = _borkRepo.GetReBorkById(borkId);
            return Ok(bork);
        }

        // POST /rebork
        [HttpPost]
        public IActionResult Post([FromBody] ReBorkRecord reBork)
        {
            _logger.Info("Trying to create a rebork");
            reBork.DateCreated = DateTime.Now;
            var newBork = _borkRepo.AddReBork(reBork);

            // Send notification
            var notification = new NotificationMessage()
            {
                From = "noreply@bork.bork",
                To = reBork.OriginalUserName,
                Subject = "You were ReBorked",
                Body = $"You were ReBorked by {reBork.UserName} and the"
                     + $"original content was '{reBork.OriginalContent}'"
            };
            _queuingService.Send(notification);

            return Created($"/bork/{reBork.Id}", reBork);
        }
    }
}
