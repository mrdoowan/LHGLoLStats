using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RiotSharp;
using RiotSharp.Misc;

namespace LoLStatsAPIv4_GUI {
    public class MasterWrapper {

        private const string SOLO_QUEUE_STRING = "RANKED_SOLO_5x5";
        private const string FLEX_QUEUE_STRING = "RANKED_FLEX_SR";
        #region Database Tables, Columns, Consts
        // T = Table Name
        // C = Column Name
        private const string T_SUMMONERS = "Summoners";
        private const string T_COMPETITIONS = "Competitions";
        private const string T_REGISTEREDPLAYERS = "RegisteredPlayers";
        private const string T_TEAMS = "Teams";
        private const string T_MATCHES = "Matches";

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
        #endregion

        private readonly DBWrapper DBClass = new DBWrapper();
        private static RiotApi apiDev;
        private static Dictionary<string, int> compIDs = new Dictionary<string, int>();

        // Whenever textbox updates, dev instance
        public static void UpdateAPIDevInstance(string key) {
            apiDev = RiotApi.GetDevelopmentInstance(key);
        }

        // Input: None
        // API Output: None
        // Database: Competitions, getting list of names
        // APIKey Usage: None
        // Also updates a local dictionary to reduce calls to Database
        public static List<string> GetCompetitionNames() {
            var list = new List<string>() { "" };   // For a blank option in Combobox
            var objList = DBWrapper.DBReadFromTable(T_COMPETITIONS);

            foreach (var objDict in objList) {
                string compName = objDict[C_NAME].ToString();
                int compID = (int)objDict[C_ID];
                list.Add(compName);
                compIDs.Add(compName, compID);
            }
            return list;
        }

        // Input: None
        // API Output: None
        // Database: Match IDs, based on Competition Name
        // APIKey Usage: None
        public static List<string> GetMatchIdList(string compName) {
            string compID = compIDs[compName].ToString();

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

        // Input: None
        // API Output: None
        // Database: Team Names, based on Competition Name
        // APIKey Usage: None
        public static List<string> GetTeamNames(string compName) {
            string compID = compIDs[compName].ToString();

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

        // Input: Summoner Name Rosters based on Competition
        // API Output: Summoner IDs
        // Database: Summoner
        // APIKey Usage: 1 per new Name not in Summoners Table loaded from .txt
        public static bool LoadSummonerNamesIntoDB(string compName, List<string> compList) {
            var summonerMap = new Dictionary<string, Tuple<string, string>>(); // Key: SummonerId -> Values: AccountId, Name

            // If new, register the competition
            var conditions = new Dictionary<string, string>() { 
                { C_NAME, compName } 
            };
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
                    DBWrapper.DBUpdateTable(T_SUMMONERS, conditions, update);
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
                if (!DBWrapper.DBTableHasEntry(T_REGISTEREDPLAYERS, conditions)) {
                    DBWrapper.DBInsertIntoTable(T_REGISTEREDPLAYERS, conditions);
                }
            }

            return true;
        }

        // Input: Summoner IDs (from Table RegisteredPlayers)
        // API Output: Summoner ranks
        // Database: Summoner
        // APIKey Usage: 1 per summoner ID in the selected Competition
        public static bool CompetitionUpdateSummonerRanks(string compName) {
            if (compName.Length == 0) { return false; }

            var summIdList = new List<string>();
            var conds = new Dictionary<string, string>() {
                { C_COMPNAME, compName }
            };
            var objMap = DBWrapper.DBReadFromTable(T_REGISTEREDPLAYERS, conds);
            foreach (var row in objMap) {
                summIdList.Add(row[C_SUMID].ToString());
            }

            // Now update their Solo queue and Flex queue ranking
            foreach (string summId in summIdList) {
                var leagueTask = apiDev.League.GetLeagueEntriesBySummonerAsync(Region.Na, summId);
                WaitTaskPassException(leagueTask, APIParam.LEAGUES, summId);
                var leagueObj = leagueTask.Result;

                foreach (var leagueEntry in leagueObj) {
                    string tier = null, div = null;
                    string queueType = leagueEntry.QueueType;
                    if (queueType == SOLO_QUEUE_STRING || queueType == FLEX_QUEUE_STRING) {
                        tier = leagueEntry.Tier;
                        div = leagueEntry.Rank;
                    }

                    if (!string.IsNullOrWhiteSpace(tier) && !string.IsNullOrWhiteSpace(div)) {
                        string colTier = (queueType == SOLO_QUEUE_STRING) ? C_STIER : C_FTIER;
                        string colDiv = (queueType == SOLO_QUEUE_STRING) ? C_SDIV : C_FDIV;
                        conds = new Dictionary<string, string>() {
                            { C_SUMID, summId }
                        };
                        var set = new Dictionary<string, string>() {
                            { colTier, tier },
                            { colDiv, div }
                        };
                        DBWrapper.DBUpdateTable(T_SUMMONERS, conds, set);
                    }
                }
            }

            return true;
        }

        // Input: Match IDs, and Teams
        // API Output: Every match stat
        // Database: Matches
        // APIKey Usage: 2 per matchID
        public static bool LoadMatchStatsIntoDB(string compName, string blueTeamName, string redTeamName, long matchID) {
            int blueTeamID = GetTeamID(blueTeamName);
            int redTeamID = GetTeamID(redTeamName);
            var matchTimelineTask = apiDev.Match.GetMatchTimelineAsync(Region.Na, matchID);
            var matchInfoTask = apiDev.Match.GetMatchAsync(Region.Na, matchID);
            WaitTaskPassException(matchInfoTask, APIParam.MATCH, matchID.ToString());
            var matchInfoObj = matchInfoTask.Result;
            WaitTaskPassException(matchTimelineTask, APIParam.MATCH, matchID.ToString());
            var matchTimelineObj = matchTimelineTask.Result;

            return true;
        }

        #region Private Helper Functions

        // In case Task.Wait() returns an exception, we don't want to Exit
        private static bool WaitTaskPassException(dynamic task, APIParam type, string param) {
            bool taskCompleted = false;
            while (!taskCompleted) {
                try {
                    task.Wait();
                    taskCompleted = true;
                    LogClass.APICalled(type, param, false);
                }
                catch (Exception ex) {
                    if (ex.InnerException.Message.Contains("404")) {
                        LogClass.APICalled(type, param, true);
                        return false;
                    }
                    else {
                        // Dunno what happens when it's not 404??
                        Debug.WriteLine("Error with Task: " + ex.Message + "\nRate Limit reached?");
                    }
                }
            }
            return true;
        }

        // Get Team ID from name
        private static int GetTeamID(string teamName) {
            var cond = new Dictionary<string, string>() {
                { C_NAME, teamName }
            };
            var objMap = DBWrapper.DBReadFromTable(T_TEAMS, cond);
            if (objMap.Count > 0) {
                return (int)objMap[0][C_NAME];
            }
            return 0;
        }

        #endregion
    }
}
