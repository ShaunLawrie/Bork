using System;

namespace Bork.Contracts
{
    public class BorkRecord
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }
    }
}
