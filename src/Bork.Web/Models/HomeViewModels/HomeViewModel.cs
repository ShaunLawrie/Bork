using Bork.Web.Models.BorkViewModels;

namespace Bork.Web.Models.HomeViewModels
{
    public class HomeViewModel
    {
        public string Username { get; set; }
        public TopBorksViewModel TopBorksModel { get; set; }
        public int Borks { get; set; }
        public int ReBorks { get; set; }
        public int ReBorked { get; set; }
    }
}
