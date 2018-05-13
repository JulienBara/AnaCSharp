using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnaCSharp.DAL.Model
{
    public class LogWord
    {
        [Key]
        public int LogWordId { get; set; }

        public int ChatId { get; set; }

        [ForeignKey("ChatId")]
        public virtual Chat Chat { get; set; }

        public int WordId { get; set; }

        [ForeignKey("WordId")]
        public virtual Word Word { get; set; }
    }
}
