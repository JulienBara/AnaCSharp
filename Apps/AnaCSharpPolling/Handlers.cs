using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace AnaCSharpPolling
{
    public static class Handlers
    {
        public static bool muted = false;

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
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

        public static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
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
