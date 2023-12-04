
using CobaltCoreModding.Definitions.ModContactPoints;
using CobaltCoreModding.Definitions.ModManifests;
using Microsoft.Extensions.Logging;
using CobaltCoreModding.Definitions.ExternalItems;
using System.Timers;
using System.Reflection;
using TwitchLib.Client.Models;
using CobaltCoreModding.Definitions;
using Newtonsoft.Json.Linq;
using CobaltCoreModLoaderApp;

namespace CobaltChatCore
{
    public class CobaltChatCoreManifest : ISpriteManifest, IAnimationManifest, ICustomEventManifest, IDeckManifest, IAddinManifest
    {
        public const string version = "0.7.9";

        public DirectoryInfo? ModRootFolder { get; set; }
        public DirectoryInfo? GameRootFolder { get; set; }

        
        private ExternalAnimation? default_animation;
        
        private ExternalDeck? twitch_deck;//required to register deck, to register animation
        
        public static ICustomEventHub? EventHub;

        public string Name => "Lazy_Fangs.CobaltChatCore";
        public const string StartCombatEvent = "LazyFangs.CCC.StartCombatEvent";
        public const string UpdateCombatEvent = "LazyFangs.CCC.UpdateCombatEvent";
        public const string EnterRouteEvent = "LazyFangs.CCC.EnterRouteEvent";
        public const string GRenderEvent = "LazyFangs.CCC.GRenderEvent";
        public const string SelectEnemyChatterEvent = "LazyFangs.CCC.SelectEnemyChatterEvent";
        public const string ChatterJoinsEvent = "LazyFangs.CCC.ChatterJoinsEvent";
        public const string ClearChattersEvent = "LazyFangs.CCC.ClearChattersEvent";
        public const string ChatterShoutEvent = "LazyFangs.CCC.ChatterShoutEvent";
        public const string ChatterEjectedEvent = "LazyFangs.CCC.ChatterEjectedEvent";
        
        public const string ASpawnBeginPre = "LazyFangs.CCC.ASpawnBeginPre";
        public const string ASpawnBeginPost = "LazyFangs.CCC.ASpawnBeginPost";
        public const string SelectDroneChatterEvent = "LazyFangs.CCC.SelectDroneChatterEvent";
        public const string StuffOnDestroyed = "LazyFangs.CCC.StuffOnDestroyed";

        public const string TwitchCharacterName = "LazyFangs.CCC.TwitchEnemy";
        public const string TwitchCharacterShout = TwitchCharacterName + "Text";
        public const string IsaacDroneShout = "LazyFangs.CCC.DroneShoutText";

        public ILogger? Logger { get; set; }

        public static CommandManager commandManager { get; private set; }
        public static EnemyHijacker? enemyHijacker { get; private set; }
        public static DroneHijacker? droneHijacker { get; private set; }

        public Assembly CobaltCoreAssembly => throw new NotImplementedException();

        IEnumerable<DependencyEntry> IManifest.Dependencies => new DependencyEntry[0];

        System.Timers.Timer timer;


        public void LoadManifest(ISpriteRegistry artRegistry)
        {
         
            if (ModRootFolder == null)
                throw new Exception("Root Folder not set");

            var path = Path.Combine(ModRootFolder.FullName, "Sprites", Path.GetFileName("viewer_avatar_fallback.png"));
            var tempSprite = new ExternalSprite(TwitchCharacterName, new FileInfo(path));
            artRegistry.RegisterArt(tempSprite);//tempsprite will be the main ID used for replacements

            var default_character_sprite = new ExternalSprite(TwitchCharacterName + "Sprite", new FileInfo(path));
            artRegistry.RegisterArt(default_character_sprite);

            path = Path.Combine(ModRootFolder.FullName, "Sprites", Path.GetFileName("viewer_frame.png"));
            var default_character_border = new ExternalSprite(TwitchCharacterName + "Frame", new FileInfo(path));
            artRegistry.RegisterArt(default_character_border);
            
            enemyHijacker = new EnemyHijacker(tempSprite.Id.Value, default_character_sprite.Id.Value, default_character_border.Id.Value);
        }


        public void LoadManifest(ICustomEventHub eventHub)
        {
            
            HarmonyPatcher.Patch(Logger);
            eventHub.MakeEvent<State>(StartCombatEvent);
            eventHub.MakeEvent<Combat>(UpdateCombatEvent);
            eventHub.MakeEvent<string>(EnterRouteEvent);
            eventHub.MakeEvent<string>(GRenderEvent);
            eventHub.MakeEvent<string>(SelectEnemyChatterEvent);
            eventHub.MakeEvent<ChatMessage>(ChatterJoinsEvent);
            eventHub.MakeEvent<string>(ClearChattersEvent);
            eventHub.MakeEvent<ChatMessage>(ChatterShoutEvent);
            eventHub.MakeEvent<string>(ChatterEjectedEvent);
            
            eventHub.MakeEvent<ASpawn>(ASpawnBeginPre);
            eventHub.MakeEvent<ASpawn>(ASpawnBeginPost);
            eventHub.MakeEvent<string>(SelectDroneChatterEvent);
            eventHub.MakeEvent<StuffBase>(StuffOnDestroyed);
            EventHub = eventHub;

            Task.Run(async () => await PrepareAndConnectToTwitch());
        }

