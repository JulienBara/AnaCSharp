using System.Threading.Tasks;
using AnaCsharp.Dal.Interfaces.Repositories.Commands;
using AnaCSharp.DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace AnaCSharp.DAL.Repositories
{
    public class WordRepository : IWordCommandRepository, IWordQueryRepository
    {
        private readonly AnaContext _anaContext;

        public WordRepository(AnaContext anaContext)
        {
            _anaContext = anaContext;
        }

        public Task<int> AddWordAsync(string labels)
        {
            throw new System.NotImplementedException();
        }

        public async Task<int> GetWordIdByLabelAsync(string label)
        {
            var word = await _anaContext
                .Words
                .FirstOrDefaultAsync(x => x.Label == label);

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
