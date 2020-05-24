namespace AnaCsharp.Dal.Interfaces.Dtos
{
    public class DeterminingWord : IDeterminingWord
    {
        public int DeterminingWordId { get; set; }

        public int WordId { get; set; }

        public IWord Word { get; set; }

        public int DeterminingStateId { get; set; }

        public IDeterminingState DeterminingState { get; set; }

        public int Order { get; set; }
    }
}
