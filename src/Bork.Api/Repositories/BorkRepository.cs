using Bork.Api.Data;
using Bork.Api.Helpers;
using Bork.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Bork.Api.Repositories
{
    public class BorkRepository : IBorkRepository
    {
        private readonly BorkDbContext _borks;

        public BorkRepository(BorkDbContext borks)
        {
            _borks = borks;
        }

        public IList<BorkRecord> TopBorks(int limit)
        {
            var qt = new QueryTimer("TopBorks");

            var result =  _borks.Borks
                .OrderByDescending(b => b.Id)
                .Take(limit)
                .ToList();

            qt.LogQueryTime();

            return result;
        }

        public IList<ReBorkRecord> TopReBorks(int limit)
        {
            var qt = new QueryTimer("TopReBorks");

            var result =  _borks.ReBorks
                .OrderByDescending(b => b.Id)
                .Take(limit)
                .ToList();

            qt.LogQueryTime();

            return result;
        }

        public BorkRecord AddBork(BorkRecord bork)
        {
            var qt = new QueryTimer("AddBork");

            bork.Id = 0; // force unset id
            var newBork = _borks.Add(bork);
            _borks.SaveChanges();

            qt.LogQueryTime();

            return newBork.Entity;
        }

        public ReBorkRecord AddReBork(ReBorkRecord reBork)
        {
            var qt = new QueryTimer("AddReBork");

            reBork.Id = 0; // force unset id
            var newBork = _borks.Add(reBork);
            _borks.SaveChanges();

            qt.LogQueryTime();

            return newBork.Entity;
        }

        public BorkRecord GetBorkById(int id)
        {
            var qt = new QueryTimer("GetBorkById");
            var result = _borks.Borks.First(b => b.Id == id);
            qt.LogQueryTime();

            return result;
        }

        public ReBorkRecord GetReBorkById(int id)
        {
            var qt = new QueryTimer("GetReBorkById");
            var result = _borks.ReBorks.First(b => b.Id == id);
            qt.LogQueryTime();

            return result;
        }

        public int BorkCount(string username)
        {
            var qt = new QueryTimer("BorkCount");
            var result = _borks.Borks.Count(b => b.UserName == username);
            qt.LogQueryTime();

            return result;
        }

        public int ReBorkCount(string username)
        {
            var qt = new QueryTimer("ReBorkCount");
            var result = _borks.ReBorks.Count(b => b.UserName == username);
            qt.LogQueryTime();

            return result;
        }

        public int ReBorkedCount(string username)
        {
            var qt = new QueryTimer("ReBorkedCount");
            var result = _borks.ReBorks.Count(b => b.OriginalUserName == username);
            qt.LogQueryTime();

            return result;
        }
    }
}
