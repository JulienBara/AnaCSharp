using System.ComponentModel.DataAnnotations;

namespace AnaCSharp.DAL.Model
{
    public class Chat
    {
        [Key]
        public int ChatId { get; set; }

        public string Label { get; set; }
    }
}
