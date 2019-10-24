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

namespace LoLStatsAPIv4_GUI {
    public class MasterWrapper {

        #region Private Functions & Consts

        private const string SOLO_QUEUE_STRING = "RANKED_SOLO_5x5";
        private const string FLEX_QUEUE_STRING = "RANKED_FLEX_SR";
        // T = Table Name
        // C = Column Name
        private const string T_SUMMONERS = "Summoners";
        private const string T_COMPETITIONS = "Competitions";
        private const string T_REGISTEREDPLAYERS = "RegisteredPlayers";
        private const string T_TEAMS = "Teams";
        private const string T_MATCHES = "Matches";
        private const string T_CHAMPIONS = "Champions";

        private const string C_ACCID = "accountID";
        private const string C_SUMID = "summonerID";
        private const string C_NAME = "name";
        private const string C_STIER = "soloTier";
        private const string C_SDIV = "soloDiv";
        private const string C_FTIER = "flexTier";
        private const string C_FDIV = "flexDiv";
        private const string C_ID = "ID";
        private const string C_TYPE = "type";
        private const string C_COMPNAME = "competitionName";
        private const string C_COMPID = "competitionID";
        private const string C_REDTEAMID = "redTeamID";
        private const string C_BLUETEAMID = "blueTeamID";
        private const string C_LASTUPDATE = "lastUpdated";

        // Cache Storage
        private static RiotApi apiDev;
        private static readonly Dictionary<string, int> compDict = new Dictionary<string, int>();     // Acting as a cache
        private static readonly Dictionary<int, string> champDict = new Dictionary<int, string>();    // Acting as a cache

