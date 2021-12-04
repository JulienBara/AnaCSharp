using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace AnaCSharpPolling
{
    class Program
    {
        private static bool muted = false;

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true);

            IConfiguration config = builder.Build();

            var botClient = new TelegramBotClient(config["botToken"]);

            using var cts = new CancellationTokenSource();

            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { } // receive all update types
            };
            botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken: cts.Token);

            var me = botClient.GetMeAsync().Result;

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();

            // Send cancellation request to stop bot
            cts.Cancel();
        }

        private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Only process Message updates: https://core.telegram.org/bots/api#message
            if (update.Type != UpdateType.Message) return;
            // Only process text messages
            if (update.Message!.Type != MessageType.Text) return;

            var chatId = update.Message.Chat.Id;
            var messageId = update.Message.MessageId;
            var messageText = update.Message.Text;

            switch (messageText.Split(' ').First())
            {
                // Mute
                case "/mute":
                    muted = true;
                    await botClient.SendTextMessageAsync(chatId, "Ana muted");
                    break;
                // Unmute
                case "/unmute":
                    muted = false;
                    await botClient.SendTextMessageAsync(chatId, "Ana unmuted");
                    break;
                // Compute message
                default:
                    if (muted)
                        break;
                    var answer = "pouet"; // await _anaService.GenerateAnswerAsync(message.Text);
                    await botClient.SendTextMessageAsync(chatId, answer, cancellationToken: cancellationToken, replyToMessageId: messageId);
                    break;
            }
        }

        private static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}
