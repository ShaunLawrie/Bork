using System;

namespace Bork.Web.Models.HomeViewModels
{
    public class DisplayBork
    {
        // General
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string UserName { get; set; }
        public string Type { get; set; }

        // For borks
        public string Content { get; set; }

        // For reborks
        public int OriginalBorkId { get; set; }
        public string OriginalUserName { get; set; }
        public string OriginalContent { get; set; }
    }
}
