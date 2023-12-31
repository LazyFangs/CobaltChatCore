﻿using CobaltCoreModding.Definitions.ExternalItems;
using Microsoft.Xna.Framework.Graphics;
using CobaltCoreModding.Components.Services;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using System.Collections;
using TwitchLib.Client.Models;
using static CobaltChatCore.Configuration;
using static CobaltChatCore.CommandManager;
using TwitchLib.Client.Events;
using System.Collections.Immutable;
using daisyowl.text;

namespace CobaltChatCore
{
    /// <summary>
    /// Stores new enemy data and replaces enemy as soon as possible
    /// </summary>
    public class EnemyHijacker
    {
        public class TwitchChatter : IDisposable
        {
            public MemoryStream TextureStream { get; set; }
            public Texture2D Texture { get; set; }
            public string Name;

            public TwitchChatter(string name, MemoryStream textureStream)
            {
                TextureStream = textureStream;
                Name = name;
                Texture = null;
            }

            public void Dispose()
            {
                if (TextureStream != null)
                    TextureStream.Dispose();
                if (Texture != null)
                    Texture.Dispose();
            }
        }

        public int Id { get; private set; }
        public int FallbackCharacterSprite { get; private set; }
        public int BorderSprite { get; private set; }

        private static GraphicsDevice graphics_device;
        private static Dictionary<Spr, Texture2D> CachedTextures;
        private static IDictionary CharPanels;
        private ILogger Logger;

        private const string FallbackCharacterName = "Chatterman";
        Dictionary<string, TwitchChatter> chatterEnemies = new Dictionary<string, TwitchChatter>();

        ConcurrentQueue<TwitchChatter> newIncomingChatters = new ConcurrentQueue<TwitchChatter>();
        
        
        ConcurrentQueue<string> incomingShouts = new ConcurrentQueue<string>();
        bool forceDefault = false;
        
        Character TwitchEnemyCharacter;

        private string _currentCombatant;
        public string CurrentCombatant { get => _currentCombatant; set 
            {
                OnStateEnterClearCharacter(null);
                _currentCombatant = value;
                
            } 
        }
        

        public EnemyHijacker(int twitch_sprite_id, int twitch_character_sprite_id, int twitch_character_border_sprite_id) 
        {
            Id = twitch_sprite_id;
            FallbackCharacterSprite = twitch_character_sprite_id;
            BorderSprite = twitch_character_border_sprite_id;
            TwitchEnemyCharacter = new Character() { type = CobaltChatCoreManifest.TwitchCharacterName + "Deck" };
            chatterEnemies.Add(FallbackCharacterName, new TwitchChatter(FallbackCharacterName, null));
        }

        public void Setup(ILogger logger)
        {
            Logger = logger;
            TwitchChat.Client.OnMessageReceived += OnChatMessageReceived;

            CobaltChatCoreManifest.EventHub.ConnectToEvent<State>(CobaltChatCoreManifest.StartCombatEvent, (s) => ReplaceEnemyWithChatter(s));
            CobaltChatCoreManifest.EventHub.ConnectToEvent<string>(CobaltChatCoreManifest.EnterRouteEvent, (s) => OnStateEnterClearCharacter(s));
            CobaltChatCoreManifest.EventHub.ConnectToEvent<string>(CobaltChatCoreManifest.GRenderEvent, (s) => OnGRenderMakeEnemyASAP());
            CobaltChatCoreManifest.EventHub.ConnectToEvent<Combat>(CobaltChatCoreManifest.UpdateCombatEvent, (c) => OnCombatUpdate(c));

            //CobaltChatCoreManifest.EventHub.ConnectToEvent<string>(CobaltChatCoreManifest.SelectEnemyChatterEvent, (s) => CurrentCombatant = s);
            CobaltChatCoreManifest.EventHub.ConnectToEvent<ChatMessage>(CobaltChatCoreManifest.ChatterJoinsEvent, (cm) => Task.Run(async () => await GetChatterTextureStream(cm.DisplayName)));
            CobaltChatCoreManifest.EventHub.ConnectToEvent<string>(CobaltChatCoreManifest.ClearChattersEvent, (s) => ClearIncomingQueue());
            CobaltChatCoreManifest.EventHub.ConnectToEvent<string>(CobaltChatCoreManifest.ChatterEjectedEvent, (s) => OnChatterEjected(s));
        }

