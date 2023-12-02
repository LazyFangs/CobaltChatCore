using CobaltChatCore;
using CobaltChatCore.Setup;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace CobaltChatCore
{

    public class Configuration 
    {
        public string AccessToken { get; set; } = "";
        public long ValidUntil { get; set; } = 0;
        public string ChannelName { get; set; } = "";
        public const string ClientId = Secrets.ClientID;
        [JsonIgnore]
        public string RedirectUri
        {
            get
            {
                return "http://localhost:" + (Port > 0 ? Port : 3000);
            }
        }
        public int Port { get; set; } = 3000;
        public static string ConfigPath { get; set; }
        
        public static List<string> Scopes = new List<string> { "chat:read", "user:bot", "chat:edit" };
        public static List<string> SpecialPeople = new List<string> { "Lazy_Fangs", "johndaguerra", "CherofTunes", "daisyowl_ben" };
        [JsonIgnore]
        public bool TokenValidated { get; set; } = false;
        #region Strings
        public int SecondsBetweenReminders { get; set; } = 300;//every 5 minutes
        public static string OnSetupCompleteText { get => $"CobaltChatCore v {CobaltChatCoreManifest.version} active!"; }
        public string RemindersText { get; set; } = "Type {JoinCommand} to potentially become an enemy in the next fight!";
        public string QueueOpenText { get; set; } = "Queue open! Type {JoinCommand} to potentially become an enemy in the next fight!";
        public string QueueClosedText { get; set; } = "Queue closed! No more fighting chat!";
        public string InsufficientPrivledgeText { get; set; } = "You can't use this command, sorry!";
        public string JoinFailedQueueClosedText { get; set; } = "You can't join right now, the queue is closed, sorry!";
        public string JoinFailedAlreadyQueuedText { get; set; } = "You are already prepared to fight! You can't join twice!";
        public string JoinFailedBannedText { get; set; } = "You are banned from taking part in the queue!";
        public string ChatterJoinedText { get; set; } = "{User} joined the fray! They might show up as the next enemy!";
        public string QueueClearedText { get; set; } = "Queue cleared! No one left to fight!";
        public string ChatterEjectText { get; set; } = "{User} ejected from the cockpit! Look at them go... into space.... weeee...";
        public string ChatterBanText { get; set; } = "{User}'s timestreams were cast into the void...";
        public string ChatterUnbanText { get; set; } = "{User} is too important for the timestream, they came back.";
        #endregion
        #region Commands
        public char CommandSignal { get; set; } = '^';
        public string JoinCommand { get; set; } = "join";
        public string ChatterListClearCommand { get; set; } = "clear";
        public string CardGiveCommand { get; set; } = "card";
        public string CloseQueueCommand { get; set; } = "close";
        public string OpenQueueCommand { get; set; } = "open";
        public string ChatterEjectCommand { get; set; } = "eject";
        public string ChatterBanCommand { get; set; } = "ban";
        public string ChatterUnbanCommand { get; set; } = "unban";
        public bool JoinCommandEnabled { get; set; } = true;
        
        #endregion
        #region Battle Join Parameters
        public float ChatterChance { get; set; } = 1;//0 - 100% chance
        public List<BattleType> AllowedEncounterOverrides { get; set; } = new List<BattleType> {BattleType.Normal};
        /// <summary>
        /// How many times a player can join between they no longer show up? Anything bigger than 1000 is infinity
        /// </summary>
        public int ChatterPickLimit { get; set; } = 1000;
        /// <summary>
        /// Queue means each person will get a go, in the order they applied.
        /// Random is random
        /// Least Selected is like queue, but randoms the people who were featured the least
        /// </summary>
        public enum ChatterChoiceMode { LEAST_SELECTED, RANDOM }
        public ChatterChoiceMode ChoiceMode { get; set; } = ChatterChoiceMode.LEAST_SELECTED;
        public bool AllowChatterPicturesAsEnemies { get; set; } = true;
        public bool AllowChatterShoutsAsEnemies { get; set; } = true;
        /// <summary>
        /// When to reset all the chatter selection counters
        /// </summary>
        public enum EncountersResetCondition { NEVER }
        public EncountersResetCondition ResetCondition { get; set; } = EncountersResetCondition.NEVER;
        
        #endregion

        public static Configuration Instance { get; private set; }
        static ILogger logger;
        public Configuration()
        {
            //not used
        }
        public static void GetConfiguration(ILogger logger, DirectoryInfo rootDirectory)
        {
            Configuration.logger = logger;
            Configuration.ConfigPath = rootDirectory.FullName +Path.DirectorySeparatorChar+ "config.json";
            logger.LogInformation("Config located at: " + Configuration.ConfigPath);
            if (!File.Exists(Configuration.ConfigPath))
            {
                File.WriteAllText(Configuration.ConfigPath, JsonConvert.SerializeObject(new Configuration(), Formatting.Indented));
                logger.LogInformation($"Created new CobaltCoreChat Configuration file at {Configuration.ConfigPath}");
            }
            string json = File.ReadAllText(Configuration.ConfigPath);
            if (string.IsNullOrEmpty(json))
                throw new Exception("Empty configuration file!");
            var settings = new JsonSerializerSettings
            {
                ObjectCreationHandling = ObjectCreationHandling.Replace
            };            
            Instance = JsonConvert.DeserializeObject<Configuration>(json, settings);
            if (Instance == null)
                throw new Exception("Deserialization failure!");
            if (string.IsNullOrEmpty(Instance.ChannelName))
                logger.LogError($"Missing channel name! Go to {Configuration.ConfigPath} to update the configuration!");
            logger.LogInformation("Configuration loaded!");
        }

        public static void SaveConfiguration()
        {
            using (StreamWriter file = File.CreateText(Configuration.ConfigPath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(file, Instance);
                logger.LogInformation($"Config Saved");
            }
        }

       
       
    }
}

