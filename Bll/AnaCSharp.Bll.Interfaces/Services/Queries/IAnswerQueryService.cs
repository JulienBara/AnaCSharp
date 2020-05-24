using System.Threading.Tasks;

namespace AnaCSharp.Bll.Interfaces.Services.Queries
{
    public interface IAnswerQueryService
    {
        Task<string> GenerateAnswerAsync(string message);
    }
}
