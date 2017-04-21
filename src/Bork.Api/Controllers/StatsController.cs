using Bork.Api.Repositories;
using Bork.Contracts;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace Bork.Api.Controllers
{
    [Route("[controller]")]
    public class StatsController : Controller
    {
        
        private readonly ILogger _logger;
        private readonly IBorkRepository _borkRepo;
        
        public StatsController(ILogger logger,
            IBorkRepository borkRepo)
        {
            _logger = logger;
            _borkRepo = borkRepo;
        }
        
        // GET /stats/{username}
        [HttpGet]
        [Route("{username}")]
        public IActionResult GetStats([FromRoute] string username)
        {
            _logger.Info($"Getting bork stats for user '{username}'");
            var stats = new BorkStats
            {
                Borks = _borkRepo.BorkCount(username),
                ReBorks = _borkRepo.ReBorkCount(username),
                ReBorked = _borkRepo.ReBorkedCount(username)
            };

            return Ok(stats);
        }
    }
}
