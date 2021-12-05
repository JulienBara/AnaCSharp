using System.Threading.Tasks;

namespace AnaCSharp.Bll.Interfaces.Services.Commands
{
    public interface ILearnCommandService
    {
        Task LearnAsync(string message, string previousMessage);
    }
}
