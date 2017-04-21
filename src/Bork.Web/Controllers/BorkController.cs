using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Bork.Contracts;
using Bork.Web.Models;
using NLog;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Bork.Web.Services;
using System;
using System.Collections.Generic;
using Bork.Web.Models.BorkViewModels;
using Bork.Web.Models.HomeViewModels;
using AutoMapper;

namespace Bork.Web.Controllers
{
    [Authorize]
    public class BorkController : Controller
    {
        private readonly ILogger _logger;
        private readonly IBorkApiAccessService _borkApiAccess;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public BorkController(ILogger logger,
            IBorkApiAccessService borkApiAccess,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            _borkApiAccess = borkApiAccess;
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> TopBorks()
        {
            var username = HttpContext.User.Identity.Name;
            List<DisplayBork> mergedBorks = new List<DisplayBork>();
            IList<BorkRecord> topBorks = new List<BorkRecord>();
            IList<ReBorkRecord> topReBorks = new List<ReBorkRecord>();

            // Get latest borks
            try
            {
                topBorks = await _borkApiAccess.GetTopBorks();
                var dispBorks = _mapper.Map<IList<BorkRecord>, IList<DisplayBork>>(topBorks);
                mergedBorks.AddRange(dispBorks);
            }
            catch (Exception e)
            {
                _logger.Error(e, "Failed to get top borks.");
            }
            // Get latest reborks
            try
            {
                topReBorks = await _borkApiAccess.GetTopReBorks();
                var dispReBorks = _mapper.Map<IList<ReBorkRecord>, IList<DisplayBork>>(topReBorks);
                mergedBorks.AddRange(dispReBorks);
            }
            catch (Exception e)
            {
                _logger.Error(e, "Failed to get top reborks.");
            }

            // Sort the display borks for the feed
            mergedBorks.Sort((x, y) => y.DateCreated.CompareTo(x.DateCreated));

            var model = new TopBorksViewModel
            {
                Username = username,
                TopBorks = mergedBorks
            };
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] BorkRecord bork)
        {
            _logger.Info("In post bork method");
            _logger.Info("Username from context = " + HttpContext.User.Identity.Name);
            bork.UserName = HttpContext.User.Identity.Name;
            await _borkApiAccess.CreateBork(bork);
            return Created("bork", bork);
        }

        [HttpPost]
        [Route("/rebork/{originalBorkId}")]
        public async Task<IActionResult> ReBork([FromRoute] int originalBorkId)
        {
            _logger.Info("In post rebork method");
            var username = HttpContext.User.Identity.Name;

            var originalBork = await _borkApiAccess.GetBorkById(originalBorkId);

            _logger.Info("Username from context = " + username);
            var reBork = new ReBorkRecord
            {
                UserName = username,
                OriginalBorkId = originalBorkId,
                OriginalUserName = originalBork.UserName,
                OriginalContent = originalBork.Content,
                DateCreated = DateTime.Now
            };
            var newReBork = await _borkApiAccess.CreateReBork(reBork);
            return Created("rebork", newReBork);
        }
    }
}
