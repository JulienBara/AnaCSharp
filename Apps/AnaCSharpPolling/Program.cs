using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace AnaCSharpPolling
{
    class Program
    {
        private static TelegramBotClient Bot;
        private static bool muted = false;

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            Bot = new TelegramBotClient(config["botToken"]);

            var me = Bot.GetMeAsync().Result;

            Bot.OnMessage += BotOnMessageReceived;

            Bot.StartReceiving(Array.Empty<UpdateType>());
            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();
            Bot.StopReceiving();
        }

        private static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;

            if (message == null || message.Type != MessageType.Text) return;

            switch (message.Text.Split(' ').First())
            {
                // Mute
                case "/mute":
                    muted = true;
                    await Bot.SendTextMessageAsync(message.Chat.Id, "Ana muted");
                    break;
                // Unmute
                case "/unmute":
                    muted = false;
                    await Bot.SendTextMessageAsync(message.Chat.Id, "Ana unmuted");
                    break;
                // Compute message
                default:
                    if (muted)
                        break;
                    var answer = "pouet"; // await _anaService.GenerateAnswerAsync(message.Text);
                    await Bot.SendTextMessageAsync(message.Chat.Id, answer, replyToMessageId: message.MessageId);
                    break;
            }
        }
    }
}
