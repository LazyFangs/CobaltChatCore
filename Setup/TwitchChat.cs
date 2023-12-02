using Microsoft.Extensions.Logging;
using System;
using System.Runtime.CompilerServices;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;

namespace CobaltChatCore
{
    
    class TwitchChat
    {
        public static TwitchClient Client = null;
        static ILogger __logger;

        public static void Setup(ILogger logger)
        {
            __logger = logger;
        }

        public async static Task ConnectToChat()
        {
            if (Client != null)
                Client.Disconnect();
            Client = null;

            if (!Configuration.Instance?.TokenValidated ?? false)
                throw new Exception("Configuration not created or lack of authorization!");

            ConnectionCredentials credentials = new ConnectionCredentials(Configuration.Instance.ChannelName, Configuration.Instance.AccessToken);
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 200,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };
            WebSocketClient customClient = new WebSocketClient(clientOptions);
            Client = new TwitchClient(customClient);
            Client.Initialize(credentials, Configuration.Instance.ChannelName);

            Client.OnLog += Client_OnLog;
            Client.OnJoinedChannel += Client_OnJoinedChannel;
            Client.OnMessageReceived += Client_OnMessageReceived;
            Client.OnConnected += Client_OnConnected;
            Client.OnConnectionError += Client_OnConnectionError;
            Client.OnIncorrectLogin += Client_OnIncorrectLogin;

            __logger?.LogInformation($"Attempting to connect to TwitchChat...");
            Client.Connect();
            
        }

        public static void SendMessageToChat(string message)
        {
            Client.SendMessage(Configuration.Instance.ChannelName, message);
        }

        private static void Client_OnIncorrectLogin(object? sender, OnIncorrectLoginArgs e)
        {
            __logger?.LogError($"Failed to join{e.Exception.Username} due to {e.Exception.Message}");
        }

        private static void Client_OnConnectionError(object? sender, OnConnectionErrorArgs e)
        {
            __logger?.LogError(e.Error.Message);
        }

        private static void Client_OnLog(object sender, OnLogArgs e)
        {
            string toLog = e.Data;
            if (e.Data.Contains("PRIVMSG #"))
                toLog = e.Data.Substring(e.Data.IndexOf("PRIVMSG"));
            if (e.Data.Contains("USERSTATE #"))
                return;
                __logger?.LogInformation($"{e.DateTime.ToString()}: {e.BotUsername} - {toLog}");
        }

        private static void Client_OnConnected(object sender, OnConnectedArgs e)
        {
            __logger?.LogInformation($"Connected to Twitch!");
        }

        private static void Client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            __logger?.LogInformation($"Bot joined channel {e.Channel}");
            Client.SendMessage(e.Channel, Configuration.OnSetupCompleteText);
            Client.SendMessage(e.Channel,
                Configuration.Instance.RemindersText.Replace("{JoinCommand}", Configuration.Instance.CommandSignal + Configuration.Instance.JoinCommand)
                );
        }

        private static void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            
        }


       
    }
}