using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnaCsharp.Dal.Interfaces.Dtos;
using AnaCsharp.Dal.Interfaces.Repositories.Commands;
using AnaCsharp.Dal.Interfaces.Repositories.Queries;
using AnaCSharp.Bll.Interfaces.Services.Commands;
using AnaCSharp.Bll.Interfaces.Services.Queries;

namespace AnaCSharp.BLL.Services
{
    public class AnaService : IAnswerQueryService, ILearnCommandService
    {
        private readonly IDeterminedWordCommandRepository _determinedWordCommandRepository;
        private readonly IDeterminedWordQueryRepository _determinedWordQueryRepository;
        private readonly IDeterminingStateQueryRepository _determiningStateQueryRepository;

        private int _markovDegree = 2;

        public AnaService(
            IDeterminedWordCommandRepository determinedWordCommandRepository,
            IDeterminedWordQueryRepository determinedWordQueryRepository,
            IDeterminingStateQueryRepository determiningStateQueryRepository)
        {
            _determinedWordCommandRepository = determinedWordCommandRepository;
            _determinedWordQueryRepository = determinedWordQueryRepository;
            _determiningStateQueryRepository = determiningStateQueryRepository;
        }

        public async Task LearnAsync(string message, string previousMessage)
        {
            message += " EOM";
            previousMessage += " EOM";

            var words = message.Split();
            var lastWordsQueue = new Queue<string>(previousMessage.Split().TakeLast(_markovDegree).ToList());
            foreach (var word in words)
            {
                await LearnAStateAsync(word, lastWordsQueue, _markovDegree);
                lastWordsQueue.Enqueue(word);
                lastWordsQueue.Dequeue();
            }
        }

        public async Task<string> GenerateAnswerAsync(string message)
        {
            message += " EOM";
            var words = message.Split(' ');

            if (words.Length <= _markovDegree)
                return "";

            var lastWords = words.Skip(words.Length - _markovDegree).ToList();

            var retMessage = "";

            var lastWordsqueue = new Queue(lastWords.ToArray());

            while (true)
            {
                var determinedWords = await _determinedWordQueryRepository.FindDeterminedWordsAsync(lastWords);
                if (!determinedWords.Any())
                    break;
                var bestweightedMessage = GetBestWeightedRandomMessage(determinedWords);
                if (bestweightedMessage == "EOM")
                    break;
                retMessage += " " + bestweightedMessage;
                lastWordsqueue.Enqueue(bestweightedMessage);
                lastWordsqueue.Dequeue();
            }

            return retMessage;
        }

        public async Task LearnAStateAsync(string word, IEnumerable<string> lastWords, int markovDegree)
        {
            if (lastWords.Count() == markovDegree)
            {
                var determiningStateId = await _determiningStateQueryRepository.GetDeterminingStateByLastWordAsync(lastWords);
                await _determinedWordCommandRepository.AddDeterminedWordAsync(word, determiningStateId);
            }
        }

        public string GetBestWeightedRandomMessage(IEnumerable<IDeterminedWord> listWeightMessages)
        {
            var weightSum = 0;

            foreach (var weightMessage in listWeightMessages)
            {
                weightSum += weightMessage.Number;
            }

            Random rnd = new Random();
            int rand = rnd.Next(0, weightSum);

            weightSum = 0;

            foreach (var weightMessage in listWeightMessages)
            {
                weightSum += weightMessage.Number;
                if (weightSum >= rand)
                {
                    return weightMessage.Word.Label;
                }
            }

            // shouldn't be reached
            return "";
        }
    }
}
