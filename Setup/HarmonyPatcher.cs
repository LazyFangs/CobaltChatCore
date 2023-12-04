using CobaltCoreModding.Components.Services;
using daisyowl.text;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Reflection;
using static System.Windows.Forms.AxHost;

namespace CobaltChatCore
{
    public class HarmonyPatcher
    {
        static ILogger Logger;
        
        public static void Patch(ILogger logger)
        {
            Logger = logger;
            Harmony harmony = new Harmony("Lazy_Fangs.CCC.Combat") ?? throw new Exception("Can't get hermony!");

            //to select a chatter
            var combat_enter_method = CobaltCoreHandler.CobaltCoreAssembly?.GetType("Combat")?.GetMethod("OnEnter") ?? throw new Exception("Combat OnEnter not Found!");
            //to check for new shouts and execute, thread safely
            var combat_update_method = CobaltCoreHandler.CobaltCoreAssembly?.GetType("Combat")?.GetMethod("Update") ?? throw new Exception("Combat OnExit not Found!");
            //to check if we are moving away from combat
            var route_enter_method = CobaltCoreHandler.CobaltCoreAssembly?.GetType("Route")?.GetMethod("OnEnter") ?? throw new Exception("Combat OnExit not Found!");
            //to help load character textures
            var g_render_method = CobaltCoreHandler.CobaltCoreAssembly?.GetType("G")?.GetMethod("Render") ?? throw new Exception("G Render not Found!");
            var card_render_method = CobaltCoreHandler.CobaltCoreAssembly?.GetType("Card")?.GetMethod("Render") ?? throw new Exception("AttackDrone Render not Found!");

            var attackdrone_render_method = CobaltCoreHandler.CobaltCoreAssembly?.GetType("AttackDrone")?.GetMethod("Render") ?? throw new Exception("Card Render not Found!");
            var aspawn_begin_method = CobaltCoreHandler.CobaltCoreAssembly?.GetType("ASpawn")?.GetMethod("Begin") ?? throw new Exception("Card Render not Found!");
            var stuffbase_ondestroyed_method = CobaltCoreHandler.CobaltCoreAssembly?.GetType("StuffBase")?.GetMethod("DoDestroyedEffect") ?? throw new Exception("DoDestroyedEffect not Found!");

            var combat_enter_postfix = typeof(HarmonyPatcher).GetMethod("CombatOnEnter_PostFix", BindingFlags.Static | BindingFlags.NonPublic) ?? throw new Exception("Combat start postfix not found");
            var combat_update_postfix = typeof(HarmonyPatcher).GetMethod("CombatUpdate_PostFix", BindingFlags.Static | BindingFlags.NonPublic) ?? throw new Exception("Combat update postfix not found");
            var route_enter_postfix = typeof(HarmonyPatcher).GetMethod("RouteOnEnter_PostFix", BindingFlags.Static | BindingFlags.NonPublic) ?? throw new Exception("route start postfix not found");
            var g_render_postfix = typeof(HarmonyPatcher).GetMethod("GRender_PostFix", BindingFlags.Static | BindingFlags.NonPublic) ?? throw new Exception("route start postfix not found");
            var card_render_postfix = typeof(HarmonyPatcher).GetMethod("CardRender_PostFix", BindingFlags.Static | BindingFlags.NonPublic) ?? throw new Exception("route start postfix not found");
            var attackdrone_render_postfix = typeof(HarmonyPatcher).GetMethod("AttackDroneRender_PostFix", BindingFlags.Static | BindingFlags.NonPublic) ?? throw new Exception("attack drone postfix not found");
            var aspawn_begin_postfix = typeof(HarmonyPatcher).GetMethod("ASpawnBegin_PostFix", BindingFlags.Static | BindingFlags.NonPublic) ?? throw new Exception("attack drone postfix not found");
            var aspawn_begin_prefix = typeof(HarmonyPatcher).GetMethod("ASpawnBegin_PreFix", BindingFlags.Static | BindingFlags.NonPublic) ?? throw new Exception("attack drone postfix not found");
            var stuffbase_ondestroyed_postfix = typeof(HarmonyPatcher).GetMethod("StuffBaseOnDestroyedEffect_PostFix", BindingFlags.Static | BindingFlags.NonPublic) ?? throw new Exception("attack drone postfix not found");

            harmony.Patch(combat_enter_method, postfix: new HarmonyMethod(combat_enter_postfix));
            harmony.Patch(combat_update_method, postfix: new HarmonyMethod(combat_update_postfix));
            harmony.Patch(route_enter_method, postfix: new HarmonyMethod(route_enter_postfix));
            harmony.Patch(g_render_method, postfix: new HarmonyMethod(g_render_postfix)); 
            harmony.Patch(card_render_method, postfix: new HarmonyMethod(card_render_postfix));
            harmony.Patch(attackdrone_render_method, postfix: new HarmonyMethod(attackdrone_render_postfix));
            harmony.Patch(aspawn_begin_method, postfix: new HarmonyMethod(aspawn_begin_postfix));
            harmony.Patch(aspawn_begin_method, prefix: new HarmonyMethod(aspawn_begin_prefix));
            harmony.Patch(stuffbase_ondestroyed_method, postfix: new HarmonyMethod(stuffbase_ondestroyed_postfix));


        }

