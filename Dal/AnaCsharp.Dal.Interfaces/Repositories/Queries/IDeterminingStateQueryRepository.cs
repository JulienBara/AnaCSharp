using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnaCsharp.Dal.Interfaces.Repositories.Queries
{
    public interface IDeterminingStateQueryRepository
    {
        Task<int> GetDeterminingStateByLastWordAsync(IEnumerable<string> lastWords);
    }
}
