using System.Threading.Tasks;

namespace AnaCSharp.DAL.Repositories
{
    public interface IWordRepository
    {
        Task<int> GetWordIdByLabel(string label);
    }
}