        // In case Task.Wait() returns an exception, we don't want to Exit
        private static bool WaitTaskPassException(dynamic task, APIParam type, string param) {
            bool taskCompleted = false;
            while (!taskCompleted) {
                try {
                    task.Wait();
                    taskCompleted = true;
                    LogClass.APICalled(type, param, false);
                }
                catch (RiotSharpRateLimitException ex) {
                    LogClass.APICalled(type, param, true);
                    LogClass.WriteLogLine("Error " + ex.HttpStatusCode + ". Rate Limit Reached! Wait for " + ex.RetryAfter);
                    Cursor.Current = Cursors.WaitCursor;
                    Task.Delay(ex.RetryAfter);
                    Cursor.Current = Cursors.Default;
                }
                catch (Exception ex) {
                    LogClass.APICalled(type, param, true);
                    LogClass.WriteLogLine("Error: " + ex.Message);
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region Public Functions

        // Whenever textbox updates
        public static void UpdateAPIDevInstance(string key) {
            apiDev = RiotApi.GetDevelopmentInstance(key);
        }

        // Whenever connection string updates
        public static void UpdateConnectionString(string connection) {
            DBWrapper.ConnectionString = connection;
        }

        // DB Table: Teams
        // Should make it a cache lol
        public static int GetTeamID(string teamName) {
            var cond = new Dictionary<string, string>() {
                { C_NAME, teamName }
            };
            var objMap = DBWrapper.DBReadFromTable(T_TEAMS, cond);
            if (objMap.Count > 0) {
                return (int)objMap[0][C_ID];
            }
            return 0;
        }

        // Get Champ Name from id
        public static string GetChampName(int id) {
            return champDict[id];
        }

        #region API Calls

        // DB Table: Competitions, Summoners, Registered Players
        // API Input: Summoner Name Rosters based on Competition
        // API Output: Summoner IDs
        // APIKey Usage: 1 per new Name loaded from .txt
        public static bool LoadSummonerNamesIntoDB(string compName, string compType, List<string> compList) {
            var summonerMap = new Dictionary<string, Tuple<string, string>>(); // Key: SummonerId -> Values: AccountId, Name
            // If new, register the competition
            var conditions = new Dictionary<string, string>();
            conditions.Add(C_NAME, compName);
            conditions.Add(C_TYPE, compType);
            if (!DBWrapper.DBTableHasEntry(T_COMPETITIONS, conditions)) {
                if (MessageBox.Show("Do you want to add new competition \"" + compName + "\"?", "New Competition",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) {
                    return false;
                }
                DBWrapper.DBInsertIntoTable(T_COMPETITIONS, conditions);
            }

            // Before calling the API, check to see if summonerName is already in database
            // Add to a list of summoners that will update the database
            for (int i = 0; i < compList.Count; ++i) {
                string summonerName = compList[i];
                conditions = new Dictionary<string, string>() {
                    { C_NAME, summonerName }
                };
                var objMaps = DBWrapper.DBReadFromTable(T_SUMMONERS, conditions);
                string summid = "", accid = "";
                if (objMaps.Count > 0) {
                    summid = objMaps[0][C_SUMID].ToString();
                    accid = objMaps[0][C_ACCID].ToString();
                }
                else {
                    // New summoner name
                    var summonerTask = apiDev.Summoner.GetSummonerByNameAsync(Region.Na, summonerName);
                    if (WaitTaskPassException(summonerTask, APIParam.SUMMONER_NAME, summonerName)) {
                        var summonerObj = summonerTask.Result;
                        summid = summonerObj.Id;
                        accid = summonerObj.AccountId;
                    }
                }
                summonerMap.Add(summid, new Tuple<string, string>(accid, summonerName));
            }

            // Check if that summonerID exists in the database: If so, they had a name change recently
            foreach (string summID in summonerMap.Keys) {
                var summTuple = summonerMap[summID];
                string accID = summTuple.Item1;
                string name = summTuple.Item2;

                conditions = new Dictionary<string, string>() {
                    { C_SUMID, summID }
                };
                if (!DBWrapper.DBTableHasEntry(T_SUMMONERS, conditions)) {
                    // ID does not exist. INSERT new summoner
                    var insert = new Dictionary<string, string>() {
                        { C_ACCID, accID },
                        { C_SUMID, summID },
                        { C_NAME, name }
                    };
                    DBWrapper.DBInsertIntoTable(T_SUMMONERS, insert);
                }
                else {
                    // ID does exist. UPDATE summoner
                    conditions = new Dictionary<string, string>() {
                        { C_ACCID, accID },
                        { C_SUMID, summID }
                    };
                    var update = new Dictionary<string, string>() {
                        { C_NAME, name }
                    };
                    DBWrapper.DBUpdateTable(T_SUMMONERS, conditions, update, Operator.AND);
                }

            }

            foreach (string summID in summonerMap.Keys) {
                // Now update RegisteredPlayers based on the Competition names
                int compID = 0;
                conditions = new Dictionary<string, string>() {
                    { C_NAME, compName }
                };
                var objRead = DBWrapper.DBReadFromTable(T_COMPETITIONS, conditions);
                if (objRead.Count > 0) {
                    compID = (int)objRead[0][C_ID];
                }

                conditions = new Dictionary<string, string>() {
                    { C_COMPID, compID.ToString() },
                    { C_SUMID, summID }
                };
                if (!DBWrapper.DBTableHasEntry(T_REGISTEREDPLAYERS, conditions, Operator.AND)) {
                    DBWrapper.DBInsertIntoTable(T_REGISTEREDPLAYERS, conditions);
                }
            }

            return true;
        }

        // DB Table: Registered Players, Summoners, 
        // API Input: Summoner IDs (from Table RegisteredPlayers)
        // API Output: Summoner Ranks
        // APIKey Usage: 1 per Summoner in selected Competition
        public static bool CompetitionUpdateSummonerRanks(string compName) {
            if (compName.Length == 0) { return false; }

            var summIdList = new List<string>();
            var conds = new Dictionary<string, string>();
            conds.Add(C_COMPNAME, compName);
            var objMap = DBWrapper.DBReadFromTable(T_REGISTEREDPLAYERS, conds);
            foreach (var row in objMap) {
                summIdList.Add(row[C_SUMID].ToString());
            }

            // Now update their Solo queue and Flex queue ranking
            foreach (string summId in summIdList) {
                var leagueTask = apiDev.League.GetLeagueEntriesBySummonerAsync(Region.Na, summId);
                WaitTaskPassException(leagueTask, APIParam.LEAGUES, summId);
                var leagueObj = leagueTask.Result;
                var timeNow = DateTime.Now;

                conds = new Dictionary<string, string>();
                conds.Add(C_SUMID, summId);
                var set = new Dictionary<string, string>();
                set.Add(C_LASTUPDATE, timeNow.ToString());
                string sTier = null, sDiv = null, fTier = null, fDiv = null;
                foreach (var leagueEntry in leagueObj) {
                    string queueType = leagueEntry.QueueType;
                    if (queueType == SOLO_QUEUE_STRING) {
                        sTier = leagueEntry.Tier;
                        sDiv = leagueEntry.Rank;
                        set.Add(C_STIER, sTier);
                        set.Add(C_SDIV, sDiv);
                    }
                    else if (queueType == FLEX_QUEUE_STRING) {
                        fTier = leagueEntry.Tier;
                        fDiv = leagueEntry.Rank;
                        set.Add(C_FTIER, fTier);
                        set.Add(C_FDIV, fDiv);
                    }
                }
                DBWrapper.DBUpdateTable(T_SUMMONERS, conds, set);
            }

            return true;
        }

        // DB Table: Matches, PlayerStats, TeamStats, BannedChamps, Objectives
        // API Input: Match ID
        // API Output: Every applicable stat in that match
        // APIKey Usage: 2
        public static bool LoadMatchStatsIntoDB(string compName, string blueTeamName, string redTeamName, long matchID) {
            int blueTeamID = GetTeamID(blueTeamName);
            int redTeamID = GetTeamID(redTeamName);
            var matchTimelineTask = apiDev.Match.GetMatchTimelineAsync(Region.Na, matchID);
            var matchInfoTask = apiDev.Match.GetMatchAsync(Region.Na, matchID);
            WaitTaskPassException(matchInfoTask, APIParam.MATCH, matchID.ToString());
            var matchInfoObj = matchInfoTask.Result;
            WaitTaskPassException(matchTimelineTask, APIParam.MATCH, matchID.ToString());
            var matchTimelineObj = matchTimelineTask.Result;

            MatchStats match = new MatchStats(compDict[compName]);
            match.InitializeClass(matchInfoObj, matchTimelineObj);

            return true;
        }

        #endregion

        #region DB Calls

        // DB Table: Champions
        public static void LoadChampionJSON(string fileName) {
            JObject json = JObject.Parse(File.ReadAllText(fileName));

            JObject champData = (JObject)json["data"];
            foreach (var champObj in champData) {
                string jsonName = champObj.Value["name"].ToString();
                string jsonId = champObj.Value["key"].ToString();

                var conds = new Dictionary<string, string>() {
                    { C_NAME, jsonName }
                };
                if (!champDict.ContainsKey(int.Parse(jsonId))) {
                    var insert = new Dictionary<string, string>() {
                        { C_NAME, jsonName },
                        { C_ID, jsonId }
                    };
                    DBWrapper.DBInsertIntoTable(T_CHAMPIONS, insert);
                }
                else {
                    string dbName = champDict[int.Parse(jsonId)];
                    if (jsonName != dbName) {
                        var update = new Dictionary<string, string>() {
                            { C_ID, jsonId }
                        };
                        DBWrapper.DBUpdateTable(T_CHAMPIONS, conds, update);
                    }
                }
                // Yikes this is ugly
            }
        }

        // DB Table: Champions
        // Cache initializes upon startup
        public static void InitializeChampDict() {
            var objList = DBWrapper.DBReadFromTable(T_CHAMPIONS);

            foreach (var objDict in objList) {
                string champName = objDict[C_NAME].ToString();
                int champID = (int)objDict[C_ID];
                champDict.Add(champID, champName);
            }
        }

        // DB Table: Competitions
        // Also updates a local dictionary (or cache) to reduce calls to Database
        public static List<string> GetCompetitionNames() {
            var list = new List<string>() { "" };   // For a blank option in Combobox
            var objList = DBWrapper.DBReadFromTable(T_COMPETITIONS);

            foreach (var objDict in objList) {
                string compName = objDict[C_NAME].ToString();
                int compID = (int)objDict[C_ID];
                list.Add(compName);
                compDict.Add(compName, compID);
            }
            return list;
        }

        // DB Table: Match IDs, based on Competition Name
        public static List<string> GetMatchIdList(string compName) {
            string compID = compDict[compName].ToString();

            var list = new List<string>() { "" };   // This is for Combobox
            var conds = new Dictionary<string, string>() {
                { C_COMPID, compID }
            };
            var objList = DBWrapper.DBReadFromTable(T_MATCHES, conds);

            foreach (var objDict in objList) {
                list.Add(objDict[C_ID].ToString());
            }

            return list;
        }

        // DB Table: Teams, based on Competition Name
        public static List<string> GetTeamNames(string compName) {
            string compID = compDict[compName].ToString();

            var list = new List<string>();
            var conds = new Dictionary<string, string>() {
                { C_COMPID, compID }
            };
            var objList = DBWrapper.DBReadFromTable(T_TEAMS, conds);

            foreach (var objDict in objList) {
                list.Add(objDict[C_NAME].ToString());
            }

            return list;
        }

        // DB Table: Team Names, based on Competition Name
        public static bool IsTeamInMatchesTable(string teamName) {
            int teamID = GetTeamID(teamName);

            var conds = new Dictionary<string, string>() {
                { C_REDTEAMID, teamID.ToString() },
                { C_BLUETEAMID, teamID.ToString() }
            };
            return DBWrapper.DBTableHasEntry(T_MATCHES, conds, Operator.OR);
        }

        // DB Table: Teams, Modify based on Team
        public static void RemoveTeamInTable(string teamName) {
            var conds = new Dictionary<string, string>() {
                { C_NAME, teamName }
            };
            DBWrapper.DBDeleteFromTable(T_TEAMS, conds, Operator.AND);
        }

        // DB Table: Teams, Add new Team
        public static void AddTeamInTable(string teamName, string compName) {
            string compID = compDict[compName].ToString();

            var entries = new Dictionary<string, string>() {
                { C_NAME, teamName },
                { C_COMPID, compID }
            };

            DBWrapper.DBInsertIntoTable(T_TEAMS, entries);
        }

        // DB Table: Update Team Name
        public static void UpdateTeamNameInTable(string oldName, string newName) {
            var conds = new Dictionary<string, string>() {
                { C_NAME, oldName }
            };
            var update = new Dictionary<string, string>() {
                { C_NAME, newName }
            };

            DBWrapper.DBUpdateTable(T_TEAMS, conds, update);
        }

        #endregion

        #endregion
    }
}
