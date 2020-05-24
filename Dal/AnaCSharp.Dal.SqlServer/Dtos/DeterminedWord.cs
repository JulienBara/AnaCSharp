namespace AnaCsharp.Dal.Interfaces.Dtos
{
    public class DeterminedWord : IDeterminedWord
    {

        public IWord Word { get; set; }

        public int Number { get; set; }
    }
}
