using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnaCSharp.DAL.Model
{
    //[Table("maxMarkovDegree")]
    public class MaxMarkovDegree
    {
        [Key]
        public int MaxMarkovDegreeId { get; set; }

        //[Column("maxMarkovDegree")]
        public int MaxMarkovDegreeValue { get; set; }
    }
}
