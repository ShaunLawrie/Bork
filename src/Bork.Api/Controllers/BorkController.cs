using Bork.Api.Repositories;
using Bork.Contracts;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System;

namespace Bork.Api.Controllers
{
    [Route("[controller]")]
    public class BorkController : Controller
    {
        
        private readonly ILogger _logger;
        private readonly IBorkRepository _borkRepo;
        
        public BorkController(ILogger logger,
            IBorkRepository borkRepo)
        {
            _logger = logger;
            _borkRepo = borkRepo;
        }

        // GET /bork
        [HttpGet]
        public IActionResult Index()
        {
            _logger.Info("Getting top borks");
            var topBorks = _borkRepo.TopBorks(20);
            return Ok(topBorks);
        }

        // GET /bork/{borkid}
        [HttpGet]
        [Route("{borkId}")]
        public IActionResult GetSpecificBork([FromRoute] int borkId)
        {
            _logger.Info($"Getting specific bork with id '{borkId}'");
            var bork = _borkRepo.GetBorkById(borkId);
            return Ok(bork);
        }

        // POST /bork
        [HttpPost]
        public IActionResult Post([FromBody] BorkRecord bork)
        {
            _logger.Info("Trying to create a bork");
            bork.DateCreated = DateTime.Now;
            var newBork = _borkRepo.AddBork(bork);
            return Created($"/bork/{bork.Id}", bork);
        }
    }
}
