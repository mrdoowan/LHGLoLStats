using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RiotSharp.Endpoints.MatchEndpoint;
using RiotSharp.Endpoints.MatchEndpoint.Enums;
using System.Collections;

namespace LoLStatsAPIv4_GUI {
    public class ObjectiveList : IEnumerable {

        // Consts
        private const double BARON_DURATION = 3.5;  // in Minutes

        // Private member variables
        private Dictionary<ObjectiveEvent, List<int>> EventIdx; // Cancer
        private List<Objective> Objectives;

        #region DB Columns for Team

        public int TowersAt15 { get; set; }
        public int TowersDiff15 { get; set; }
        public int KillsAt15 { get; set; }
        public int KillsDiff15 { get; set; }
        public int TowersAt25 { get; set; }
        public int TowersDiff25 { get; set; }
        public int KillsAt25 { get; set; }
        public int KillsDiff25 { get; set; }

        #endregion

        // Ctor
        public ObjectiveList() {
            EventIdx = new Dictionary<ObjectiveEvent, List<int>> () {
                { ObjectiveEvent.TOWER_DESTROYED, new List<int>() },
                { ObjectiveEvent.DRAGON_KILL, new List<int>() },
                { ObjectiveEvent.BARON_KILL, new List<int>() },
                { ObjectiveEvent.HERALD_KILL, new List<int>() },
                { ObjectiveEvent.INHIBITOR_DESTROYED, new List<int>() }
            };
            Objectives = new List<Objective>();
        }

        // Index overwrite
        public Objective this[int i] {
            get => Objectives[i];
            set => Objectives[i] = value;
        }

        // IEnumerable
        public IEnumerator GetEnumerator() {
            foreach (Objective obj in Objectives) {
                // Yield each day of the week.
                yield return obj;
            }
        }

        public int Count() {
            return Objectives.Count;
        }

        // Private Constants
        // --- ObjectiveEvent
        // "BUILDING_KILL";         "type"
        // "CHAMPION_KILL";         "type"
        // "ELITE_MONSTER_KILL";    "type"
        // "TOWER_BUILDING";        "buildingType"
        // "INHIBITOR_BUILDING";    "buildingType"
        // "DRAGON";                "monsterType"
        // "RIFTHERALD";            "monsterType"
        // "BARON_NASHOR";          "monsterType"

        // --- ObjectiveType
        // "OUTER_TURRET";          "towerType"
        // "INNER_TURRET";          "towerType"
        // "BASE_TURRET";           "towerType"
        // "NEXUS_TURRET";          "towerType"
        // "FIRE_DRAGON";           "monsterSubType"
        // "WATER_DRAGON";          "monsterSubType"
        // "EARTH_DRAGON";          "monsterSubType"
        // "AIR_DRAGON";            "monsterSubType"
        // "ELDER_DRAGON";          "monsterSubType"

        public void AddObjective(MatchEvent eventObj) {
            if (eventObj.EventType == MatchEventType.BuildingKill || eventObj.EventType == MatchEventType.EliteMonsterKill) {
                ObjectiveEvent enumEvent = ObjectiveEvent.NONE;
                ObjectiveType enumType = ObjectiveType.NONE;
                if (eventObj.EventType == MatchEventType.BuildingKill) {
                    if (eventObj.BuildingType == BuildingType.TowerBuilding) {
                        enumEvent = ObjectiveEvent.TOWER_DESTROYED;
                        enumType = (eventObj.TowerType == TowerType.OuterTurret) ? ObjectiveType.OUTER_TURRET :
                            (eventObj.TowerType == TowerType.InnerTurret) ? ObjectiveType.INNER_TURRET :
                            (eventObj.TowerType == TowerType.BaseTurret) ? ObjectiveType.BASE_TURRET :
                            (eventObj.TowerType == TowerType.NexusTurret) ? ObjectiveType.NEXUS_TURRET :
                            ObjectiveType.NONE;
                        if (eventObj.Timestamp.TotalMinutes < MasterWrapper.MINUTE_15) {
                            TowersAt15++;
                        }
                        if (eventObj.Timestamp.TotalMinutes < MasterWrapper.MINUTE_25) {
                            TowersAt25++;
                        }
                    }
                    else if (eventObj.BuildingType == BuildingType.InhibitorBuilding) {
                        enumEvent = ObjectiveEvent.INHIBITOR_DESTROYED;
                        enumType = ObjectiveType.INHIBITOR;
                    }
                    else { return; }
                }
                else {
                    // EliteMonsterKill
                    if (eventObj.MonsterType == MonsterType.Dragon) {
                        enumEvent = ObjectiveEvent.DRAGON_KILL;
                        enumType = (eventObj.MonsterSubType == MonsterSubType.AirDragon) ? ObjectiveType.AIR_DRAGON :
                            (eventObj.MonsterSubType == MonsterSubType.FireDragon) ? ObjectiveType.FIRE_DRAGON :
                            (eventObj.MonsterSubType == MonsterSubType.EarthDragon) ? ObjectiveType.EARTH_DRAGON :
                            (eventObj.MonsterSubType == MonsterSubType.WaterDragon) ? ObjectiveType.WATER_DRAGON :
                            (eventObj.MonsterSubType == MonsterSubType.ElderDragon) ? ObjectiveType.ELDER_DRAGON :
                            ObjectiveType.NONE;
                    }
                    else if (eventObj.MonsterType == MonsterType.RiftHerald) {
                        enumEvent = ObjectiveEvent.HERALD_KILL;
                        enumType = ObjectiveType.RIFT_HERALD;
                    }
                    else if (eventObj.MonsterType == MonsterType.BaronNashor) {
                        enumEvent = ObjectiveEvent.BARON_KILL;
                        enumType = ObjectiveType.BARON_NASHOR;
                    }
                    else { return; }
                }
                var objective = new Objective(enumEvent,
                        enumType,
                        eventObj.Timestamp,
                        eventObj.LaneType,
                        null);
                Objectives.Add(objective);
                EventIdx[enumEvent].Add(Objectives.Count - 1);
            }
            else if (eventObj.EventType == MatchEventType.ChampionKill) {
                if (eventObj.Timestamp.TotalMinutes < MasterWrapper.MINUTE_15) {
                    KillsAt15++;
                }
                if (eventObj.Timestamp.TotalMinutes < MasterWrapper.MINUTE_25) {
                    KillsAt25++;
                }
            }
        }

