using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnaCsharp.Dal.Interfaces.Dtos;
using AnaCsharp.Dal.Interfaces.Repositories.Commands;
using AnaCsharp.Dal.Interfaces.Repositories.Queries;
using Microsoft.EntityFrameworkCore;

namespace AnaCSharp.DAL.Repositories
{
    public class DeterminedWordRepository : IDeterminedWordCommandRepository, IDeterminedWordQueryRepository
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

        public async Task<int> AddDeterminedWordAsync(string label, int determiningStateId)
        {
            var wordId = await _wordRepository.GetWordIdByLabelAsync(label);

            var determinedWord = _anaContext
                .DeterminedWords
                .FirstOrDefault(x => x.DeterminingStateId == determiningStateId
                            && x.WordId == wordId);

            if (determinedWord == null)
            {
                var newDeterminedWord = new Model.DeterminedWord
                {
                    DeterminingStateId = determiningStateId,
                    WordId = wordId,
                    Number = 1
                };

                _anaContext.DeterminedWords.Add(newDeterminedWord);
                await _anaContext.SaveChangesAsync();
                return newDeterminedWord.DeterminedWordId;
            }

            determinedWord.Number = determinedWord.Number + 1;
            await _anaContext.SaveChangesAsync();
            return determinedWord.DeterminedWordId;
        }

        public async Task<IEnumerable<IDeterminedWord>> FindDeterminedWordsAsync(IEnumerable<string> lastWords)
        {
            var determiningStateId = await _determiningStateRepository.GetDeterminingStateByLastWordAsync(lastWords);

            return _anaContext.DeterminedWords
                .Where(x => x.DeterminingStateId == determiningStateId)
                .Include(x => x.Word)
                .Select(x => new DeterminedWord
                {
                    Number = x.Number,
                    Word = new Word
                    {
                        WordId = x.Word.WordId,
                        Label = x.Word.Label
                    }
                });
        }
    }
}
