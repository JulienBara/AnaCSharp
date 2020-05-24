using System.Threading.Tasks;

namespace AnaCsharp.Dal.Interfaces.Repositories.Commands
{
    public interface IDeterminedWordCommandRepository
    {
        Task<int> AddDeterminedWordAsync(string label, int determiningStateId);
    }
}
