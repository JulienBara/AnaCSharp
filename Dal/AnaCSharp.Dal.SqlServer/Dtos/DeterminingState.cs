using System.Collections.Generic;

namespace AnaCsharp.Dal.Interfaces.Dtos
{
    public class DeterminingState : IDeterminingState
    {
        public int DeterminingStateId { get; set; }

        public ICollection<IDeterminedWord> DeterminedWords { get; set; }
    }
}
