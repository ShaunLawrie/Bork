using Bork.Contracts;
using System.Collections.Generic;

namespace Bork.Api.Repositories
{
    public interface IBorkRepository
    {
        IList<BorkRecord> TopBorks(int limit);
        IList<ReBorkRecord> TopReBorks(int limit);
        BorkRecord AddBork(BorkRecord bork);
        ReBorkRecord AddReBork(ReBorkRecord reBork);
        BorkRecord GetBorkById(int id);
        ReBorkRecord GetReBorkById(int id);
        int BorkCount(string username);
        int ReBorkCount(string username);
        int ReBorkedCount(string username);
    }
}