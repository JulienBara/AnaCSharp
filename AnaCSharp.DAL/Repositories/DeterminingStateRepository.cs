using System.Collections.Generic;
using System.Linq;
using AnaCSharp.DAL.Model;

namespace AnaCSharp.DAL.Repositories
{
    public class DeterminingStateRepository
    {
        private readonly AnaContext _anaContext;
        private readonly WordRepository _wordRepository;

        public DeterminingStateRepository(
            AnaContext anaContext,
            WordRepository wordRepository
            )
        {
            _anaContext = anaContext;
            _wordRepository = wordRepository;
        }

        public int GetDeterminingStateByLastWord(List<string> lastWords)
        {
            // get markov degree
            var n = lastWords.Count;

            // forge query
            var query = _anaContext
                .DeterminingStates
                .AsQueryable();

            for (var i = 0; i < n; i++)
            {
                query = query.Where(x => x.DeterminingWords
                        .Any(y => y.Word.Label == lastWords[i]
                                && y.Order == n - i - 1));
            }

            var determiningState = query.FirstOrDefault();

            // if doesn't exist add
            if (determiningState == null)
            {
                var determiningWords = new List<DeterminingWord>();
                for (var i = 0; i < n; i++)
                {
                    var newDeterminingWord = new DeterminingWord
                    {
                        WordId = _wordRepository.GetWordIdByLabel(lastWords[i]),
                        Order = n - i - 1
                    };
                    determiningWords.Add(newDeterminingWord);
                }

                var newDeterminingState = new DeterminingState
                {
                    DeterminingWords = determiningWords
                };

                _anaContext.SaveChanges();

                return newDeterminingState.DeterminingStateId;
            }

            // else get the id
            return determiningState.DeterminingStateId;
        }
    }
}
