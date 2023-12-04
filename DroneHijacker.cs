﻿using FMOD;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Events;
using static CobaltChatCore.CommandManager;
using static CobaltChatCore.Configuration;

namespace CobaltChatCore
{
    public class ChatterDrone : IComparable<ChatterDrone>
    {
        public string owner;
        public WeakReference droneRef;
        public bool nameStaysUp = false;
        public Color color;

        public bool moved = false;
        public bool shot = false;

        public ChatterDrone(string selectedChatter, WeakReference weakReference) : this(selectedChatter, weakReference, new Color())
        {
            
        }

        public ChatterDrone(string selectedChatter, WeakReference weakReference, Color color)
        {
            this.owner = selectedChatter;
            this.droneRef = weakReference;
            this.color = color;
        }

        public int CompareTo(ChatterDrone other)
        {
            if (droneRef.Target == null)
                return -1;
            if (other.droneRef.Target == null)
                return 1;
            StuffBase a = (StuffBase)droneRef.Target;
            StuffBase b = (StuffBase)other.droneRef.Target;
            return a.x > b.x ? 1 : -1;//we will never have 2 objects in the same place
        }
    }

    class SingleDroneShot : CardAction
    {
        StuffBase drone;

        public SingleDroneShot(StuffBase drone)
        {
            this.drone = drone;
        }

        public override void Begin(G g, State s, Combat c)
        {
            this.timer = 0;
            c.Queue(drone.GetActions(s, c));
        }
    }

    class SingleDroneMove : ADroneMove
    {
        StuffBase drone;
        public SingleDroneMove(StuffBase drone, DroneHijacker.ChatterDroneAction direction)
        {
            this.drone = drone;
            dir = direction == DroneHijacker.ChatterDroneAction.LEFT ? -1 : 1;
            timer = 0.2;
            isRandom = false;
        }
        public override void Begin(G g, State s, Combat c)
        {
            var newX = drone.x + dir;
            //check for collission
            foreach (StuffBase stuff in c.stuff.Values)
            {
                if (stuff.x == newX && stuff != drone)
                {
                    c.DestroyDroneAt(s, newX, true);
                    c.DestroyDroneAt(s, drone.x, true);
                    return;
                }
            }
            //move
            c.stuff.Remove(drone.x);
            drone.x = newX;
            c.stuff.Add(drone.x, drone);
            Audio.Play(new GUID?(FSPRO.Event.Move));
            //check for walls
            if (c.leftWall.HasValue && newX < c.leftWall.Value)
                c.DestroyDroneAt(s, newX, true);
            if (c.rightWall.HasValue && newX >= c.rightWall.Value)
                c.DestroyDroneAt(s, newX, true);
            
        }
    }

    public class DroneHijacker
    {
        public static List<ChatterDrone> hijackedDrones = new List<ChatterDrone>();
        public static List<string> Owners { get => hijackedDrones.Select(h => h.owner).ToList(); }
        ConcurrentQueue<Action<Combat>> combatActions = new ConcurrentQueue<Action<Combat>>();
        public enum ChatterDroneAction {LEFT, RIGHT, SHOOT }

        ILogger Logger;        

