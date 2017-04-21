using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Bork.Contracts;
using Bork.Web.Models;
using NLog;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Bork.Web.Services;
using Bork.Web.Models.HomeViewModels;
using System;
using System.Collections.Generic;
using AutoMapper;
using Bork.Web.Models.BorkViewModels;

namespace Bork.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger _logger;
        private readonly IBorkApiAccessService _borkApiAccess;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger logger,
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
        public async Task<IActionResult> Index()
        {
            var username = HttpContext.User.Identity.Name;
            List<DisplayBork> mergedBorks = new List<DisplayBork>();
            IList<BorkRecord> topBorks = new List<BorkRecord>();
            IList<ReBorkRecord> topReBorks = new List<ReBorkRecord>();
            BorkStats stats = new BorkStats();

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
            // Get latest bork stats
            try
            {
                stats = await _borkApiAccess.GetBorkStats(username);
            }
            catch (Exception e)
            {
                _logger.Error(e, "Failed to get bork stats.");
            }

            // Sort the display borks for the feed
            mergedBorks.Sort((x, y) => y.DateCreated.CompareTo(x.DateCreated));

            var model = new HomeViewModel
            {
                Username = username,
                Borks = stats.Borks,
                ReBorks = stats.ReBorks,
                ReBorked = stats.ReBorked,
                TopBorksModel = new TopBorksViewModel
                {
                    Username = username,
                    TopBorks = mergedBorks
                }
            };

            return View(model);
        }
    }
}