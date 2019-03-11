﻿using AnaCSharp.BLL.Services;
using AnaCSharp.DAL;
using AnaCSharp.DAL.Repositories;
using System;
using System.Linq;
using AnaCSharp.DAL.Model;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Unity;

namespace AnaTelegramBotClient
{
    class Program
    {
        private static readonly TelegramBotClient Bot = new TelegramBotClient("your key");
        private static AnaService _anaService;
        private static AnaContext _anaContext;
        private static bool muted = false;

        static void Main(string[] args)
        {
            // prepare AnaService
            IUnityContainer container = new UnityContainer();
            //container.AddExtension(new Diagnostic());

            container.RegisterType<AnaContext>();
            _anaContext = container.Resolve<AnaContext>();
            _anaContext.Database.Migrate();
            var maxMarkovDegree = _anaContext.MaxMarkovDegrees.ToList().FirstOrDefault();
            if (maxMarkovDegree == null)
            {
                _anaContext.MaxMarkovDegrees.Add(new MaxMarkovDegree { Value = 3 });
                _anaContext.SaveChanges();
            }

            container.RegisterType<AnaService>();
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
                    muted = false;
                    Bot.SendTextMessageAsync(message.Chat.Id, "Ana unmuted");
                    break;
                // Compute message
                default:
                    if (muted)
                        break;
                    Bot.SendTextMessageAsync(message.Chat.Id, _anaService.GenerateAnswer(message.Text), replyToMessageId: message.MessageId);
                    break;
            }
        }
    }
}
