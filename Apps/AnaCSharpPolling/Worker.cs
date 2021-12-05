using AnaCSharp.Bll.Interfaces.Services.Commands;
using AnaCSharp.Bll.Interfaces.Services.Queries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
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
    public class Worker : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<Worker> _logger;
        private readonly IAnswerQueryService _anaQueryService;
        private readonly ILearnCommandService _anaCommandService;
        private bool muted = true;

        public Worker(
            IConfiguration configuration,
            ILogger<Worker> logger,
            IAnswerQueryService anaQueryService,
            ILearnCommandService anaCommandService
            )
        {
            _configuration = configuration;
            _logger = logger;
            _anaQueryService = anaQueryService;
            _anaCommandService = anaCommandService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var botClient = new TelegramBotClient(_configuration["botToken"]);

            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { } // receive all update types
            };
            botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken: stoppingToken);

            var me = await botClient.GetMeAsync();

            _logger.Log(LogLevel.Information, $"Start listening for @{me.Username}");
        }


        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
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
                    if (! muted)
                    {
                        var answer = await _anaQueryService.GenerateAnswerAsync(messageText);
                        if (answer != "")
                            await botClient.SendTextMessageAsync(chatId, answer, cancellationToken: cancellationToken, replyToMessageId: messageId);
                    }

                    if (update.Message.ReplyToMessage.Text != null)
                        await _anaCommandService.LearnAsync(messageText, update.Message.ReplyToMessage.Text);
                    break;
            }
        }

        Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            _logger.Log(LogLevel.Error, ErrorMessage);
            return Task.CompletedTask;
        }
    }
}
