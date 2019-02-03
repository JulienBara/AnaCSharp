using System;
using System.Collections.Generic;
using System.Linq;
using AnaCSharp.DAL.Model;

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

        public List<Tuple<string,int>> FindDeterminedWords(List<string> lastWords)
        {
            var determiningStateId = _determiningStateRepository.GetDeterminingStateByLastWord(lastWords);

            var determiningState = _anaContext.DeterminingStates.Find(determiningStateId);

            var pairs = new List<Tuple<string, int>>();

            foreach (var word in determiningState.DeterminedWords.ToList())
            {
                pairs.Add(new Tuple<string, int>(word.Word.Label, word.Number));
            }

            return pairs;
        }
    }
}