        public void OnChatMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            
            if (CurrentCombatant == e.ChatMessage.DisplayName && Configuration.Instance.AllowChatterShoutsAsEnemies)
                ReceiveNewShout(e.ChatMessage);
        }

        void ReplaceEnemyWithChatter(State state)
        {
            Combat c = (Combat)state.route;
            AI ai = c?.otherShip?.ai;

            if (ai == null || ai.Name() == "FakeCombat")
            {
                Logger.LogError("Ignoring Fake Combat");
                return;
            }

            if (state.route == null || state.route is not Combat || state.map?.GetCurrent()?.contents is not MapBattle)
            {
                Logger.LogError("Empty combat passed or not a combat!");
                return;
            }

            var combatType = ((MapBattle)state.map.GetCurrent().contents).battleType;

            if (!Configuration.Instance.AllowedEncounterOverrides.Contains(combatType))
            {
                Logger.LogInformation($"Combat type {combatType} not allowed for override!");
                return;
            }

            if (ai.character.type == CobaltChatCoreManifest.TwitchCharacterName + "Deck")
            {
                Logger.LogInformation("Chatter already present");
                return;
            }

            CurrentCombatant = SelectChatter();
            if (CurrentCombatant != null)
            {
                //we put out a signal here to make sure only valid fights are replaced
                CobaltChatCoreManifest.EventHub.SignalEvent<string>(CobaltChatCoreManifest.SelectEnemyChatterEvent, CurrentCombatant);
                Logger.LogInformation($"Selected {CurrentCombatant} as next enemy");
                
            }
            else
                Logger.LogInformation("No chatters or no luck this time. No replacement done");
        }

