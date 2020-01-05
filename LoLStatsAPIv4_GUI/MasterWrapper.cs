using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using RiotSharp;
using RiotSharp.Misc;
using RiotSharp.Endpoints.MatchEndpoint;

namespace LoLStatsAPIv4_GUI {
    public class MasterWrapper {

        #region Private Consts

        private const string SOLO_QUEUE_STRING = "RANKED_SOLO_5x5";
        private const string FLEX_QUEUE_STRING = "RANKED_FLEX_SR";
        public const string SIG_FIGS = "{0:F2}";
        public const string PCT_FRM = "{0:0.00%}";
        public const int BLUE_ID = 100;
        public const int RED_ID = 200;
        public const int MINUTE_15 = 15;
        public const int MINUTE_25 = 25;
        // T = Table Name
        // C = Column Name
        private const string T_SUMMONERS = "Summoners";
        private const string T_COMPETITIONS = "Competitions";
        private const string T_REGISTEREDPLAYERS = "RegisteredPlayers";
        private const string T_TEAMS = "Teams";
        private const string T_MATCHES = "Matches";
        private const string T_CHAMPIONS = "Champions";
        private const string T_PLAYERSTATS = "PlayerStats";
        private const string T_BANNEDCHAMPS = "BannedChamps";
        private const string T_OBJECTIVES = "Objectives";
        private const string T_TEAMSTATS = "TeamStats";
        // Misc
        private const string C_ACCID = "accountID";
        private const string C_SUMMID = "summonerID";
        private const string C_NAME = "name";
        private const string C_STIER = "soloTier";
        private const string C_SDIV = "soloDiv";
        private const string C_FTIER = "flexTier";
        private const string C_FDIV = "flexDiv";
        private const string C_ID = "ID";
        private const string C_TYPE = "type";
        private const string C_COMPNAME = "competitionName";
        private const string C_COMPID = "competitionID";
        private const string C_LASTUPDATE = "lastUpdated";
        private const string C_TEAMID = "teamID";
        private const string C_MATCHID = "matchID";
        private const string C_CHAMPID = "champID";
        // Matches
        private const string C_REDTEAMID = "redTeamID";
        private const string C_BLUETEAMID = "blueTeamID";
        private const string C_DURATION = "duration";
        private const string C_PATCH = "patch";
        private const string C_CREATION = "dateCreated";
        private const string C_GAMENUM = "gameNumber";
        // BannedChamps
        private const string C_TEAMBANID = "teamBanID";
        private const string C_TEAMBANNEDID = "teamBannedAgainstID";
        private const string C_BANORDER = "banOrder";
        // Objectives
        private const string C_OBJEVENT = "objEvent";
        private const string C_OBJTYPE = "objType";
        private const string C_TIMESTAMP = "timeStamp";
        private const string C_LANE = "lane";
        private const string C_BARONPP = "baronPowerPlay";
        // PlayerStats & MatchStats
        private const string C_ROLE = "role";
        private const string C_WIN = "win";
        private const string C_KDA = "KDA";
        private const string C_KILLP = "killParticipation";
        private const string C_DEATHP = "deathParticipation";
        private const string C_DMGDEALTCHMPSPERMIN = "dmgDealtChampsPerMin";
        private const string C_DMGDEALTOBJPERMIN = "dmgDealtObjectivesPerMin";
        private const string C_DMGTAKEN = "dmgTakenPerMin";
        private const string C_GOLDPERMIN = "goldPerMin";
        private const string C_CSPERMIN = "csPerMin";
        private const string C_VSPERMIN = "vsPerMin";
        private const string C_FBLOODKILL = "firstBloodKill";
        private const string C_FBLOODASSIST = "firstBloodAssist";
        private const string C_FTOWER = "firstTower";
        private const string C_CSAT15 = "csAt15";
        private const string C_CSDIFF15 = "csDiff15";
        private const string C_GOLDAT15 = "goldAt15";
        private const string C_GOLDDIFF15 = "goldDiff15";
        private const string C_XPAT15 = "xpAt15";
        private const string C_XPDIFF15 = "xpDiff15";
        private const string C_JGCSAT15 = "jungleCSAt15";
        private const string C_JGCSDIFF15 = "jungleCSDiff15";
        private const string C_CSAT25 = "csAt25";
        private const string C_CSDIFF25 = "csDiff25";
        private const string C_GOLDAT25 = "goldAt25";
        private const string C_GOLDDIFF25 = "goldDiff25";
        private const string C_XPAT25 = "xpAt25";
        private const string C_XPDIFF25 = "xpDiff25";
        private const string C_KILLS = "kills";
        private const string C_DEATHS = "deaths";
        private const string C_ASSISTS = "assists";
        private const string C_DMGDEALTCHAMPS = "dmgDealtChamps";
        private const string C_GOLD = "gold";
        private const string C_CREEPSCORE = "creepScore";
        private const string C_VISIONSCORE = "visionScore";
        private const string C_DKILL = "doubleKills";
        private const string C_TKILL = "tripleKills";
        private const string C_QKILL = "quadraKills";
        private const string C_PKILL = "pentaKills";
        private const string C_SIDE = "side";
        private const string C_FBLOOD = "firstBlood";
        private const string C_FDRAG = "firstDragon";
        private const string C_FRIFT = "firstRiftHerald";
        private const string C_KILLSAT15 = "killsAt15";
        private const string C_KILLSDIFF15 = "killsDiff15";
        private const string C_TOWERSAT15 = "towersAt15";
        private const string C_TOWERSDIFF15 = "towersDiff15";
        private const string C_KILLSAT25 = "killsAt25";
        private const string C_KILLSDIFF25 = "killsDiff25";
        private const string C_TOWERSAT25 = "towersAt25";
        private const string C_TOWERSDIFF25 = "towersDiff25";
        private const string C_TOTKILLS = "totalKills";
        private const string C_TOTDEATHS = "totalDeaths";
        private const string C_TOTASSISTS = "totalAssists";
        private const string C_TOTDMGDEALTCHAMPS = "totalDmgDealtChamps";
        private const string C_TOTGOLD = "totalGold";
        private const string C_TOTCS = "totalCreepScore";
        private const string C_TOTVS = "totalVisionScore";