        public void LoadManifest(IAnimationRegistry registry)
        {
            default_animation = new ExternalAnimation(TwitchCharacterName+"Animation", twitch_deck ?? throw new NullReferenceException(), "neutral", false, new ExternalSprite[] {
                ExternalSprite.GetRaw(enemyHijacker.Id),
                ExternalSprite.GetRaw(enemyHijacker.Id),
                ExternalSprite.GetRaw(enemyHijacker.Id),
                ExternalSprite.GetRaw(enemyHijacker.Id),
                ExternalSprite.GetRaw(enemyHijacker.Id)
            });

            registry.RegisterAnimation(default_animation);
            
        }

        public void LoadManifest(IDeckRegistry registry)
        {
            //make peri deck mod          

            var temp1 = ExternalSprite.GetRaw((int)Spr.cards_colorless);
            var temp2 = ExternalSprite.GetRaw((int)Spr.cardShared_border_dracula);

            twitch_deck = new ExternalDeck(TwitchCharacterName+"Deck", System.Drawing.Color.Crimson, System.Drawing.Color.Purple, temp1 ?? throw new NullReferenceException(), temp2 ?? throw new NullReferenceException(), null);

            if (!registry.RegisterDeck(twitch_deck))
                return;
        }

       /* public void LoadManifest(ICharacterRegistry registry)
        {
            

            var start_cards = new Type[] {  };
            var twitch_character = new ExternalCharacter(TwitchCharacterName, twitch_deck ?? throw new NullReferenceException(), twitch_sprite, start_cards, new Type[0], default_animation ?? throw new NullReferenceException(), mini_animation ?? throw new NullReferenceException());
            twitch_character.AddNameLocalisation("Dummy Chatter");
            twitch_character.AddDescLocalisation("Dummy THICC");
            registry.RegisterCharacter(twitch_character);
        }*/

        async Task PrepareAndConnectToTwitch()
        {
            try
            {
                if (Configuration.Instance == null)
                    throw new Exception("Configuration missing! Please run Warmup before starting the mod! CobaltChatCore aborted...");
                if (!Configuration.Instance.TokenValidated || string.IsNullOrEmpty(Configuration.Instance.ChannelName))
                    throw new Exception("Invalid token or missing channel name! CobaltChatCore aborted...");

                Configuration.SaveConfiguration();//right after warmup, so save any potential stuff like changed channel name
                
                //await Configuration.GetConfiguration(Logger, ModRootFolder); we use warmup
                //await TwitchApiUser.ObtainAccessToken(); in warmup
                await TwitchChat.ConnectToChat();
                Logger?.LogInformation("Connection ready!");
                commandManager = new CommandManager();
                
                while (enemyHijacker == null) Thread.Sleep(1000);
                enemyHijacker.Setup(Logger);
                commandManager.Setup(Logger);
                droneHijacker = new DroneHijacker();
                droneHijacker.Setup(Logger);

                Logger?.LogInformation("Setup ready!");

                // Create a timer
                var timer = new System.Timers.Timer();
                timer.Elapsed += new ElapsedEventHandler(SendReminder);
                timer.Interval = Configuration.Instance.SecondsBetweenReminders * 1000;
                timer.Enabled = true;
            }
            catch (Exception e)
            {
                Logger?.LogError(e, "Failed to start mod!");
                if (TwitchChat.Client?.IsConnected ?? false)
                    TwitchChat.Client.Disconnect();
                if (timer != null)
                    timer.Enabled=false;
            }
            
        }

        void SendReminder(object source, ElapsedEventArgs e)
        {
            if (TwitchChat.Client == null)
                return;
            if (!TwitchChat.Client.IsConnected) 
            {
                timer.Enabled = false;
                return;
            }
            TwitchChat.SendMessageToChat(Configuration.Instance.RemindersText.Replace("{JoinCommand}", Configuration.Instance.CommandSignal + Configuration.Instance.JoinCommand));
        }

        public void ModifyLauncher(object? launcherUI)
        {
            if (launcherUI == null || launcherUI is not MainForm)
                throw new Exception("Incorrect or missing UI data!");
            if (Logger == null)
                throw new Exception("Logger not set!");

            MainForm form = (MainForm)launcherUI;

            Configuration.GetConfiguration(Logger, ModRootFolder);
            TwitchApiUser.Setup(Logger);
            TwitchChat.Setup(Logger);
            Task.Run(async () => await TwitchApiUser.IsTokenValid()).Wait();
            var addon = new Form1();
            addon.InitMainForm(form);
            //Form1.Init(form);
        }
    }
}