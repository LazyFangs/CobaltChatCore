using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using TwitchLib.Api.Helix.Models.Charity;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using static CobaltChatCore.Configuration;

namespace CobaltChatCore
{
    public class CommandManager
    {

        public class TwitchCommand
        {

            public TwitchCommand(Action<OnMessageReceivedArgs> value) : this(AccessLevel.EVERYONE, value)
            {

            }

            public TwitchCommand(AccessLevel a, Action<OnMessageReceivedArgs> value)
            {
                this.action = value;
                this.AllowedLevel = a;
            }

            public enum AccessLevel { EVERYONE = 0, VIP, MOD, STREAMER, SPECIAL }
            public AccessLevel AllowedLevel { get; set; }

            public Action<OnMessageReceivedArgs> action { private get; set; }

            public void Invoke(OnMessageReceivedArgs e)
            {
                bool allowed = false;
                if (AllowedLevel == AccessLevel.SPECIAL && Configuration.SpecialPeople.Contains(e.ChatMessage.DisplayName))
                    allowed = true;
                else
                {
                    int level = e.ChatMessage.IsBroadcaster ? (int)AccessLevel.STREAMER : e.ChatMessage.IsVip ? (int)AccessLevel.VIP : e.ChatMessage.IsModerator ? (int)AccessLevel.MOD : (int)AccessLevel.EVERYONE;
                    allowed = level >= (int)AllowedLevel;
                }

                if (allowed) 
                    action?.Invoke(e);
                else
                    TwitchChat.SendMessageToChat(Configuration.Instance.InsufficientPrivledgeText);
            }
        }

        public static Dictionary<string, TwitchCommand> commands = new Dictionary<string, TwitchCommand>();
        ConcurrentQueue<Action<Combat>> combatActions = new ConcurrentQueue<Action<Combat>>();

        /// <summary>
        /// displayname -> number of times they were picked;
        /// </summary>
        public static Dictionary<string, int> ChattersAvailable = new Dictionary<string, int>();
        public static Dictionary<string, Color> ChatterColors = new Dictionary<string, Color>();
        List<string> bannedChatters = new List<string>();
       
        public bool queueOpen = true;
        bool inCombat = false;

        ILogger logger;

        public CommandManager()
        {
            AddCommands();
        }

        public void Setup(ILogger log)
        {
            logger = log;
            
            TwitchChat.Client.OnMessageReceived += OnChatMessageReceived;

            CobaltChatCoreManifest.EventHub.ConnectToEvent<string>(CobaltChatCoreManifest.SelectEnemyChatterEvent, (st) => EnemyChatterSelected(st));
            CobaltChatCoreManifest.EventHub.ConnectToEvent<string>(CobaltChatCoreManifest.EnterRouteEvent, (s) => TryEndCombat(s));
            CobaltChatCoreManifest.EventHub.ConnectToEvent<Combat>(CobaltChatCoreManifest.UpdateCombatEvent, (c) => OnCombatUpdate(c));

        }

        void EnemyChatterSelected(string user)
        {
            ChattersAvailable[user] += 1;
            inCombat = true;
        }

        public void OnChatMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            string[] parts = e.ChatMessage.Message.Split(" ");
            string command = parts[0];
            if (command.StartsWith(Configuration.Instance.CommandSignal) && commands.TryGetValue(command.Substring(1), out TwitchCommand action))
                action.Invoke(e);
        }

        public void TryEndCombat(string type)
        {
            //don't end combat if we aren't in combat and we have a lined up chatter
            if (!inCombat && type == "Combat")
                return;
            inCombat = false;           
            logger.LogInformation("Exited Combat");
            //roll for another chatter ahead of time
            combatActions.Clear();
            
        }