        public void UpdateBaronPP(List<MatchFrame> frameList, HashSet<string> teamPartIds, HashSet<string> oppPartIds) {
            foreach (int idx in EventIdx[ObjectiveEvent.BARON_KILL]) {
                TimeSpan tsAtKill = Objectives[idx].Timestamp;
                int? teamGoldAtKill = TeamGoldAtTimeStamp(tsAtKill, frameList, teamPartIds);
                int? oppGoldAtKill = TeamGoldAtTimeStamp(tsAtKill, frameList, oppPartIds);
                if (teamGoldAtKill == null || oppGoldAtKill == null) { continue; }
                TimeSpan tsAtExpire = tsAtKill + TimeSpan.FromMinutes(BARON_DURATION);
                int? teamGoldAtExpire = TeamGoldAtTimeStamp(tsAtExpire, frameList, teamPartIds);
                int? oppGoldAtExpire = TeamGoldAtTimeStamp(tsAtExpire, frameList, oppPartIds);
                if (teamGoldAtExpire == null || oppGoldAtExpire == null) { continue; }
                int? baronPP = (teamGoldAtExpire - teamGoldAtKill) - (oppGoldAtExpire - oppGoldAtKill);
                Objectives[idx].BaronPowerPlay = baronPP;
            }
        }

        public void SetDiffValues(ObjectiveList oppObjs) {
            TowersDiff15 = TowersAt15 - oppObjs.TowersAt15;
            KillsDiff15 = KillsAt15 - oppObjs.KillsAt15;
            TowersDiff25 = TowersAt25 - oppObjs.TowersAt25;
            KillsDiff25 = KillsAt25 - oppObjs.KillsAt25;
        }

        #region Private Functions

        private int? TeamGoldAtTimeStamp(TimeSpan timeStamp, List<MatchFrame> frameList, HashSet<string> teamPartIds) {
            int tsMinute = (int)timeStamp.TotalMinutes;
            int tsSecond = (int)timeStamp.TotalSeconds % 60;
            if (tsMinute + 1 >= frameList.Count) { return null; } // Means out of Range

            // Take team Gold at the marked minute, and minute + 1. Average
            int teamGoldAtMinute = TeamGoldAtFrame(frameList[tsMinute].ParticipantFrames, teamPartIds);
            int teamGoldAtMinutePlus1 = TeamGoldAtFrame(frameList[tsMinute + 1].ParticipantFrames, teamPartIds);
            decimal goldPerSecond = (teamGoldAtMinutePlus1 - teamGoldAtMinute) / (decimal)60;
            return teamGoldAtMinute + (int)(goldPerSecond * tsSecond);
        }

        private int TeamGoldAtFrame(Dictionary<string, ParticipantFrame> frame, HashSet<string> teamPartIds) {
            int teamGold = 0;
            foreach (string id in teamPartIds) {
                teamGold += frame[id].TotalGold;
            }
            return teamGold;
        }

        #endregion
    }
}