        private static void StuffBaseOnDestroyedEffect_PostFix(StuffBase __instance, object[] __args)
        {
            State s = (State)__args[0];

            if (__instance.droneNameAccordingToIsaac != null && !__instance.IsHostile() && DroneHijacker.Owners.Contains(__instance.droneNameAccordingToIsaac))
            {
                Character? goat = s.characters.FirstOrDefault<Character>((Func<Character, bool>)(c => c.type == "goat"));
                if (goat != null)
                {
                    if (!DB.currentLocale.strings.ContainsKey(CobaltChatCoreManifest.IsaacDroneShout))
                        DB.currentLocale.strings.Add(CobaltChatCoreManifest.IsaacDroneShout, "");

                    DB.currentLocale.strings[CobaltChatCoreManifest.IsaacDroneShout] = string.Format(GetIsaacDestroyShout(), __instance.droneNameAccordingToIsaac);
                    goat.shout = new Shout() { who = goat.type, key = CobaltChatCoreManifest.IsaacDroneShout };

                }
            }
        }

        private static string GetIsaacDestroyShout()
        {
            var list = new List<string>();
            list.Add("Well, at least you tried, {0}");
            list.Add("{0}, you were taken too soon...");
            list.Add("I guess you should have hopped away {0}!");

            var rnd = new Random();
            return list[rnd.Next(list.Count())];
        }

        private static void ASpawnBegin_PreFix(ASpawn __instance)
        {
            CobaltChatCoreManifest.EventHub?.SignalEvent<ASpawn>(CobaltChatCoreManifest.ASpawnBeginPre, __instance);
        }

        private static void ASpawnBegin_PostFix(ASpawn __instance, object[] __args)
        {
          
            G g = (G)__args[0];
            Combat c = (Combat)__args[2];

            if (!__instance.isaacNamesIt && __instance.thing.droneNameAccordingToIsaac != null && !__instance.thing.IsHostile()
                && c.stuff.ContainsKey(__instance.thing.x))//make sure it's a named drone that isn't immediatelly destroyed due to bad launch
            {
                Character? goat = g.state.characters.FirstOrDefault<Character>((Func<Character, bool>)(c => c.type == "goat"));
                if (goat != null)
                {
                    if (!DB.currentLocale.strings.ContainsKey(CobaltChatCoreManifest.IsaacDroneShout))
                        DB.currentLocale.strings.Add(CobaltChatCoreManifest.IsaacDroneShout, "");

                    DB.currentLocale.strings[CobaltChatCoreManifest.IsaacDroneShout] = string.Format(GetIsaacSpawnShout(), __instance.thing.droneNameAccordingToIsaac);
                    goat.shout = new Shout() { who = goat.type, key = CobaltChatCoreManifest.IsaacDroneShout };

                }
            }
            //else if (!c.stuff.ContainsKey(__instance.thing.x) && !__instance.isaacNamesIt)
              //  __instance.thing.droneNameAccordingToIsaac = null;
            //sorting and name fixing, clears name
            CobaltChatCoreManifest.EventHub?.SignalEvent<ASpawn>(CobaltChatCoreManifest.ASpawnBeginPost, __instance);

        }
        private static string GetIsaacSpawnShout()
        {
            var list = new List<string>();
            list.Add("This little guy came from a different dimension! Its name is {0}");
            list.Add("Whoa! This one just jumped out! It says it's called {0}");
            list.Add("This one looks very polite! Everyone, meet {0}!");

            var rnd = new Random();
            return list[rnd.Next(list.Count())];
        }

