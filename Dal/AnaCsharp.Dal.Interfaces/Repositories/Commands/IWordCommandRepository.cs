using System.Threading.Tasks;

namespace AnaCsharp.Dal.Interfaces.Repositories.Commands
{
    public interface IWordCommandRepository
    {
        Task<int> AddWordAsync(string labels);
    }
}
