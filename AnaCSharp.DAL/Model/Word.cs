using System.ComponentModel.DataAnnotations.Schema;

namespace AnaCSharp.DAL.Model
{
    [Table("words")]
    public class Word
    {
        [Column("wordId")]
        public int WordId { get; set; }

        [Column("label")]
        public string Label { get; set; }
    }
}
