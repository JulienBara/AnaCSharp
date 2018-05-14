using System.Linq;
using AnaCSharp.DAL.Model;

namespace AnaCSharp.DAL.Repositories
{
    public class WordRepository
    {
        private readonly AnaContext _anaContext;

        public WordRepository(AnaContext anaContext)
        {
            _anaContext = anaContext;
        }

        public int GetWordIdByLabel(string label)
        {
            var word = _anaContext
                .Words
                .FirstOrDefault(x => x.Label == label);

            if (word == null)
            {
                var newWord = new Word
                {
                    Label = label
                };

                _anaContext
                    .Words
                    .Add(newWord);
                _anaContext.SaveChanges();

                return newWord.WordId;
            }

            return word.WordId;
        }
    }
}
