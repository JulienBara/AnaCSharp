﻿using AnaCSharp.BLL.Services;
using AnaCSharp.DAL;
using AnaCSharp.DAL.Repositories;
using System;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Unity;

namespace AnaTelegramBotClient
{
    class Program
    {
        private static readonly TelegramBotClient Bot = new TelegramBotClient("your key");
        private static AnaService _anaService = null;
        private static bool muted = false;

        static void Main(string[] args)
        {
            // prepare AnaService
            IUnityContainer container = new UnityContainer();
            container.AddExtension(new Diagnostic());

            container.RegisterType<AnaService>();
            container.RegisterType<AnaContext>();
            container.RegisterType<DeterminedWordRepository>();
            container.RegisterType<DeterminingStateRepository>();

            _anaService = container.Resolve<AnaService>();


            var me = Bot.GetMeAsync().Result;
            Console.Title = me.Username;

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
                    Bot.SendTextMessageAsync(message.Chat.Id, "Ana muted");
                    break;
                // Unmute
                case "/unmute":
                    muted = true;
                    Bot.SendTextMessageAsync(message.Chat.Id, "Ana unmuted");
                    break;
                default:
                    // Compute message
                    if (muted)
                        break;
                    Bot.SendTextMessageAsync(message.Chat.Id, _anaService.GenerateAnswer(message.Text), replyToMessageId: message.MessageId); 
                    break;
            }
        }
    }
}