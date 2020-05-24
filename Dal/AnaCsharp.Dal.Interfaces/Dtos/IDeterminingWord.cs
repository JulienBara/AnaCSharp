namespace AnaCsharp.Dal.Interfaces.Dtos
{
    public interface IDeterminingWord
    {
        int DeterminingWordId { get; set; }

        int WordId { get; set; }

        IWord Word { get; set; }

        int DeterminingStateId { get; set; }

        IDeterminingState DeterminingState { get; set; }

        int Order { get; set; }
    }
}
