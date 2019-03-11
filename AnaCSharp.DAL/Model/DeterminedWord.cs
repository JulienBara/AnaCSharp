using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnaCSharp.DAL.Model
{
    public class DeterminedWord
    {
        [Key]
        public int DeterminedWordId { get; set; }

        public int DeterminingStateId { get; set; }

        [ForeignKey("DeterminingStateId")]
        public virtual DeterminingState DeterminingState { get; set; }

        public int WordId { get; set; }

        [ForeignKey("WordId")]
        public virtual Word Word { get; set; }

        public int Number { get; set; }
    }
}
