using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnaCSharp.Bll.Interfaces.Services.Commands
{
    public interface ILearnCommandService
    {
        Task LearnAsync(string message, IEnumerable<string> lastWords);
    }
}
 