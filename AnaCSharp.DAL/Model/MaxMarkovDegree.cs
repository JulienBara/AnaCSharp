using System.ComponentModel.DataAnnotations;

namespace AnaCSharp.DAL.Model
{
    public class MaxMarkovDegree
    {
        [Key]
        public int MaxMarkovDegreeId { get; set; }

        public int MaxMarkovDegreeValue { get; set; }
    }
}
