using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnaCSharp.DAL.Repositories
{
    public interface IDeterminingStateQueryRepository
    {
        Task<int> GetDeterminingStateByLastWordAsync(IEnumerable<string> lastWords);
    }
}
