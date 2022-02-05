using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnaCSharp.DAL.Model
{
    public class DeterminingState
    {
        [Key]
        public int DeterminingStateId { get; set; }

        public int Word0Id { get; set; }

        [ForeignKey("Word0Id")]
        public virtual Word Word0 { get; set; }

        public int Word1Id { get; set; }

        [ForeignKey("Word1Id")]
        public virtual Word Word1 { get; set; }

        public int Word2Id { get; set; }

        [ForeignKey("Word2Id")]
        public virtual Word Word2 { get; set; }

        public virtual ICollection<DeterminedWord> DeterminedWords { get; set; }
    }
}
