using System.Threading.Tasks;
using Bork.Contracts;
using System.Collections.Generic;

namespace Bork.Web.Services
{
    public interface IBorkApiAccessService
    {
        Task<IList<BorkRecord>> GetTopBorks();
        Task<IList<ReBorkRecord>> GetTopReBorks();
        Task<BorkRecord> GetBorkById(int id);
        Task<BorkRecord> CreateBork(BorkRecord bork);
        Task<ReBorkRecord> CreateReBork(ReBorkRecord reBork);
        Task<BorkStats> GetBorkStats(string username);
    }
}