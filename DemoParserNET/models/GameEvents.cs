using System;
using System.Collections.Generic;
using System.Linq;

namespace DemoParserNET.models
{
    public class GameEvents
    {
        public event EventHandler<object> EventPlayerConnect;
        public event EventHandler<object> EventPlayerDisconnect;
        public event EventHandler<object> EventPlayerSpawn;
        public event EventHandler<object> EventPlayerTeam;
        public event EventHandler<object> EventPlayerFootstep;
        public event EventHandler<object> EventPlayerJump;
        public event EventHandler<object> EventPlayerFallDamage;
        public event EventHandler<object> EventPlayerBlind;
        public event EventHandler<object> EventWeaponFire;
        public event EventHandler<object> EventWeaponReload;
        public event EventHandler<object> EventWeaponZoom;
        public event EventHandler<object> EventWeaponFireOnEmpty;
        public event EventHandler<object> EventPlayerHurt;
        public event EventHandler<object> EventPlayerDeath;
        public event EventHandler<object> EventOtherDeath;
        public event EventHandler<object> EventItemPickup;
        public event EventHandler<object> EventItemEquip;
        public event EventHandler<object> EventItemRemove;
        public event EventHandler<object> EventRoundStart;
        public event EventHandler<object> EventRoundEnd;
        public event EventHandler<object> EventRoundMvp;
        public event EventHandler<object> EventBombPickup;
        public event EventHandler<object> EventBombDropped;
        public event EventHandler<object> EventBombBeginPlant;
        public event EventHandler<object> EventBombPlanted;
        public event EventHandler<object> EventBombExploded;
        public event EventHandler<object> EventBombBeginDefuse;
        public event EventHandler<object> EventBombDefused;
        public event EventHandler<object> EventDecoyDetonate;
        public event EventHandler<object> EventDecoyExpire;
        public event EventHandler<object> EventFlashDetonate;
        public event EventHandler<object> EventSmokeDetonate;
        public event EventHandler<object> EventSmokeExpire;
        public event EventHandler<object> EventNadeDetonate;
        public event EventHandler<object> EventMollyDetonate;
        public event EventHandler<object> EventMollyExpire;
        
        public Dictionary<int, GameEvent> gameEventList = new Dictionary<int, GameEvent>();
        private List<object> tickEvents = new List<object>();

        public GameEvents()
        {
            DemoReader.CSvcGameEventListEvent += HandleGameEventList;
            DemoReader.CSvcGameEventEvent += HandleGameEvent;
            DemoFile.TickEnd += HandleTickEnd;
        }

        private void HandleGameEventList(object sender, CSvcMsgGameEventList message)
        {
            foreach (var descriptor in message.Descriptors)
            {
                gameEventList.Add(descriptor.EventId, new GameEvent()
                {
                    id = descriptor.EventId,
                    name = descriptor.Name,
                    keyNames = descriptor.Keys != null
                        ? descriptor.Keys.Select(t => t.Name).ToArray()
                        : new string[1] {""}
                });
            }
        }

        private void HandleGameEvent(object sender, CSvcMsgGameEvent message)
        {
            if (gameEventList.TryGetValue(message.EventId, out GameEvent gameEvent))
            {
                try
                {
                    tickEvents.Add(gameEvent.MessageToObject(message));
                }
                catch (Exception e)
                {
                    // Do Nothing
                }
            }
        }