        public void Setup(ILogger logger)
        {
            this.Logger = logger;            

            //make sure we don't select the current enemy
            CobaltChatCoreManifest.EventHub.ConnectToEvent<string>(CobaltChatCoreManifest.SelectEnemyChatterEvent, (s) => { hijackedDrones.Add(new ChatterDrone(s, new WeakReference(null))); });

            CobaltChatCoreManifest.EventHub.ConnectToEvent<ASpawn>(CobaltChatCoreManifest.ASpawnBeginPre, (asp) => PrefixReplaceDroneWithChatter(asp));
            CobaltChatCoreManifest.EventHub.ConnectToEvent<ASpawn>(CobaltChatCoreManifest.ASpawnBeginPost, (asp) => PostfixSortDronesAndFixNamePositions(asp));
            CobaltChatCoreManifest.EventHub.ConnectToEvent<string>(CobaltChatCoreManifest.EnterRouteEvent, (s) => OnStateEnterClearDrones(s));
            CobaltChatCoreManifest.EventHub.ConnectToEvent<Combat>(CobaltChatCoreManifest.UpdateCombatEvent, (c) => OnCombatUpdate(c));            

            if (Configuration.Instance.AllowChatterDronesMove)
                commands.Add(Configuration.Instance.ChatterDroneMoveLeftCommand, new TwitchCommand(TwitchCommand.AccessLevel.EVERYONE, (e) =>
                {
                    foreach(ChatterDrone chd in hijackedDrones)
                    {
                        if (chd.owner == e.ChatMessage.DisplayName)
                        {
                            combatActions.Enqueue((c) => { DoDroneAction(chd, ChatterDroneAction.LEFT, c); });
                            return;
                        }
                    }
                }));
            if (Configuration.Instance.AllowChatterDronesMove)
                commands.Add(Configuration.Instance.ChatterDroneMoveRightCommand, new TwitchCommand(TwitchCommand.AccessLevel.EVERYONE, (e) =>
                {
                    foreach (ChatterDrone chd in hijackedDrones)
                    {
                        if (chd.owner == e.ChatMessage.DisplayName)
                        {
                            combatActions.Enqueue((c) => { DoDroneAction(chd, ChatterDroneAction.RIGHT, c); });
                            return;
                        }
                    }

                }));
            if (Configuration.Instance.AllowChatterDronesShoot)
                commands.Add(Configuration.Instance.ChatterDroneShootCommand, new TwitchCommand(TwitchCommand.AccessLevel.EVERYONE, (e) =>
                {
                    foreach (ChatterDrone chd in hijackedDrones)
                    {
                        if (chd.owner == e.ChatMessage.DisplayName)
                        {
                            combatActions.Enqueue((c) => { DoDroneAction(chd, ChatterDroneAction.SHOOT, c); });
                            return;
                        }
                    }
                }));
        }



        void DoDroneAction(ChatterDrone chd, ChatterDroneAction action, Combat c)
        {
            if (chd.droneRef.Target == null)
            {
                //don't do anything, they had their chance
                return;
            }

            StuffBase drone = (StuffBase)chd.droneRef.Target;
            switch (action)
            {
                case ChatterDroneAction.LEFT:
                    if (!chd.moved)
                    {
                        c.Queue(new SingleDroneMove(drone, ChatterDroneAction.LEFT));
                        chd.moved = true;
                    }
                    break;
                case ChatterDroneAction.RIGHT:
                    if (!chd.moved)
                    {
                        c.Queue(new SingleDroneMove(drone, ChatterDroneAction.RIGHT));
                        chd.moved = true;
                    }
                    break;
                case ChatterDroneAction.SHOOT:
                    if (!chd.shot)
                    {
                        c.Queue(new SingleDroneShot(drone));
                        chd.shot = true;
                    }
                    break;
            }           

        }

        

        void OnCombatUpdate(Combat combat)
        {
            if (combatActions.TryDequeue(out Action<Combat> action))
            {
                action.Invoke(combat);
            }
        }

        void OnStateEnterClearDrones(string type)
        {
            if (type != "Combat")
            {
                hijackedDrones.Clear();
                //currentState = null;
            }

        }

        void PostfixSortDronesAndFixNamePositions(ASpawn spawn)
        {
            hijackedDrones.Sort();
            bool nameSorter = true;
            foreach (ChatterDrone cd in hijackedDrones)
            {
                cd.nameStaysUp = nameSorter;
                nameSorter = !nameSorter;
            }
        }

        void PrefixReplaceDroneWithChatter(ASpawn spawn)
        {
            if (!Configuration.Instance.AllowChattersAsDrones)
                return;

            if (spawn.thing is not AttackDrone && spawn.thing is not ShieldDrone && spawn.thing is not EnergyDrone)
            {
                Logger.LogInformation("Not allowed drone type");
                return;
            }

            var selectedChatter = SelectChatter();
            if (selectedChatter != null)
            {
                ChatterDrone chdrone = new ChatterDrone(selectedChatter, new WeakReference(spawn.thing), CommandManager.ChatterColors[selectedChatter]);
                Logger.LogInformation($"Drone hijacked by {selectedChatter}");
                hijackedDrones.Add(chdrone);
                spawn.isaacNamesIt = false;
                spawn.thing.droneNameAccordingToIsaac = selectedChatter;
            }
            else
            {                
                Logger.LogInformation($"Normal drone selected");
            }
                

        }

        string SelectChatter()
        {
            Random random = new Random();
            double randomNumber = random.NextDouble();
            List<string> notAllowed = Owners;
            List<string> list = CommandManager.ChattersAvailable.ToImmutableDictionary().Keys.Where(k => !notAllowed.Contains(k)).ToList();

            if (list.Count > 0 && randomNumber < Configuration.Instance.ChatterDroneChance)
            { 
                return list.ElementAt(random.Next(0, list.Count()));  
            }

            else
                return null;
        }
        

    }
}
