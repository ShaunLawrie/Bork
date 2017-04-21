using Bork.Web.Models.HomeViewModels;
using System.Collections.Generic;

namespace Bork.Web.Models.BorkViewModels
{
    public class TopBorksViewModel
    {
        public string Username { get; set; }
        public IList<DisplayBork> TopBorks { get; set; }
    }
}
