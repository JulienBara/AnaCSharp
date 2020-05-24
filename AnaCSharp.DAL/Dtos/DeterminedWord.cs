namespace AnaCsharp.Dal.Interfaces.Dtos
{
    public class DeterminedWord : IDeterminedWord
    {
        public int DeterminedWordId { get; set; }

        public int DeterminingStateId { get; set; }

        public IDeterminingState DeterminingState { get; set; }

        public int WordId { get; set; }

        public IWord Word { get; set; }

        public int Number { get; set; }
    }
}
