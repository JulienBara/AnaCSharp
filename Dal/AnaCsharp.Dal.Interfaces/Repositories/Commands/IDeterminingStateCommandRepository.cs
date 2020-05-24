using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnaCsharp.Dal.Interfaces.Repositories.Commands
{
    public interface IDeterminingStateCommandRepository
    {
        Task<int> AddDeterminingStateAsync(IEnumerable<string> lastWords);
    }
}