        /// <summary>
        /// Emits Game Events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="tick"></param>
        private void HandleTickEnd(object sender, int tick)
        {
            foreach (GameEvent.Event tickEvent in tickEvents)
            {
                switch (tickEvent.Name)
                {
                    case "player_connect":
                        EventPlayerConnect?.Invoke(this, tickEvent);
                        break;
                    case "player_disconnect":
                        EventPlayerDisconnect?.Invoke(this, tickEvent);
                        break;
                    case "player_spawn":
                        EventPlayerSpawn?.Invoke(this, tickEvent);
                        break;
                    case "player_team":
                        EventPlayerTeam?.Invoke(this, tickEvent);
                        break;
                    case "player_footstep":
                        EventPlayerFootstep?.Invoke(this, tickEvent);
                        break;
                    case "player_jump":
                        EventPlayerJump?.Invoke(this, tickEvent);
                        break;
                    case "player_falldamage":
                        EventPlayerFallDamage?.Invoke(this, tickEvent);
                        break;
                    case "player_blind":
                        EventPlayerBlind?.Invoke(this, tickEvent);
                        break;
                    case "weapon_fire":
                        EventWeaponFire?.Invoke(this, tickEvent);
                        break;
                    case "weapon_reload":
                        EventWeaponReload?.Invoke(this, tickEvent);
                        break;
                    case "weapon_zoom":
                        EventWeaponZoom?.Invoke(this, tickEvent);
                        break;
                    case "weapon_fire_on_empty":
                        EventWeaponFireOnEmpty?.Invoke(this, tickEvent);
                        break;
                    case "player_hurt":
                        EventPlayerHurt?.Invoke(this, tickEvent);
                        break;
                    case "player_death":
                        EventPlayerDeath?.Invoke(this, tickEvent);
                        break;
                    case "other_death":
                        EventOtherDeath?.Invoke(this, tickEvent);
                        break;
                    case "item_pickup":
                        EventItemPickup?.Invoke(this, tickEvent);
                        break;
                    case "item_equip":
                        EventItemEquip?.Invoke(this, tickEvent);
                        break;
                    case "item_remove":
                        EventItemRemove?.Invoke(this, tickEvent);
                        break;
                    case "round_start":
                        EventRoundStart?.Invoke(this, tickEvent);
                        break;
                    case "round_end":
                        EventRoundEnd?.Invoke(this, tickEvent);
                        break;
                    case "round_mvp":
                        EventRoundMvp?.Invoke(this, tickEvent);
                        break;
                    case "bomb_pickup":
                        EventBombPickup?.Invoke(this, tickEvent);
                        break;
                    case "bomb_dropped":
                        EventBombDropped?.Invoke(this, tickEvent);
                        break;
                    case "bomb_beginplant":
                        EventBombBeginPlant?.Invoke(this, tickEvent);
                        break;
                    case "bomb_planted":
                        EventBombPlanted?.Invoke(this, tickEvent);
                        break;
                    case "bomb_exploded":
                        EventBombExploded?.Invoke(this, tickEvent);
                        break;
                    case "bomb_begindefuse":
                        EventBombBeginDefuse?.Invoke(this, tickEvent);
                        break;
                    case "bomb_defused":
                        EventBombDefused?.Invoke(this, tickEvent);
                        break;
                    case "decoy_started":
                        EventDecoyDetonate?.Invoke(this, tickEvent);
                        break;
                    case "decoy_detonate":
                        EventDecoyExpire?.Invoke(this, tickEvent);
                        break;
                    case "hegrenade_detonate":
                        EventNadeDetonate?.Invoke(this, tickEvent);
                        break;
                    case "flashbang_detonate":
                        EventFlashDetonate?.Invoke(this, tickEvent);
                        break;
                    case "smokegrenade_detonate":
                        EventSmokeDetonate?.Invoke(this, tickEvent);
                        break;
                    case "smokegrenade_expired":
                        EventSmokeExpire?.Invoke(this, tickEvent);
                        break;
                    case "inferno_startburn":
                        EventMollyDetonate?.Invoke(this, tickEvent);
                        break;
                    case "inferno_expire":
                        EventMollyExpire?.Invoke(this, tickEvent);
                        break;
                    case "hltv_chase":
                    case "hltv_status":
                    case "cs_win_panel_round":
                    case "hltv_fixed":
                    case "player_connect_full":
                    case "server_cvar":
                        break;
                }
            }

            tickEvents.Clear();
        }
    }
}