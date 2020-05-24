using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnaCsharp.Dal.Interfaces.Dtos;
using AnaCSharp.DAL.Repositories;

namespace AnaCSharp.BLL.Services
{
    public class AnaService
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

        public void InsertNewWordInList(ref List<string> list, string word, int markovDegree)
        {
            list.Add(word);
            if (list.Count > markovDegree)
            {
                list.RemoveAt(0);
            }
        }

        public void Learn(string message, ref List<string> lastWords)
        {
            message += " EOM";
            var words = message.Split();
            foreach (var word in words)
            {
                LearnAState(word, lastWords, _markovDegree);
                InsertNewWordInList(ref lastWords, word, _markovDegree);
            }
        }

        public async Task<string> GenerateAnswer(string message)
        {
            message += " EOM";
            var words = message.Split(' ');

            if (words.Length <= _markovDegree)
                return "";

            var lastWords = words.Skip(words.Length - _markovDegree).ToList();

            var retMessage = "";

            while(true)
            {
                var determinedWords =  await _determinedWordRepository.FindDeterminedWordsAsync(lastWords);
                if (!determinedWords.Any())
                    break;
                var bestweightedMessage = GetBestWeightedRandomMessage(determinedWords);
                if (bestweightedMessage == "EOM")
                    break;
                retMessage += " " + bestweightedMessage;
                InsertNewWordInList(ref lastWords, bestweightedMessage, _markovDegree);
            }

            return retMessage;
        }

        public async void LearnAState(string word, List<string> lastWords, int markovDegree)
        {
            if (lastWords.Count == markovDegree)
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