        void AddCommands()
        {
            if (Configuration.Instance.JoinCommandEnabled || Configuration.Instance.AllowChattersAsDrones || Configuration.Instance.AllowChatterDroneEnemies)
                commands.Add(Configuration.Instance.JoinCommand, new TwitchCommand((e) => {
                    if (!queueOpen)
                    {
                        TwitchChat.SendMessageToChat(Configuration.Instance.JoinFailedQueueClosedText);
                        return;
                    }
                    if (ChattersAvailable.ContainsKey(e.ChatMessage.DisplayName))
                    {
                        TwitchChat.SendMessageToChat(Configuration.Instance.JoinFailedAlreadyQueuedText);
                        return;
                    }

                    if (bannedChatters.Contains(e.ChatMessage.DisplayName))
                    {
                        TwitchChat.SendMessageToChat(Configuration.Instance.JoinFailedBannedText);
                        return;
                    }

                    logger.LogInformation($"{e.ChatMessage.DisplayName} joined the fight!");
                    ChattersAvailable.Add(e.ChatMessage.DisplayName, 0);
                    try
                    {
                        //RFT bug, seems sometimes there aren't colors?
                        ChatterColors.Add(e.ChatMessage.DisplayName, new Color(e.ChatMessage.ColorHex.Substring(1)));
                    }catch(Exception ex)
                    {
                        logger.LogError($"Missing color! It says its {e.ChatMessage.ColorHex}", ex);
                        ChatterColors.Add(e.ChatMessage.DisplayName, new Color(1,1,1));
                    }
                    CobaltChatCoreManifest.EventHub.SignalEvent(CobaltChatCoreManifest.ChatterJoinsEvent, e.ChatMessage);
                    TwitchChat.SendMessageToChat(Configuration.Instance.ChatterJoinedText.Replace("{User}", e.ChatMessage.DisplayName));

                }));

            commands.Add(Configuration.Instance.OpenQueueCommand, new TwitchCommand(TwitchCommand.AccessLevel.MOD, (e) =>
            {
                queueOpen = true;
                TwitchChat.SendMessageToChat(Configuration.Instance.QueueOpenText);
            }));

            commands.Add(Configuration.Instance.CloseQueueCommand, new TwitchCommand(TwitchCommand.AccessLevel.MOD, (e) =>
            {
                queueOpen = false;
                TwitchChat.SendMessageToChat(Configuration.Instance.QueueClosedText);
            }));

            commands.Add(Configuration.Instance.ChatterListClearCommand, new TwitchCommand(TwitchCommand.AccessLevel.MOD, (e) =>
            {
                ChattersAvailable.Clear();
                CobaltChatCoreManifest.EventHub.SignalEvent(CobaltChatCoreManifest.ClearChattersEvent, e.ChatMessage.DisplayName);
                TwitchChat.SendMessageToChat(Configuration.Instance.QueueClearedText);
            }));
            commands.Add(Configuration.Instance.ChatterEjectCommand, new TwitchCommand(TwitchCommand.AccessLevel.MOD, (e) =>
            {
                var list = e.ChatMessage.Message.Split(" ");
                string name = null;
                if (list.Length > 1 && !string.IsNullOrEmpty(list[1]))
                {
                     name = TryFindChatterName(list[1]);
                }
                CobaltChatCoreManifest.EventHub.SignalEvent(CobaltChatCoreManifest.ChatterEjectedEvent, name);   
            }));
            commands.Add(Configuration.Instance.ChatterBanCommand, new TwitchCommand(TwitchCommand.AccessLevel.MOD, (e) =>
            {
                var list = e.ChatMessage.Message.Split(" ");

                if (list.Length > 1 && !string.IsNullOrEmpty(list[1]))
                {
                    var name = TryFindChatterName(list[1]);
                    
                    if (name != null) {
                        ChattersAvailable.Remove(name);
                        CobaltChatCoreManifest.EventHub.SignalEvent(CobaltChatCoreManifest.ChatterEjectedEvent, name);
                        bannedChatters.Add(name);
                        TwitchChat.SendMessageToChat(Configuration.Instance.ChatterBanText.Replace("{User}", name));
                    }
                }
            }));
            commands.Add(Configuration.Instance.ChatterUnbanCommand, new TwitchCommand(TwitchCommand.AccessLevel.MOD, (e) =>
            {
                var list = e.ChatMessage.Message.Split(" ");

                if (list.Length > 1 && !string.IsNullOrEmpty(list[1]))
                {
                    var name = TryFindChatterName(list[1], bannedChatters);
                    if (name != null)  {
                       bannedChatters.Remove(name);
                       TwitchChat.SendMessageToChat(Configuration.Instance.ChatterUnbanText.Replace("{User}", name));         
                    }
                }
            }));
            commands.Add(Configuration.Instance.CardGiveCommand, new TwitchCommand(TwitchCommand.AccessLevel.SPECIAL, (e) =>
            {
                Card card = null;
                string cardName = e.ChatMessage.Message.Substring(Configuration.Instance.CardGiveCommand.Length + 1).Trim().ToLower();
                try {
                    card = TryMakeCard(cardName);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed spawning card!");
                }
                    

                if (card != null)
                combatActions.Enqueue((c) =>
                {
                    try
                    {
                        card.singleUseOverride = true;
                        card.temporaryOverride = true;

                        var addAction = new AAddCard()
                        {
                            card = card,
                            destination = CardDestination.Hand,
                            amount = 1
                        };
                        c.QueueImmediate(addAction);
                    }catch(Exception ex)
                    {
                        logger.LogError(ex, "Failed inserting card into hand");
                    }
                });
            //silent fail, this is a hidden function after all
            }));
            


        }

        public static string TryFindChatterName(string clue)
        {
            return TryFindChatterName(clue, ChattersAvailable.Keys.ToList());
        }

        public static string TryFindChatterName(string clue, List<string> list )
        {
            var name = clue.StartsWith("@") ? clue.Substring(1) : clue;
            foreach (string user in list)
            {
                if (user.ToLower() == name.ToLower())
                {
                    return user;
                }
            }
            return null;
        }

        Card TryMakeCard(string cardName)
        {
            string upgrade = null;
            if (cardName.EndsWith(" a"))
            {
                upgrade = "a";
                cardName = cardName[..^2];//.Substring(0, cardName.Length - 2)
            }
            if (cardName.EndsWith(" b"))
            {
                upgrade = "b";
                cardName = cardName[..^2];
            }
            Card card = null;
            logger.LogInformation("Looking for card " + cardName);
            foreach (string name in DB.currentLocale.strings.Keys)
            {
                if (DB.currentLocale.strings[name].ToLower() == cardName && name.StartsWith("card."))
                {
                    int i = name.IndexOf(".")+1;
                    string key = "";
                    if (name.EndsWith(".name")) 
                        key = name.Substring(i, name.LastIndexOf(".") - i);
                    else
                        key = name.Substring(i);
                    logger.LogInformation("Trying to make card " + key);
                    if (DB.cards.ContainsKey(key))
                    {
                        card = (Card?)Activator.CreateInstance(DB.cards[key]);
                        if (upgrade == "a")
                            card.upgrade = Upgrade.A;
                        if (upgrade == "b")
                            card.upgrade = Upgrade.B;

                    }
                    return card;    
                }

            }
            return card;
        }

        void OnCombatUpdate(Combat combat)
        {
            if (combatActions.TryDequeue(out Action<Combat> action))
            {
                action.Invoke(combat);
            }
        }

    }


}
