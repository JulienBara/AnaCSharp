using AnaCsharp.Dal.Interfaces.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnaCsharp.Dal.Interfaces.Repositories.Queries
{
    public interface IDeterminedWordQueryRepository
    {
        Task<IEnumerable<IDeterminedWord>> FindDeterminedWordsAsync(IEnumerable<string> lastWords);
    }
}