        private static void AttackDroneRender_PostFix(object[] __args, AttackDrone __instance)
        {
            var foundDrone = DroneHijacker.hijackedDrones.Where(d => d.droneRef.Target == __instance).ToList();
            if (foundDrone.Count() > 0)
            {
                Color? color2 = foundDrone[0].color;
                Color? colorForce = new Color?();
                Vec v = (Vec)__args[1];
                Vec offset = __instance.GetOffset((G)__args[0]);
                Draw.Text(foundDrone[0].owner.Substring(0,5), v.x + offset.x+10, v.y + offset.y+25+(foundDrone[0].nameStaysUp ? 0 : 10),
                    color: color2,
                    colorForce: colorForce,
                    outline : new Color?(Colors.black),
                    align: TAlign.Center
                    );
                
            }
                
        }

        /// <summary>
        /// Only called once per combat start.
        /// used in CommandManager to determine if we are in combat
        /// </summary>
        /// <param name="__args"></param>
        private static void CombatOnEnter_PostFix(object[] __args)
        {
            var state = (State)__args[0];
            //Logger.LogInformation(state.route.GetType().ToString());
            if (state.route is Combat)
                //pass state, because state has map info that defines what type of combat this is.
                CobaltChatCoreManifest.EventHub?.SignalEvent<State>(CobaltChatCoreManifest.StartCombatEvent, state);
        }


        /// <summary>
        /// Called every combat frame
        /// </summary>
        /// <param name="__args"></param>
        private static void CombatUpdate_PostFix(object[] __args)
        {
            var state = ((G)__args[0]).state;

            if (state.route is Combat)
                CobaltChatCoreManifest.EventHub?.SignalEvent<Combat>(CobaltChatCoreManifest.UpdateCombatEvent, (Combat)state.route);


        }
        /// <summary>
        /// When entering a new route, if it's not combat, attempt to end the currently ongoing combat. This will trigger getting a new chatter and DL them
        /// </summary>
        /// <param name="__args"></param>
        private static void RouteOnEnter_PostFix(object[] __args)
        {
            
            var state = (State)__args[0];
            //Logger.LogInformation($"///////////////////////// {state.route.GetType()}");
            CobaltChatCoreManifest.EventHub?.SignalEvent<string>(CobaltChatCoreManifest.EnterRouteEvent, state.route.GetType().ToString());
           
        }

        private static void GRender_PostFix()
        {
            CobaltChatCoreManifest.EventHub?.SignalEvent<string>(CobaltChatCoreManifest.GRenderEvent, null);
        }

        private static void CardRender_PostFix(object[] __args, Card __instance)
        {
            return;//TODO

            if (((G)__args[0]).state.route is not CardReward)
                return;

            Color? color2 = new Color?(Colors.energy);
            Color? colorForce = new Color?();
            double? progress = new double?();
            Draw.Text("Vote ^0", __instance.pos.x+ 15, __instance.pos.y+96,
                color: color2,
                colorForce: colorForce

                );
        }

    }
}