        #endregion

        #region Private Functions / Member Variables

        // Cache Storage
        private static RiotApi apiDev;
        private static readonly Map<string, int> cacheComp = new Map<string, int>();     
        // Key1: CompName [Type], Key2: compID
        private static readonly Map<string, int> cacheChamp = new Map<string, int>();
        // Key1: Champion Name, Key2: champID
        private static readonly Map<string, int> cacheTeam = new Map<string, int>();
        // Key1: Team Name, Key2: teamID
        private static readonly Map<string, string> cacheSummoner = new Map<string, string>();
        // Key1: Summoner Name, Key 2: summonerID
        private static HashSet<string> cacheMatch = new HashSet<string>();

        private const int RETRY_ATTEMPTS = 5;
        // In case Task.Wait() returns an exception, we don't want to Exit
        private static bool WaitTaskPassException(dynamic task, APIParam type, string param) {
            bool taskCompleted = false;
            int retry = 0;
            while (!taskCompleted && retry < RETRY_ATTEMPTS) {
                try {
                    task.Wait();
                    taskCompleted = true;
                    LogClass.APICalled(type, param, false);
                }
                catch (Exception ex) {
                    LogClass.APICalled(type, param, true);
                    if (ex.InnerException is RiotSharpRateLimitException RateLimitEx) {
                        LogClass.WriteLogLine("Error " + RateLimitEx.HttpStatusCode + ". Rate Limit Reached! Wait for " + RateLimitEx.RetryAfter);
                        Cursor.Current = Cursors.WaitCursor;
                        Task.Delay(RateLimitEx.RetryAfter);
                        Cursor.Current = Cursors.Default;
                        retry++;
                    }
                    else {
                        LogClass.WriteLogLine("Error: " + ex.Message);
                        return false;
                    }
                }
            }
            if (retry >= RETRY_ATTEMPTS) {
                MessageBox.Show("Riot API down for " + type.ToString() + " endpoint.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        #endregion

        #region Public Functions

        #region Public Static Calls

        // Whenever textbox updates
        public static void UpdateAPIDevInstance(string key) {
            apiDev = RiotApi.GetDevelopmentInstance(key);
        }

        // Whenever connection string updates
        public static void UpdateConnectionString(string connection) {
            DBWrapper.ConnectionString = connection;
        }

        // Using cacheTeam
        public static int GetTeamID(string teamName) {
            if (cacheTeam.Forward.ContainsKey(teamName)) {
                return cacheTeam.Forward[teamName];
            }
            else {
                return -1;
            }
        }

        public static string GetTeamName(int teamId) {
            return cacheTeam.Reverse[teamId];
        }

        // Using cacheChamp
        public static string GetChampName(int id) {
            return cacheChamp.Reverse[id];
        }

        // Using cacheSummoner
        public static string GetSummonerName(string id) {
            if (cacheSummoner.Reverse.ContainsKey(id)) {
                return cacheSummoner.Reverse[id];
            }
            else {
                return "";
            }
        }
        public static string GetSummonerID(string name) {
            if (cacheSummoner.Forward.ContainsKey(name)) {
                return cacheSummoner.Forward[name];
            }
            else {
                return null;
            }
        }

        public static bool IsMatchIDInCache(string id) {
            return cacheMatch.Contains(id);
        }

        #endregion

        #region API Calls

        // DB Table: Competitions, Summoners, RegisteredPlayers
        // API Input: Summoner Names
        // API Output: Summoner IDs
        // APIKey Usage: 1 call for new Summoner
        // Essentially adds them into the Summoners database forever
        public static string AddSummonerIntoDBAndCache(string compName, string teamName, string summName) {

            // Key: SummonerId -> Values: AccountId, Name
            // Before calling the API, check to see if that summonerName exists in db Table Summoners
            string summId = "", accId = null;
            if (cacheSummoner.Forward.ContainsKey(summName)) {
                summId = cacheSummoner.Forward[summName];
            }
            else {
                // New summoner name
                var summonerTask = apiDev.Summoner.GetSummonerByNameAsync(Region.Na, summName);
                if (WaitTaskPassException(summonerTask, APIParam.SUMMONER_NAME, summName)) {
                    var summonerObj = summonerTask.Result;
                    summId = summonerObj.Id;
                    accId = summonerObj.AccountId;
                }
                else {
                    // Summoner name does not exist
                    MessageBox.Show("Summoner name does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }

            // Check if summonerID exists in the db Table Summoners
            // If so, they had a name change recently
            string retName = null;
            if (!cacheSummoner.Reverse.ContainsKey(summId)) {
                // ID does not exist. INSERT new summoner
                var insert = new Dictionary<string, Tuple<DB, string>>();
                insert.Add(C_ACCID, new Tuple<DB, string>(DB.INSERT, accId));
                insert.Add(C_SUMMID, new Tuple<DB, string>(DB.INSERT, summId));
                insert.Add(C_NAME, new Tuple<DB, string>(DB.INSERT, summName));
                DBWrapper.DBInsertIntoTable(T_SUMMONERS, insert);
                cacheSummoner.Add(summName, summId);

                retName = summName;
            }
            else {
                string oldName = cacheSummoner.Reverse[summId];

                // ID does exist. UPDATE summoner name
                var update = new Dictionary<string, Tuple<DB, string>>();
                update.Add(C_SUMMID, new Tuple<DB, string>(DB.WHERE, summId));
                update.Add(C_NAME, new Tuple<DB, string>(DB.SET, summName));
                DBWrapper.DBUpdateTable(T_SUMMONERS, update);
                cacheSummoner.RemoveBackward(summId);
                cacheSummoner.Add(summName, summId);

                retName = oldName;
            }

            // Add to RegisteredPlayers if not yet in Competition
            int compID = cacheComp.Forward[compName];
            int teamID = cacheTeam.Forward[teamName];
            var readConds = new Dictionary<string, Tuple<DB, string>>();
            readConds.Add(C_COMPID, new Tuple<DB, string>(DB.WHERE, compID.ToString()));
            readConds.Add(C_SUMMID, new Tuple<DB, string>(DB.WHERE, summId));
            readConds.Add(C_TEAMID, new Tuple<DB, string>(DB.WHERE, teamID.ToString()));
            if (!DBWrapper.DBTableHasEntry(T_REGISTEREDPLAYERS, readConds)) {
                var insertComp = new Dictionary<string, Tuple<DB, string>>();
                insertComp.Add(C_COMPID, new Tuple<DB, string>(DB.INSERT, compID.ToString()));
                insertComp.Add(C_SUMMID, new Tuple<DB, string>(DB.INSERT, summId));
                insertComp.Add(C_TEAMID, new Tuple<DB, string>(DB.INSERT, teamID.ToString()));
                DBWrapper.DBInsertIntoTable(T_REGISTEREDPLAYERS, insertComp);
            }
            return retName;
        }

        // DB Table: Registered Players, Summoners, 
        // API Input: Summoner IDs (from Table RegisteredPlayers)
        // API Output: Summoner Ranks
        // APIKey Usage: 1 per Summoner in selected Competition
        public static bool CompetitionUpdateSummonerRanks(string compName) {
            if (compName.Length == 0) { return false; }
            string compID = cacheComp.Forward[compName].ToString();

            var summIdList = new List<string>();
            var conds = new Dictionary<string, Tuple<DB, string>>();
            conds.Add(C_COMPID, new Tuple<DB, string>(DB.WHERE, compID));
            var objMap = DBWrapper.DBReadFromTable(T_REGISTEREDPLAYERS, conds);
            foreach (var row in objMap) {
                summIdList.Add(row[C_SUMMID].ToString());
            }

            // Now update all the Solo queue and Flex queue rankings
            foreach (string summId in summIdList) {
                var leagueTask = apiDev.League.GetLeagueEntriesBySummonerAsync(Region.Na, summId);
                if (!WaitTaskPassException(leagueTask, APIParam.LEAGUES, summId)) {
                    return false;
                }
                var leagueObj = leagueTask.Result;
                var timeNow = DateTime.Now;

                conds = new Dictionary<string, Tuple<DB, string>>();
                conds.Add(C_SUMMID, new Tuple<DB, string>(DB.WHERE, summId));
                conds.Add(C_LASTUPDATE, new Tuple<DB, string>(DB.SET, timeNow.ToString()));

                string sTier = null, sDiv = null, fTier = null, fDiv = null;
                foreach (var leagueEntry in leagueObj) {
                    string queueType = leagueEntry.QueueType;
                    if (queueType == SOLO_QUEUE_STRING) {
                        sTier = leagueEntry.Tier;
                        sDiv = leagueEntry.Rank;
                        conds.Add(C_STIER, new Tuple<DB, string>(DB.SET, sTier));
                        conds.Add(C_SDIV, new Tuple<DB, string>(DB.SET, sDiv));
                    }
                    else if (queueType == FLEX_QUEUE_STRING) {
                        fTier = leagueEntry.Tier;
                        fDiv = leagueEntry.Rank;
                        conds.Add(C_FTIER, new Tuple<DB, string>(DB.SET, fTier));
                        conds.Add(C_FDIV, new Tuple<DB, string>(DB.SET, fDiv));
                    }
                }
                DBWrapper.DBUpdateTable(T_SUMMONERS, conds);
            }

            return true;
        }

        // DB Table: Matches, PlayerStats, TeamStats, BannedChamps, Objectives
        // API Input: Match ID
        // API Output: Every applicable stat in that match
        // APIKey Usage: 2
        public static string LoadNewMatchStatsIntoDB(long matchID, string compName, string blueTeamName, string redTeamName) {

            var matchTuple = GetAPIMatchTuple(matchID);
            if (matchTuple == null) { return null; }
            Match matchInfoObj = matchTuple.Item1;
            MatchTimeline matchTimelineObj = matchTuple.Item2;
            int blueTeamID = GetTeamID(blueTeamName);
            int redTeamID = GetTeamID(redTeamName);
            MatchStats match = new MatchStats(cacheComp.Forward[compName], compName, 0);
            if (!match.InitializeClassWithWindow(matchInfoObj, matchTimelineObj, blueTeamID, redTeamID)) {
                return null;
            }

            InsertMatchStatsIntoDB(match, blueTeamID, redTeamID);
            return matchID.ToString();
        }

        // DB Table: Matches, PlayerStats, TeamStats, BannedChamps, Objectives
        // API Input: Match ID
        // API Output: Every applicable stat in that match
        // APIKey Usage: 2
        public static string LoadEditedMatchStatsIntoDB(long matchID) {

            var columns = new Dictionary<string, Tuple<DB, string>>();
            columns.Add(C_ID, new Tuple<DB, string>(DB.WHERE, matchID.ToString()));
            var matchDBObj = DBWrapper.DBReadFromTable(T_MATCHES, columns)[0];

            int compID = Convert.ToInt32(matchDBObj[C_COMPID]);
            int blueTeamID = Convert.ToInt32(matchDBObj[C_BLUETEAMID]);
            int redTeamID = Convert.ToInt32(matchDBObj[C_REDTEAMID]);
            int gameNumber = Convert.ToInt32(matchDBObj[C_GAMENUM]);
            var blueTeamDict = GetTeamDictFromDB(matchID, blueTeamID);
            var redTeamDict = GetTeamDictFromDB(matchID, redTeamID);

            var matchTuple = GetAPIMatchTuple(matchID);
            if (matchTuple == null) { return null; }
            Match matchInfoObj = matchTuple.Item1;
            MatchTimeline matchTimelineObj = matchTuple.Item2;
            MatchStats match = new MatchStats(compID, cacheComp.Reverse[compID], gameNumber);
            if (!match.InitializeClassWithWindow(matchInfoObj, matchTimelineObj, blueTeamID, redTeamID, blueTeamDict, redTeamDict)) {
                return null;
            }

            DeleteMatchStatsFromDB(matchID);
            InsertMatchStatsIntoDB(match, blueTeamID, redTeamID);

            return matchID.ToString();
        }

        // Temporary bug fixes at the touch of a button
        // Should be a removed function but whatever
        // DB Table: Objectives
        // API Input: Match ID
        // API Output: Every applicable stat in that match
        // APIKey Usage: 2
        public static bool ArbitraryBugFix() {
            var DictList = DBWrapper.DBReadFromTable(T_MATCHES);
            foreach (var matchObj in DictList) {
                int compId = Convert.ToInt32(matchObj[C_COMPID]);
                int blueTeamId = Convert.ToInt32(matchObj[C_BLUETEAMID]);
                int redTeamId = Convert.ToInt32(matchObj[C_REDTEAMID]);
                int gameNum = Convert.ToInt32(matchObj[C_GAMENUM]);
                long matchId = Convert.ToInt64(matchObj[C_ID]);
                var blueTeamDict = GetTeamDictFromDB(matchId, blueTeamId);
                var redTeamDict = GetTeamDictFromDB(matchId, redTeamId);

                var matchTuple = GetAPIMatchTuple(matchId);
                if (matchTuple == null) { return false; }
                Match matchInfoObj = matchTuple.Item1;
                MatchTimeline matchTimelineObj = matchTuple.Item2;
                MatchStats matchInst = new MatchStats(compId, cacheComp.Reverse[compId], gameNum);
                matchInst.InitializeClassWithoutWindow(matchInfoObj, matchTimelineObj, blueTeamId, redTeamId, blueTeamDict, redTeamDict);

                // EDIT-ABLE ONLY FOR BUG FIXES
                // Update PlayerStats: dmgDealtChamps, gold
                // Update TeamStats: csPerMin, totalDmgDealtChamps, totalGold, totalCreepScore, totalVisionScore
                Team blueTeam = matchInst.BlueTeam;
                Team redTeam = matchInst.RedTeam;
                // Blue Players
                foreach (Player player in blueTeam.Players) {
                    var columns = new Dictionary<string, Tuple<DB, string>>();

                    columns.Add(C_MATCHID, new Tuple<DB, string>(DB.WHERE, matchId.ToString()));
                    columns.Add(C_SUMMID, new Tuple<DB, string>(DB.WHERE, player.SummonerId.ToString()));
                    columns.Add(C_DMGDEALTCHAMPS, new Tuple<DB, string>(DB.SET, player.DamageToChamps.ToString()));
                    columns.Add(C_GOLD, new Tuple<DB, string>(DB.SET, player.Gold.ToString()));
                    DBWrapper.DBUpdateTable(T_PLAYERSTATS, columns);
                }
                // Red Players
                foreach (Player player in redTeam.Players) {
                    var columns = new Dictionary<string, Tuple<DB, string>>();

                    columns.Add(C_MATCHID, new Tuple<DB, string>(DB.WHERE, matchId.ToString()));
                    columns.Add(C_SUMMID, new Tuple<DB, string>(DB.WHERE, player.SummonerId.ToString()));
                    columns.Add(C_DMGDEALTCHAMPS, new Tuple<DB, string>(DB.SET, player.DamageToChamps.ToString()));
                    columns.Add(C_GOLD, new Tuple<DB, string>(DB.SET, player.Gold.ToString()));
                    DBWrapper.DBUpdateTable(T_PLAYERSTATS, columns);
                }
                var teamCols = new Dictionary<string, Tuple<DB, string>>();
                // Blue Team
                teamCols.Add(C_MATCHID, new Tuple<DB, string>(DB.WHERE, matchId.ToString()));
                teamCols.Add(C_TEAMID, new Tuple<DB, string>(DB.WHERE, blueTeam.TeamId.ToString()));
                teamCols.Add(C_CSPERMIN, new Tuple<DB, string>(DB.SET, blueTeam.GetCreepScorePerMinute().ToString()));
                teamCols.Add(C_TOTDMGDEALTCHAMPS, new Tuple<DB, string>(DB.SET, blueTeam.GetTotalDamageToChamps().ToString()));
                teamCols.Add(C_TOTGOLD, new Tuple<DB, string>(DB.SET, blueTeam.GetTotalGold().ToString()));
                teamCols.Add(C_TOTCS, new Tuple<DB, string>(DB.SET, blueTeam.GetTotalCreepScore().ToString()));
                teamCols.Add(C_TOTVS, new Tuple<DB, string>(DB.SET, blueTeam.GetTotalVisionScore().ToString()));
                DBWrapper.DBUpdateTable(T_TEAMSTATS, teamCols);
                // Red Team
                teamCols[C_TEAMID] = new Tuple<DB, string>(DB.WHERE, redTeam.TeamId.ToString());
                teamCols[C_CSPERMIN] = new Tuple<DB, string>(DB.SET, redTeam.GetCreepScorePerMinute().ToString());
                teamCols[C_TOTDMGDEALTCHAMPS] = new Tuple<DB, string>(DB.SET, redTeam.GetTotalDamageToChamps().ToString());
                teamCols[C_TOTGOLD] = new Tuple<DB, string>(DB.SET, redTeam.GetTotalGold().ToString());
                teamCols[C_TOTCS] = new Tuple<DB, string>(DB.SET, redTeam.GetTotalCreepScore().ToString());
                teamCols[C_TOTVS] = new Tuple<DB, string>(DB.SET, redTeam.GetTotalVisionScore().ToString());
                DBWrapper.DBUpdateTable(T_TEAMSTATS, teamCols);
            }
            return true;
        }

        #region Load MatchStats Helper Functions

        private static void InsertUltimateHelper(string tableName, Dictionary<string, object> insertElements) {
            var columns = new Dictionary<string, Tuple<DB, string>>();
            foreach (string colName in insertElements.Keys) {
                columns.Add(colName, new Tuple<DB, string>(DB.INSERT, insertElements[colName].ToString()));
            }
            DBWrapper.DBInsertIntoTable(tableName, columns);
        }

        private static void InsertMatchStatsTable(MatchStats match) {
            var columns = new Dictionary<string, object>();
            columns.Add(C_ID, match.MatchID);
            columns.Add(C_COMPID, match.CompetitionID);
            columns.Add(C_BLUETEAMID, match.BlueTeam.TeamId);
            columns.Add(C_REDTEAMID, match.RedTeam.TeamId);
            columns.Add(C_DURATION, match.GetDurationSeconds());
            columns.Add(C_PATCH, match.GetPatch());
            columns.Add(C_CREATION, match.MatchCreation);
            columns.Add(C_GAMENUM, match.GameNumber);
            InsertUltimateHelper(T_MATCHES, columns);
        }

        private static void InsertBannedChampsTable(Team team, int bannedAgainstID, long matchID) {
            var columns = new Dictionary<string, object>();
            for (int i = 0; i < team.Bans.Count; ++i) {
                columns.Clear();
                columns.Add(C_MATCHID, matchID);
                columns.Add(C_TEAMBANID, team.TeamId);
                columns.Add(C_TEAMBANNEDID, bannedAgainstID);
                columns.Add(C_CHAMPID, team.Bans[i].ID);
                columns.Add(C_BANORDER, i + 1);
                InsertUltimateHelper(T_BANNEDCHAMPS, columns);
            }
        }

        private static void InsertObjectivesTable(Team team, long matchID) {
            var columns = new Dictionary<string, object>();
            foreach (Objective Obj in team.Objectives) {
                columns.Clear();
                columns.Add(C_MATCHID, matchID);
                columns.Add(C_TEAMID, team.TeamId);
                columns.Add(C_OBJEVENT, Obj.Event.GetString());
                columns.Add(C_OBJTYPE, Obj.Type.GetString());
                columns.Add(C_TIMESTAMP, Convert.ToInt32(Obj.Timestamp.TotalSeconds));
                if (Obj.Lane != null) { columns.Add(C_LANE, Obj.Lane.ToString().Replace("Lane", "")); }
                if (Obj.BaronPowerPlay != null) { columns.Add(C_BARONPP, Obj.BaronPowerPlay); }
                InsertUltimateHelper(T_OBJECTIVES, columns);
            }
        }

        private static string SigFigs(decimal val) {
            return string.Format(SIG_FIGS, val);
        }

        private static string PctFigs(decimal val) {
            return string.Format(PCT_FRM, val);
        }

        private static void InsertPlayerStatsTable(Team team, long matchID) {
            var columns = new Dictionary<string, object>();
            foreach (Player player in team.Players) {
                columns.Clear();
                columns.Add(C_SUMMID, player.SummonerId);
                columns.Add(C_MATCHID, matchID);
                columns.Add(C_TEAMID, team.TeamId);
                columns.Add(C_ROLE, player.Role);
                columns.Add(C_CHAMPID, player.ChampId);
                columns.Add(C_WIN, player.Win);
                columns.Add(C_KDA, (player.GetKDA() == -1) ? "Perfect" : SigFigs(player.GetKDA()));
                columns.Add(C_KILLP, SigFigs(player.GetKillParticipation(team.GetTotalKills())));
                columns.Add(C_DEATHP, SigFigs(player.GetDeathParticipation(team.GetTotalDeaths())));
                columns.Add(C_DMGDEALTCHMPSPERMIN, SigFigs(player.GetDamageToChampsPerMinute()));
                columns.Add(C_DMGDEALTOBJPERMIN, SigFigs(player.GetDamageToObjectivesPerMinute()));
                columns.Add(C_DMGTAKEN, SigFigs(player.GetDamageTakenPerMinute()));
                columns.Add(C_GOLDPERMIN, SigFigs(player.GetGoldPerMinute()));
                columns.Add(C_CSPERMIN, SigFigs(player.GetCSPerMinute()));
                columns.Add(C_VSPERMIN, SigFigs(player.GetVSPerMinute()));
                columns.Add(C_FBLOODKILL, player.FirstBloodKill);
                columns.Add(C_FBLOODASSIST, player.FirstBloodAssist);
                columns.Add(C_FTOWER, player.FirstTower);
                columns.Add(C_CSAT15, player.CSAt15);
                columns.Add(C_CSDIFF15, player.CSDiff15);
                columns.Add(C_GOLDAT15, player.GoldAt15);
                columns.Add(C_GOLDDIFF15, player.GoldDiff15);
                columns.Add(C_XPAT15, player.XPAt15);
                columns.Add(C_XPDIFF15, player.XPDiff15);
                columns.Add(C_JGCSAT15, player.JungleCSAt15);
                columns.Add(C_JGCSDIFF15, player.JungleCSDiff15);
                if (player.CSAt25 != null) { columns.Add(C_CSAT25, player.CSAt25); }
                if (player.CSDiff25 != null) { columns.Add(C_CSDIFF25, player.CSDiff25); }
                if (player.GoldAt25 != null) { columns.Add(C_GOLDAT25, player.GoldAt25); }
                if (player.GoldDiff25 != null) { columns.Add(C_GOLDDIFF25, player.GoldDiff25); }
                if (player.XPAt25 != null) { columns.Add(C_XPAT25, player.XPAt25); }
                if (player.XPDiff25 != null) { columns.Add(C_XPDIFF25, player.XPDiff25); }
                columns.Add(C_KILLS, player.Kills);
                columns.Add(C_DEATHS, player.Deaths);
                columns.Add(C_ASSISTS, player.Assists);
                columns.Add(C_DMGDEALTCHAMPS, player.DamageToChamps);
                columns.Add(C_GOLD, player.Gold);
                columns.Add(C_CREEPSCORE, player.CreepScore);
                columns.Add(C_VISIONSCORE, player.VisionScore);
                columns.Add(C_DKILL, player.GetDoubleKills());
                columns.Add(C_TKILL, player.GetTripleKills());
                columns.Add(C_QKILL, player.GetQuadraKills());
                columns.Add(C_PKILL, player.GetPentaKills());
                InsertUltimateHelper(T_PLAYERSTATS, columns);
            }
        }

        private static void InsertTeamStatsTable(Team team, long matchID) {
            var columns = new Dictionary<string, object>();
            columns.Add(C_MATCHID, matchID);
            columns.Add(C_TEAMID, team.TeamId);
            if (team.Side == BLUE_ID) { columns.Add(C_SIDE, "BLUE"); }
            else { columns.Add(C_SIDE, "RED"); }
            columns.Add(C_WIN, team.Win);
            columns.Add(C_KDA, (team.GetTotalKDA() == -1) ? "Perfect" : SigFigs(team.GetTotalKDA()));
            columns.Add(C_DMGDEALTCHMPSPERMIN, SigFigs(team.GetDamageToChampsPerMinute()));
            columns.Add(C_DMGDEALTOBJPERMIN, SigFigs(team.GetDamageToObjectivesPerMinute()));
            columns.Add(C_GOLDPERMIN, SigFigs(team.GetGoldPerMinute()));
            columns.Add(C_CSPERMIN, SigFigs(team.GetCreepScorePerMinute()));
            columns.Add(C_VSPERMIN, SigFigs(team.GetVisionScorePerMinute()));
            columns.Add(C_FBLOOD, team.FirstBlood);
            columns.Add(C_FTOWER, team.FirstTower);
            columns.Add(C_FDRAG, team.FirstDragon);
            columns.Add(C_FRIFT, team.FirstRiftHerald);
            columns.Add(C_GOLDAT15, team.GetGoldAt15());
            columns.Add(C_GOLDDIFF15, team.GetGoldDiff15());
            columns.Add(C_XPAT15, team.GetXPAt15());
            columns.Add(C_XPDIFF15, team.GetXPDiff15());
            columns.Add(C_TOWERSAT15, team.GetTowersAt15());
            columns.Add(C_TOWERSDIFF15, team.GetTowersDiff15());
            columns.Add(C_KILLSAT15, team.GetKillsAt15());
            columns.Add(C_KILLSDIFF15, team.GetKillsDiff15());
            if (team.GetGoldAt25() != null) { columns.Add(C_GOLDAT25, team.GetGoldAt25()); }
            if (team.GetGoldDiff25() != null) { columns.Add(C_GOLDDIFF25, team.GetGoldDiff25()); }
            if (team.GetXPAt25() != null) { columns.Add(C_XPAT25, team.GetXPAt25()); }
            if (team.GetXPDiff25() != null) { columns.Add(C_XPDIFF25, team.GetXPDiff25()); }
            if (team.GetTowersAt25() != null) { columns.Add(C_TOWERSAT25, team.GetTowersAt25()); }
            if (team.GetTowersDiff25() != null) { columns.Add(C_TOWERSDIFF25, team.GetTowersDiff25()); }
            if (team.GetKillsAt25() != null) { columns.Add(C_KILLSAT25, team.GetKillsAt25()); }
            if (team.GetKillsDiff25() != null) { columns.Add(C_KILLSDIFF25, team.GetKillsDiff25()); }
            columns.Add(C_TOTKILLS, team.GetTotalKills());
            columns.Add(C_TOTDEATHS, team.GetTotalDeaths());
            columns.Add(C_TOTASSISTS, team.GetTotalAssists());
            columns.Add(C_TOTDMGDEALTCHAMPS, team.GetTotalDamageToChamps());
            columns.Add(C_TOTGOLD, team.GetTotalGold());
            columns.Add(C_TOTCS, team.GetTotalCreepScore());
            columns.Add(C_TOTVS, team.GetTotalVisionScore());
            InsertUltimateHelper(T_TEAMSTATS, columns);
        }

        // DB Table: Matches, PlayerStats, TeamStats, BannedChamps, Objectives
        private static void InsertMatchStatsIntoDB(MatchStats match, int blueTeamID, int redTeamID) {
            // Matches Table
            InsertMatchStatsTable(match);
            // BannedChamps Table
            InsertBannedChampsTable(match.BlueTeam, redTeamID, match.MatchID);
            InsertBannedChampsTable(match.RedTeam, blueTeamID, match.MatchID);
            // Objectives Table
            InsertObjectivesTable(match.BlueTeam, match.MatchID);
            InsertObjectivesTable(match.RedTeam, match.MatchID);
            // PlayerStats Table
            InsertPlayerStatsTable(match.BlueTeam, match.MatchID);
            InsertPlayerStatsTable(match.RedTeam, match.MatchID);
            // TeamStats Table
            InsertTeamStatsTable(match.BlueTeam, match.MatchID);
            InsertTeamStatsTable(match.RedTeam, match.MatchID);
        }

        // DB Table: Matches, PlayerStats, TeamStats, BannedChamps, Objectives
        private static void DeleteMatchStatsFromDB(long matchID) {
            var columns = new Dictionary<string, Tuple<DB, string>>();
            columns.Add(C_MATCHID, new Tuple<DB, string>(DB.WHERE, matchID.ToString()));
            DBWrapper.DBDeleteFromTable(T_TEAMSTATS, columns);
            DBWrapper.DBDeleteFromTable(T_PLAYERSTATS, columns);
            DBWrapper.DBDeleteFromTable(T_BANNEDCHAMPS, columns);
            DBWrapper.DBDeleteFromTable(T_OBJECTIVES, columns);

            columns.Remove(C_MATCHID);
            columns.Add(C_ID, new Tuple<DB, string>(DB.WHERE, matchID.ToString()));
            DBWrapper.DBDeleteFromTable(T_MATCHES, columns);
        }

        // Private Helper for 3 different Match Scenarios
        // 1) Adding stats from a New Match
        // 2) Updating role names
        // 3) Bug fixing
        // API Input: Match ID
        // API Output: Every applicable stat in that match
        // APIKey Usage: 2
        private static Tuple<Match, MatchTimeline> GetAPIMatchTuple(long matchId) {
            var matchTimelineTask = apiDev.Match.GetMatchTimelineAsync(Region.Na, matchId);
            var matchInfoTask = apiDev.Match.GetMatchAsync(Region.Na, matchId);
            if (!WaitTaskPassException(matchInfoTask, APIParam.MATCH, matchId.ToString())) {
                return null;
            }
            var matchInfoObj = matchInfoTask.Result;
            if (!WaitTaskPassException(matchTimelineTask, APIParam.MATCH, matchId.ToString())) {
                return null;
            }
            var matchTimelineObj = matchTimelineTask.Result;

            return new Tuple<Match, MatchTimeline>(matchInfoObj, matchTimelineObj);
        }

        // DB Table: PlayerStats
        private static Dictionary<Role, Tuple<string, int>> GetTeamDictFromDB(long matchId, int teamId) {
            var teamDict = new Dictionary<Role, Tuple<string, int>>();

            var columns = new Dictionary<string, Tuple<DB, string>>();
            columns.Add(C_MATCHID, new Tuple<DB, string>(DB.WHERE, matchId.ToString()));
            columns.Add(C_TEAMID, new Tuple<DB, string>(DB.WHERE, teamId.ToString()));

            var playerDBList = DBWrapper.DBReadFromTable(T_PLAYERSTATS, columns);
            foreach (var playerObj in playerDBList) {
                string roleStr = playerObj[C_ROLE].ToString();
                Role role = (roleStr == Role.TOP.ToString()) ? Role.TOP :
                    (roleStr == Role.JUNGLE.ToString()) ? Role.JUNGLE :
                    (roleStr == Role.MIDDLE.ToString()) ? Role.MIDDLE :
                    (roleStr == Role.BOTTOM.ToString()) ? Role.BOTTOM :
                    (roleStr == Role.SUPPORT.ToString()) ? Role.SUPPORT : Role.NONE;
                string summId = playerObj[C_SUMMID].ToString();
                int champId = Convert.ToInt32(playerObj[C_CHAMPID]);
                teamDict.Add(role, new Tuple<string, int>(summId, champId));
            }

            return teamDict;
        }

        #endregion

        #endregion

        #region DB Calls

        // DB Table: Champions
        public static void LoadChampionJSON(string fileName) {
            JObject json = JObject.Parse(File.ReadAllText(fileName));
            JObject champData = (JObject)json["data"];

            foreach (var champObj in champData) {
                string jsonName = champObj.Value["name"].ToString();
                string jsonId = champObj.Value["key"].ToString();

                if (!cacheChamp.Reverse.ContainsKey(int.Parse(jsonId))) {
                    var insert = new Dictionary<string, Tuple<DB, string>>();
                    insert.Add(C_NAME, new Tuple<DB, string>(DB.INSERT, jsonName));
                    insert.Add(C_ID, new Tuple<DB, string>(DB.INSERT, jsonId));
                    DBWrapper.DBInsertIntoTable(T_CHAMPIONS, insert);
                }
                else {
                    string dbName = cacheChamp.Reverse[int.Parse(jsonId)];
                    if (jsonName != dbName) {
                        var param = new Dictionary<string, Tuple<DB, string>>();
                        param.Add(C_NAME, new Tuple<DB, string>(DB.WHERE, jsonName));
                        param.Add(C_ID, new Tuple<DB, string>(DB.SET, jsonId));
                        DBWrapper.DBUpdateTable(T_CHAMPIONS, param);
                    }
                }
                // Yikes this is ugly
            }
        }

        // DB Table: Champions
        // cacheComp Initialization upon startup
        public static void InitializeChampCache() {
            var objList = DBWrapper.DBReadFromTable(T_CHAMPIONS);

            foreach (var objDict in objList) {
                string champName = objDict[C_NAME].ToString();
                int champID = (int)objDict[C_ID];
                cacheChamp.Add(champName, champID);
            }
        }

        // DB Table: Competitions
        // cacheComp Initialization upon startup
        public static List<string> InitializeCompetitionCache() {
            var list = new List<string>() { "" };   // For a blank option in Combobox
            var objList = DBWrapper.DBReadFromTable(T_COMPETITIONS);

            foreach (var objDict in objList) {
                string compName = objDict[C_NAME].ToString();
                compName += " [" + objDict[C_TYPE].ToString() + "]";
                int compID = (int)objDict[C_ID];
                list.Add(compName);
                cacheComp.Add(compName, compID);
            }
            return list;
        }

        // DB Table: Teams
        // cacheTeam Initialization upon startup
        public static void InitializeTeamCache() {
            var objList = DBWrapper.DBReadFromTable(T_TEAMS);

            foreach (var objDict in objList) {
                string teamName = objDict[C_NAME].ToString();
                int teamID = (int)objDict[C_ID];
                cacheTeam.Add(teamName, teamID);
            }
        }

        // DB Table: Summoners
        // cacheSummoner Initialization upon startup
        public static void InitializeSummonerCache() {
            var objList = DBWrapper.DBReadFromTable(T_SUMMONERS);

            foreach (var objDict in objList) {
                string summName = objDict[C_NAME].ToString();
                string summID = objDict[C_SUMMID].ToString();
                cacheSummoner.Add(summName, summID);
            }
        }

        // DB Table: Match IDs, based on Competition Name
        public static List<string> GetMatchIdList(string compName) {
            string compID = cacheComp.Forward[compName].ToString();
            var conds = new Dictionary<string, Tuple<DB, string>>();
            conds.Add(C_COMPID, new Tuple<DB, string>(DB.WHERE, compID));

            cacheMatch.Clear();
            var list = new List<string>() { "" };   // This is for Combobox
            var objList = DBWrapper.DBReadFromTable(T_MATCHES, conds);
            foreach (var objDict in objList) {
                string id = objDict[C_ID].ToString();
                list.Add(id);
                cacheMatch.Add(id);
            }

            return list;
        }

        // DB Table: Teams, based on Competition Name+
        public static Dictionary<string, List<string>> GetTeamNames(string compName) {
            var output = new Dictionary<string, List<string>>(); // Key: Team names, Value: List of Summoners in Team
            string compID = cacheComp.Forward[compName].ToString();

            var conds = new Dictionary<string, Tuple<DB, string>>();
            conds.Add(C_COMPID, new Tuple<DB, string>(DB.WHERE, compID));
            var objList = DBWrapper.DBReadFromTable(T_REGISTEREDPLAYERS, conds);
            foreach (var objDict in objList) {
                int teamID = Convert.ToInt32(objDict[C_TEAMID]);
                string summID = objDict[C_SUMMID].ToString();
                string teamName = cacheTeam.Reverse[teamID];
                string summName = cacheSummoner.Reverse[summID];
                if (output.ContainsKey(teamName)) {
                    output[teamName].Add(summName);
                }
                else {
                    output[teamName] = new List<string>() { summName };
                }
            }

            return output;
        }

        // DB Table: Team Names, based on Match
        public static bool IsTeamInMatchesTable(string teamName) {
            int teamID = cacheTeam.Forward[teamName];

            var conds = new Dictionary<string, Tuple<DB, string>>();
            conds.Add(C_REDTEAMID, new Tuple<DB, string>(DB.WHERE, teamID.ToString()));
            conds.Add(C_BLUETEAMID, new Tuple<DB, string>(DB.WHERE, teamID.ToString()));

            return DBWrapper.DBTableHasEntry(T_MATCHES, conds, Operator.OR);
        }

        public static bool IsSummonerInPlayerStats(string summName, string teamName) {
            string summID = cacheSummoner.Forward[summName];
            int teamID = cacheTeam.Forward[teamName];

            var conds = new Dictionary<string, Tuple<DB, string>>();
            conds.Add(C_SUMMID, new Tuple<DB, string>(DB.WHERE, summID));
            conds.Add(C_TEAMID, new Tuple<DB, string>(DB.WHERE, teamID.ToString()));

            return DBWrapper.DBTableHasEntry(T_PLAYERSTATS, conds);
        }

        // DB Table: Teams. Add to "Teams" if team name does not exist
        public static void AddTeamInDBAndCache(string teamName) {
            if (!cacheTeam.Forward.ContainsKey(teamName)) {
                var insertTeams = new Dictionary<string, Tuple<DB, string>>();
                insertTeams.Add(C_NAME, new Tuple<DB, string>(DB.INSERT, teamName));
                DBWrapper.DBInsertIntoTable(T_TEAMS, insertTeams);

                insertTeams[C_NAME] = new Tuple<DB, string>(DB.WHERE, teamName);
                var teamObjs = DBWrapper.DBReadFromTable(T_TEAMS, insertTeams);
                cacheTeam.Add(teamName, int.Parse(teamObjs[0][C_ID].ToString()));
            }
        }

        // DB Table: Update Team Name
        public static void UpdateTeamNameInDBAndCache(string oldName, string newName) {
            int teamID = cacheTeam.Forward[oldName];

            var conds = new Dictionary<string, Tuple<DB, string>>();
            conds.Add(C_ID, new Tuple<DB, string>(DB.WHERE, teamID.ToString()));
            conds.Add(C_NAME, new Tuple<DB, string>(DB.SET, newName));
            DBWrapper.DBUpdateTable(T_TEAMS, conds);

            cacheTeam.RemoveBackward(teamID);
            cacheTeam.Add(newName, teamID);
        }

        public static void RemoveSummonerFromCompetition(string summName, string compName) {
            var delete = new Dictionary<string, Tuple<DB, string>>();
            string summId = cacheSummoner.Forward[summName];
            int compId = cacheComp.Forward[compName];
            delete.Add(C_SUMMID, new Tuple<DB, string>(DB.WHERE, summId));
            delete.Add(C_COMPID, new Tuple<DB, string>(DB.WHERE, compId.ToString()));
            DBWrapper.DBDeleteFromTable(T_REGISTEREDPLAYERS, delete);
        }

        #endregion

        #endregion
    }
}
