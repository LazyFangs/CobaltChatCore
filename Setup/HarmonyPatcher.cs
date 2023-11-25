using CobaltCoreModding.Components.Services;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using System.Reflection;
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
            var card_render_method = CobaltCoreHandler.CobaltCoreAssembly?.GetType("Card")?.GetMethod("Render") ?? throw new Exception("Card Render not Found!");

            var combat_enter_postfix = typeof(HarmonyPatcher).GetMethod("CombatOnEnter_PostFix", BindingFlags.Static | BindingFlags.NonPublic) ?? throw new Exception("Combat start postfix not found");
            var combat_update_postfix = typeof(HarmonyPatcher).GetMethod("CombatUpdate_PostFix", BindingFlags.Static | BindingFlags.NonPublic) ?? throw new Exception("Combat update postfix not found");
            var route_enter_postfix = typeof(HarmonyPatcher).GetMethod("RouteOnEnter_PostFix", BindingFlags.Static | BindingFlags.NonPublic) ?? throw new Exception("route start postfix not found");
            var g_render_postfix = typeof(HarmonyPatcher).GetMethod("GRender_PostFix", BindingFlags.Static | BindingFlags.NonPublic) ?? throw new Exception("route start postfix not found");
            var card_render_postfix = typeof(HarmonyPatcher).GetMethod("CardRender_PostFix", BindingFlags.Static | BindingFlags.NonPublic) ?? throw new Exception("route start postfix not found");

            harmony.Patch(combat_enter_method, postfix: new HarmonyMethod(combat_enter_postfix));
            harmony.Patch(combat_update_method, postfix: new HarmonyMethod(combat_update_postfix));
            harmony.Patch(route_enter_method, postfix: new HarmonyMethod(route_enter_postfix));
            harmony.Patch(g_render_method, postfix: new HarmonyMethod(g_render_postfix)); 
            harmony.Patch(card_render_method, postfix: new HarmonyMethod(card_render_postfix));


        }

        /// <summary>
        /// Only called once per combat start.
        /// used in CommandManager to determine if we are in combat
        /// </summary>
        /// <param name="__args"></param>
        private static void CombatOnEnter_PostFix(object[] __args)
        {
            var state = (State)__args[0];
            Logger.LogInformation(state.route.GetType().ToString());
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
            Logger.LogInformation($"///////////////////////// {state.route.GetType()}");
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

