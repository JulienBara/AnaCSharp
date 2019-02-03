using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnaCSharp.DAL.Model
{
    [Table("determiningWords")]
    public class DeterminingWord
    {
        [Key]
        [Column("determiningWordId")]
        public int DeterminingWordId { get; set; }

        [Column("wordId")]
        public int WordId { get; set; }

        [ForeignKey("WordId")]
        public virtual Word Word { get; set; }

        [Column("determiningStateId")]
        public int DeterminingStateId { get; set; }

        [ForeignKey("DeterminingStateId")]
        public virtual DeterminingState DeterminingState { get; set; }

        [Column("order")]
        public int Order { get; set; }
    }
}
