using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnaCSharp.DAL.Model
{
    public class MaxMarkovDegree
    {
        [Key]
        public int MaxMarkovDegreeId { get; set; }

        public int Value { get; set; }
    }
}
