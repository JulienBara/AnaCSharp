using System.Collections.Generic;
using System.Linq;
using AnaCSharp.DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace AnaCSharp.DAL.Repositories
{
    public class DeterminedWordRepository
    {
        private readonly AnaContext _anaContext;
        private readonly WordRepository _wordRepository;
        private readonly DeterminingStateRepository _determiningStateRepository;

        public DeterminedWordRepository(
            AnaContext anaContext,
            WordRepository wordRepository,
            DeterminingStateRepository determiningStateRepository
        )
        {
            _anaContext = anaContext;
            _wordRepository = wordRepository;
            _determiningStateRepository = determiningStateRepository;
        }

        public void AddDeterminedWord(string label, int determiningStateId)
        {
            var wordId = _wordRepository.GetWordIdByLabel(label);

            var determinedWord = _anaContext
                .DeterminedWords
                .FirstOrDefault(x => x.DeterminingStateId == determiningStateId
                            && x.WordId == wordId);

            if (determinedWord == null)
            {
                var newDeterminedWord = new DeterminedWord
                {
                    DeterminingStateId = determiningStateId,
                    WordId = wordId,
                    Number = 1
                };

                _anaContext.DeterminedWords.Add(newDeterminedWord);
                _anaContext.SaveChanges();
                return;
            }

            determinedWord.Number = determinedWord.Number + 1;
            _anaContext.SaveChanges();
        }

        public List<DeterminedWord> FindDeterminedWords(List<string> lastWords)
        {
            var determiningStateId = _determiningStateRepository.GetDeterminingStateByLastWord(lastWords);

            var determiningState = _anaContext.DeterminingStates
                .Include(x => x.DeterminedWords)
                    .ThenInclude(x => x.Word)
                .FirstOrDefault(x => x.DeterminingStateId == determiningStateId);

            return determiningState.DeterminedWords.ToList();
        }
    }
}
