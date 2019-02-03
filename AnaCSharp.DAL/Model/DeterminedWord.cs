using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnaCSharp.DAL.Model
{
    [Table("determinedWords")]
    public class DeterminedWord
    {
        [Key]
        [Column("determinedWordId")]
        public int DeterminedWordId { get; set; }

        [Column("determinedStateId")]
        public int DeterminingStateId { get; set; }

        [ForeignKey("DeterminingStateId")]
        public virtual DeterminingState DeterminingState { get; set; }

        [Column("wordId")]
        public int WordId { get; set; }

        [ForeignKey("WordId")]
        public virtual Word Word { get; set; }

        [Column("number")]
        public int Number { get; set; }
    }
}
