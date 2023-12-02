using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using TwitchLib.Api.Helix.Models.Charity;
using TwitchLib.Client;
using TwitchLib.Client.Events;
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

        Dictionary<string, TwitchCommand> commands = new Dictionary<string, TwitchCommand>();
        ConcurrentQueue<Action<Combat>> combatActions = new ConcurrentQueue<Action<Combat>>();

        /// <summary>
        /// displayname -> number of times they were picked;
        /// </summary>
        Dictionary<string, int> chattersAvailable = new Dictionary<string, int>();
        List<string> bannedChatters = new List<string>();
        private string _currentlySelectedChatter;
        public string CurrentlySelectedChatter { get => _currentlySelectedChatter; private set {
                _currentlySelectedChatter = value;
            } }

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

            CobaltChatCoreManifest.EventHub.ConnectToEvent<State>(CobaltChatCoreManifest.StartCombatEvent, (st) => ReplaceEnemyWithChatter(st));
            CobaltChatCoreManifest.EventHub.ConnectToEvent<string>(CobaltChatCoreManifest.EnterRouteEvent, (s) => TryEndCombat(s));
            CobaltChatCoreManifest.EventHub.ConnectToEvent<Combat>(CobaltChatCoreManifest.UpdateCombatEvent, (c) => OnCombatUpdate(c));

        }

        void ReplaceEnemyWithChatter(State state)
        {
            Combat c = (Combat)state.route;
            AI ai = c?.otherShip?.ai;

            if (ai == null || ai.Name() == "FakeCombat")
            {
                logger.LogError("Ignoring Fake Combat");
                return;
            }

            if (state.route == null || state.route is not Combat || state.map?.GetCurrent()?.contents is not MapBattle)
            {
                logger.LogError("Empty combat passed or not a combat!");
                return;
            }
            
            if (!queueOpen)
            {
                logger.LogInformation("Queue closed, not replacing enemy");
            }

            var combatType = ((MapBattle)state.map.GetCurrent().contents).battleType;

            if (Configuration.Instance.AllowedEncounterOverrides.Contains(combatType))
            {
                logger.LogInformation($"Combat type {combatType} not allowed for override!");
                return;
            }

            

            if (ai.character.type == CobaltChatCoreManifest.TwitchCharacterName + "Deck")
            {
                logger.LogInformation("Chatter already present");
                return;
            }

            if (CurrentlySelectedChatter != null)
            {
                logger.LogInformation($"Selected {CurrentlySelectedChatter} as next enemy");
                chattersAvailable[CurrentlySelectedChatter] += 1;
                inCombat = true;
            }
            else
                logger.LogInformation("No chatters or no luck this time. No replacement done");
        }

       
        string SelectChatter(ChatterChoiceMode choiceMode)
        {
            var list = chattersAvailable.ToImmutableDictionary();
            Random random = new Random();
            double randomNumber = random.NextDouble();

            if (list.Count > 0 && randomNumber < Configuration.Instance.ChatterChance)
            {
                string chosenOne = null;
                switch (choiceMode)
                {
                    case ChatterChoiceMode.LEAST_SELECTED:
                        chosenOne = list.Where(kvp => kvp.Value < Configuration.Instance.ChatterPickLimit).OrderBy(kvp => kvp.Value).First().Key;
                        break;
                    case ChatterChoiceMode.RANDOM:
                        chosenOne = list.Keys.ElementAt(random.Next(0, list.Keys.Count()));
                        break;
                }
                return chosenOne;
            }

            else
                return null;
        }

        public void OnChatMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            string[] parts = e.ChatMessage.Message.Split(" ");
            string command = parts[0];
            if (command.StartsWith(Configuration.Instance.CommandSignal) && commands.TryGetValue(command.Substring(1), out TwitchCommand action))
                action.Invoke(e);
            else
                if (CurrentlySelectedChatter == e.ChatMessage.DisplayName)
                    CobaltChatCoreManifest.EventHub.SignalEvent(CobaltChatCoreManifest.ChatterShoutEvent, e.ChatMessage);
        }

        public void TryEndCombat(string type)
        {

            
            
            //don't end combat if we aren't in combat and we have a lined up chatter
            if (!inCombat && CurrentlySelectedChatter != null && type == "Combat")
                return;
            inCombat = false;           
            logger.LogInformation("Exited Combat");
            //roll for another chatter ahead of time
            CurrentlySelectedChatter = SelectChatter(Configuration.Instance.ChoiceMode);
            CobaltChatCoreManifest.EventHub.SignalEvent<string>(CobaltChatCoreManifest.SelectChatterEvent, CurrentlySelectedChatter);
            combatActions.Clear();
            
        }

        void AddCommands()
        {
            if (Configuration.Instance.JoinCommandEnabled)
                commands.Add(Configuration.Instance.JoinCommand, new TwitchCommand((e) => {
                    if (!queueOpen)
                    {
                        TwitchChat.SendMessageToChat(Configuration.Instance.JoinFailedQueueClosedText);
                        return;
                    }
                    if (chattersAvailable.ContainsKey(e.ChatMessage.DisplayName))
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
                    chattersAvailable.Add(e.ChatMessage.DisplayName, 0);
                    CobaltChatCoreManifest.EventHub.SignalEvent(CobaltChatCoreManifest.ChatterJoinsEvent, e.ChatMessage.DisplayName);
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
                chattersAvailable.Clear();
                CobaltChatCoreManifest.EventHub.SignalEvent(CobaltChatCoreManifest.ClearChattersEvent, e.ChatMessage.DisplayName);
                TwitchChat.SendMessageToChat(Configuration.Instance.QueueClearedText);
            }));
            commands.Add(Configuration.Instance.ChatterEjectCommand, new TwitchCommand(TwitchCommand.AccessLevel.MOD, (e) =>
            {
                if (CurrentlySelectedChatter != null)
                {
                    CobaltChatCoreManifest.EventHub.SignalEvent(CobaltChatCoreManifest.ChatterEjectedEvent, CurrentlySelectedChatter);
                    TwitchChat.SendMessageToChat(Configuration.Instance.ChatterEjectText.Replace("{User}", CurrentlySelectedChatter));
                    CurrentlySelectedChatter = null;
                }
                
                
            }));
            commands.Add(Configuration.Instance.ChatterBanCommand, new TwitchCommand(TwitchCommand.AccessLevel.MOD, (e) =>
            {
                var list = e.ChatMessage.Message.Split(" ");

                if (list.Length > 1 && !string.IsNullOrEmpty(list[1]))
                {
                    var name = list[1].StartsWith("@") ? list[1].Substring(1) : list[1];
                    foreach (string user in chattersAvailable.Keys)
                    {
                        if (user.ToLower() == name.ToLower()) {
                            name = user; 
                            chattersAvailable.Remove(user);
                            CobaltChatCoreManifest.EventHub.SignalEvent(CobaltChatCoreManifest.ChatterEjectedEvent, name);
                            bannedChatters.Add(name);
                            TwitchChat.SendMessageToChat(Configuration.Instance.ChatterBanText.Replace("{User}", name));
                            break;
                        }
                    }
                }
            }));
            commands.Add(Configuration.Instance.ChatterUnbanCommand, new TwitchCommand(TwitchCommand.AccessLevel.MOD, (e) =>
            {
                var list = e.ChatMessage.Message.Split(" ");

                if (list.Length > 1 && !string.IsNullOrEmpty(list[1]))
                {
                    var name = list[1].StartsWith("@") ? list[1].Substring(1) : list[1];
                    foreach (string user in bannedChatters)
                    {
                        if (user.ToLower() == name.ToLower())
                        {
                            name = user;
                            bannedChatters.Remove(user);
                            TwitchChat.SendMessageToChat(Configuration.Instance.ChatterUnbanText.Replace("{User}", name));
                            break;
                        }
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
