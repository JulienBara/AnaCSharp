using Telegram.Bot;

namespace AnaCSharpPolling
{
    class Program
    {
        private static TelegramBotClient Bot;

        static void Main(string[] args)
        {
            Bot = new TelegramBotClient("bot token");
        }
    }
}
