namespace AnaCSharp.DAL.Model
{
    public class Word
    {
        public int WordId { get; set; }

        [StringLength(450)]
        public string Label { get; set; }
    }
}
