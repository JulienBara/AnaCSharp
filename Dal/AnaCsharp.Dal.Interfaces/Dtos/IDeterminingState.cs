using System.Collections.Generic;

namespace AnaCsharp.Dal.Interfaces.Dtos
{
    public interface IDeterminingState
    {
        int DeterminingStateId { get; set; }

        ICollection<IDeterminedWord> DeterminedWords { get; set; }
    }
}
