using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnaCsharp.Dal.Interfaces.Dtos;
using AnaCSharp.Bll.Interfaces.Services.Commands;
using AnaCSharp.Bll.Interfaces.Services.Queries;
using AnaCSharp.DAL.Repositories;

namespace AnaCSharp.BLL.Services
{
    public class AnaService : IAnswerQueryService, ILearnCommandService
    {
        private readonly DeterminedWordRepository _determinedWordRepository;
        private readonly DeterminingStateRepository _determiningStateRepository;

        private int _markovDegree = 2;

        public AnaService(
            DeterminedWordRepository determinedWordRepository,
            DeterminingStateRepository determiningStateRepository)
        {
            _determinedWordRepository = determinedWordRepository;
            _determiningStateRepository = determiningStateRepository;
        }

        public Task LearnAsync(string message, IEnumerable<string> lastWords)
        {
            message += " EOM";
            var words = message.Split();
            var lastWordsqueue = new Queue(lastWords.ToArray());
            foreach (var word in words)
            {
                LearnAState(word, lastWords, _markovDegree);
                lastWordsqueue.Enqueue(word);
                lastWordsqueue.Dequeue();
            }
            return Task.CompletedTask;
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
                var determinedWords = await _determinedWordRepository.FindDeterminedWordsAsync(lastWords);
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

        public async void LearnAState(string word, IEnumerable<string> lastWords, int markovDegree)
        {
            if (lastWords.Count() == markovDegree)
            {
                var determiningStateId = await _determiningStateRepository.GetDeterminingStateByLastWordAsync(lastWords);
                await _determinedWordRepository.AddDeterminedWordAsync(word, determiningStateId);
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
