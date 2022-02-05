using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnaCsharp.Dal.Interfaces.Repositories.Commands;
using AnaCsharp.Dal.Interfaces.Repositories.Queries;
using AnaCSharp.DAL.Model;

namespace AnaCSharp.DAL.Repositories
{
    public class DeterminingStateRepository : IDeterminingStateCommandRepository, IDeterminingStateQueryRepository
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

        public Task<int> AddDeterminingStateAsync(IEnumerable<string> lastWords)
        {
            throw new System.NotImplementedException();
        }

        public async Task<int> GetDeterminingStateByLastWordAsync(IEnumerable<string> lastWords)
        {

            var determiningState = _anaContext
                .DeterminingStates
                .FirstOrDefault(x =>
                    x.Word0.Label == lastWords.ElementAt(0)
                    && x.Word1.Label == lastWords.ElementAt(1)
                    && x.Word2.Label == lastWords.ElementAt(2)
                );

            // if doesn't exist add
            if (determiningState == null)
            {
                var newDeterminingState = new DeterminingState
                {
                    Word0Id = await _wordRepository.GetWordIdByLabelAsync(lastWords.ElementAt(0)),
                    Word1Id = await _wordRepository.GetWordIdByLabelAsync(lastWords.ElementAt(1)),
                    Word2Id = await _wordRepository.GetWordIdByLabelAsync(lastWords.ElementAt(2)),
                };
                _anaContext.DeterminingStates.Add(newDeterminingState);

                _anaContext.SaveChanges();

                return newDeterminingState.DeterminingStateId;
            }

            // else get the id
            return determiningState.DeterminingStateId;
        }
    }
}
