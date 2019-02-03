using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnaCSharp.DAL.Model
{
    [Table("maxMarkovDegree")]
    public class MaxMarkovDegree
    {
        [Key]
        [Column("maxMarkovDegreeId")]
        public int MaxMarkovDegreeId { get; set; }

        [Column("maxMarkovDegree")]
        public int Value { get; set; }
    }
}