        string SelectChatter()
        {
            var list = ChattersAvailable.ToImmutableDictionary();
            Random random = new Random();
            double randomNumber = random.NextDouble();
            
            if (list.Count > 0 && Configuration.Instance.ChatterEnemyChance > randomNumber)
            {
                string chosenOne = null;
                switch (Configuration.Instance.EnemyChatterChoiceMode)
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


        /// <summary>
        /// Called from an async thread that grabs the texxture stream from the internet
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="newTextureStream"></param>
        public void InsertNewChatterForProcessing(string owner, MemoryStream stream)
        {
            if (!chatterEnemies.ContainsKey(owner))
            {
                newIncomingChatters.Enqueue(new TwitchChatter(owner, stream));
            }
            else
                stream.Dispose();
        }

        async Task GetChatterTextureStream(string chosenOne)
        {

            try
            {
                var memoryStream = await TwitchApiUser.GetResizedChatterProfilePictureForTexture2D(chosenOne);

                Logger.LogInformation($"Downloaded and modified picture of {chosenOne}");
                InsertNewChatterForProcessing(chosenOne, memoryStream);


            }
            catch (Exception e)
            {
                Logger.LogError(e, "Failed replacing picture!");
            }

        }

        void ClearIncomingQueue()
        {
            while (newIncomingChatters.TryDequeue(out TwitchChatter tc))
                tc.Dispose();
            newIncomingChatters.Clear();
        }

        /// <summary>
        /// called when we have an enemy lined up to replace the current one. We do not dispose the old one because we want to keep the ref
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void DoCharacterSwap(Character original)
        {
            if (chatterEnemies[FallbackCharacterName].Texture == null)
                chatterEnemies[FallbackCharacterName].Texture = SpriteLoader.Get((Spr)FallbackCharacterSprite);

            
            if (CurrentCombatant == null || !chatterEnemies.ContainsKey(CurrentCombatant) )
                return;

            Logger.LogInformation($"Replacing sprite for {CurrentCombatant}");
            if (forceDefault)
                Logger.LogInformation($"Forcing a change in enemy to {CurrentCombatant}");
            CachedTextures[(Spr)Id] = Configuration.Instance.AllowChatterPicturesAsEnemies ? chatterEnemies[CurrentCombatant].Texture : chatterEnemies[FallbackCharacterName].Texture;
            DB.currentLocale.strings["char." + CobaltChatCoreManifest.TwitchCharacterName + "Deck"] = CurrentCombatant;
        }

        void OnChatterEjected(string s)
        {
            if (CurrentCombatant == null)
                return;

            if (s == null || CurrentCombatant == s)
            {
                TwitchChat.SendMessageToChat(Configuration.Instance.ChatterEjectText.Replace("{User}", CurrentCombatant));
                Logger.LogInformation($"Ejecting {CurrentCombatant}");
                CurrentCombatant = FallbackCharacterName;
                forceDefault = true;
            }       
        }

        //copypasta from the SpriteExtender in modloader
        private static GraphicsDevice GetGraphicsDevice()
        {
            if (graphics_device != null)
                return graphics_device;

            //try load graphics device...

            var mg_type = CobaltCoreHandler.CobaltCoreAssembly?.GetType("MG") ?? throw new Exception("MG type not found in assembly");

            var mg_inst = mg_type.GetField("inst")?.GetValue(null) ?? throw new Exception("MG instance not found. has game not started?");

            graphics_device = (mg_type.BaseType?.GetMethod("get_GraphicsDevice")?.Invoke(mg_inst, new object[0]) as GraphicsDevice);

            if (graphics_device == null)
            {
                throw new Exception("Cannot load Graphics device");
            }
            return graphics_device;
        }

        /// <summary>
        /// If we have a combatant, swap to the character immediatelly when it's available. Then don't swap
        /// </summary>
        /// <param name="combat"></param>
        void OnCombatUpdate(Combat combat)
        {
            
            if (!DB.currentLocale.strings.ContainsKey(CobaltChatCoreManifest.TwitchCharacterShout))
                DB.currentLocale.strings.Add(CobaltChatCoreManifest.TwitchCharacterShout, "");

            if (combat.otherShip?.ai?.character == TwitchEnemyCharacter && incomingShouts.TryDequeue(out string shout) &&
                Configuration.Instance.AllowChatterShoutsAsEnemies)
            {              
                DB.currentLocale.strings[CobaltChatCoreManifest.TwitchCharacterShout] = shout;
                TwitchEnemyCharacter.shout = new Shout() { who = TwitchEnemyCharacter.type, key = CobaltChatCoreManifest.TwitchCharacterShout };
            }

            //don't replace character already replaced or no character (fake combat)
            if (!forceDefault && (combat.otherShip?.ai?.character?.type == null || combat.otherShip?.ai?.character == TwitchEnemyCharacter))
                return;
            
            if (CurrentCombatant != null)
            {
                DoCharacterSwap(combat.otherShip.ai.character);
                combat.otherShip.ai.character = TwitchEnemyCharacter;
                forceDefault = false;
            }
        }

        void ReceiveNewShout(ChatMessage shoutContent)
        {
            
            if (shoutContent.DisplayName == CurrentCombatant)
            {
                incomingShouts.Enqueue(shoutContent.Message);
                
            }
        }

        void OnStateEnterClearCharacter(string type)
        {
            if (type != "Combat")
            {
                incomingShouts.Clear();
                _currentCombatant = null;
                TwitchEnemyCharacter.shout = null;
            }
 
        }

        /// <summary>
        /// On game update, make Texture2D and move to known chatters
        /// </summary>
        void OnGRenderMakeEnemyASAP()
        {
            

            if (CachedTextures == null)
                CachedTextures = CobaltCoreHandler.CobaltCoreAssembly?.GetType("SpriteLoader")?.GetField("textures").GetValue(null) as Dictionary<Spr,Texture2D>;
            if (CharPanels == null)
                CharPanels = CobaltCoreHandler.CobaltCoreAssembly?.GetType("DB")?.GetField("charPanels")?.GetValue(null) as IDictionary ?? throw new Exception("Cant find panels");
            if (!DB.currentLocale.strings.ContainsKey("char." + CobaltChatCoreManifest.TwitchCharacterName + "Deck"))
                DB.currentLocale.strings["char." + CobaltChatCoreManifest.TwitchCharacterName + "Deck"] = FallbackCharacterName;

            if (GetGraphicsDevice() != null && CachedTextures != null && newIncomingChatters.TryDequeue(out TwitchChatter? chatter))
            {
                if (!chatterEnemies.ContainsKey(chatter.Name))
                {
                    chatter.Texture = Texture2D.FromStream(graphics_device, chatter.TextureStream);
                    chatter.TextureStream.Dispose();
                    chatterEnemies.Add(chatter.Name, chatter);
                    Logger.LogInformation($"{chatter.Name} TwitchChatter created, ready to be fought");
                }
                else
                    chatter.Dispose();
                
            }
        }

        
    }
}

