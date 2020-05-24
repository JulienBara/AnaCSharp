using System.Threading.Tasks;

namespace AnaCSharp.DAL.Repositories
{
    public interface IWordQueryRepository
    {
        Task<int> GetWordIdByLabelAsync(string label);
    }
}
