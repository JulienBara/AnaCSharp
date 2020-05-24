using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AnaCSharp.DAL.Model
{
    public class DeterminingState
    {
        [Key]
        public int DeterminingStateId { get; set; }

        public virtual ICollection<DeterminedWord> DeterminedWords { get; set; }

        public virtual ICollection<DeterminingWord> DeterminingWords { get; set; }
    }
}
