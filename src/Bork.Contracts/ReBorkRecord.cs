using System;

namespace Bork.Contracts
{
    public class ReBorkRecord
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string UserName { get; set; }
        public int OriginalBorkId { get; set; }
        public string OriginalUserName { get; set; }
        public string OriginalContent { get; set; }
    }
}
