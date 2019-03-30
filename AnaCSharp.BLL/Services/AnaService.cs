using System;
using System.Collections.Generic;
using System.Linq;
using AnaCSharp.DAL;
using AnaCSharp.DAL.Model;
using AnaCSharp.DAL.Repositories;

namespace AnaCSharp.BLL.Services
{
    public class AnaService
    {
        private readonly AnaContext _anaContext;
        private readonly DeterminedWordRepository _determinedWordRepository;
        private readonly DeterminingStateRepository _determiningStateRepository;

        private readonly Dictionary<int, List<string>> _lastWordsDictionary = new Dictionary<int, List<string>>();

        private int _markovDegree = 2;

        public AnaService(
            AnaContext anaContext,
            DeterminedWordRepository determinedWordRepository,
            DeterminingStateRepository determiningStateRepository)
        {
            _anaContext = anaContext;
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

        public string GenerateAnswer(string message)
        {
            message += " EOM";
            var words = message.Split(' ');

            if (words.Length <= _markovDegree)
                return "";

            var lastWords = words.Skip(words.Length - _markovDegree).ToList();

            var retMessage = "";

            while(true)
            {
                var determinedWords =  _determinedWordRepository.FindDeterminedWords(lastWords);
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

        public void LearnAState(string word, List<string> lastWords, int markovDegree)
        {
            if (lastWords.Count == markovDegree)
            {
                var determiningStateId = _determiningStateRepository.GetDeterminingStateByLastWord(lastWords);
                _determinedWordRepository.AddDeterminedWord(word, determiningStateId);
            }
        }

        public string GetBestWeightedRandomMessage(List<DeterminedWord> listWeightMessages)
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
