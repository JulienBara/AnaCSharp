using AnaCSharp.DAL.Model;

namespace AnaCSharp.DAL.Repositories
{
    public class LogWordRepository
    {
        private readonly AnaContext _anaContext;
        private readonly WordRepository _wordRepository;

        public LogWordRepository(
            AnaContext anaContext,
            WordRepository wordRepository
            )
        {
            _anaContext = anaContext;
            _wordRepository = wordRepository;
        }

        public void SaveLogWord(int chatId, string label)
        {
            var newLogWord = new LogWord
            {
                ChatId = chatId,
                WordId = _wordRepository.GetWordIdByLabel(label)
            };

            _anaContext.LogWords.Add(newLogWord);
            _anaContext.SaveChanges();
        }
    }
}
