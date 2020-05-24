namespace AnaCsharp.Dal.Interfaces.Dtos
{
    public interface IDeterminedWord
    {
        int DeterminedWordId { get; set; }

        int DeterminingStateId { get; set; }

        IDeterminingState DeterminingState { get; set; }

        int WordId { get; set; }

        IWord Word { get; set; }

        int Number { get; set; }
    }
}
