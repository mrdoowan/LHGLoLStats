using System;
using System.Diagnostics;
using RiotSharp;
using RiotSharp.Misc;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using Microsoft.VisualBasic;

namespace LoLStatsAPIv4 {
    public class Program {

        private const string API_KEY = "RGAPI-0b4bf7f4-a3db-40c7-ab2b-35cb240eb646";
        private readonly static string connectionString =
            ConfigurationManager.ConnectionStrings["LoLStatsAPIv4.Properties.Settings.LHGDatabaseConnectionString"].ConnectionString;
        private const string SOLO_QUEUE_STRING = "RANKED_SOLO_5x5";
        private const string FLEX_QUEUE_STRING = "RANKED_FLEX_SR";
        #region Database Tables, Columns, Params Consts
        // T = Table Name
        // C = Column Name
        // P = Param Name for C# SQL Param library
        private const string T_SUMM = "Summoners";
        private const string C_ACCID = "accountID";
        private const string P_ACCID = "@accID";
        private const string C_SUMID = "summonerID";
        private const string P_SUMID = "@sumID";
        private const string C_NAME = "name";
        private const string P_NAME = "@name";
        private const string C_STIER = "soloTier";
        private const string P_STIER = "@sTier";
        private const string C_SDIV = "soloDiv";
        private const string P_SDIV = "@sDiv";
        private const string C_FTIER = "flexTier";
        private const string P_FTIER = "@fTier";
        private const string C_FDIV = "flexDiv";
        private const string P_FDIV = "@fDiv";

        private const string T_COMP = "Competitions";
        private const string C_ID = "ID";
        private const string P_ID = "@id";
        private const string C_TYPE = "type";
        private const string P_TYPE = "@type";

        private const string T_REGP = "RegisteredPlayers";
        private const string C_COMPNAME = "competitionName";
        private const string P_COMPNAME = "@compName";
        private const string C_COMPID = "competitionID";
        private const string P_COMPID = "@compID";

        private const string T_TEAM = "Teams";


        #endregion



        [STAThread]
        public static void Main() {
            RiotApi apiDevelop = RiotApi.GetDevelopmentInstance(API_KEY);

            // No two functions should be running at the same time lol
            // Should hopefully have all my .txt files to do this
            // 1) Need to build your tables exactly as how it is in the dbdiagram. 
            //      If that doesn't work, feel free to just grab the .mdf
            // 2) Execute below function first to load all the Summoner names from a .txt file
            //LoadSummonerNamesUpdateSummonerIds(apiDevelop);
            // 3) Manually SQL Query: Update the Competitions table row to add Competition Type. 
            //      On Actual website, we'll have to configure
            // 4) Manually SQL Query: Insert into Teams table. I've named them LHG5 Team #
            // 5) Update each Summoner's rank (Solo & Flex) based on CompetitionName in RegisteredPlayers
            //      (Execution of this function is more or less optional...)
            string competitionName = "LHG Tournament 5";
            //CompetitionUpdateSummonerRanks(apiDevelop, competitionName);
            // 6) Now load Matches from a .txt file. Follow the template in it. 
            //      This is essentially where everything gets populated.
            //      Riot API's roles may be incorrect, so that needs to be double checked somehow.
            //      If that is the case, make the conflicting roles NULL
            string competitionType = "Tournament";
            // Fxn6()
            // 6.1) Fix any NULL roles
            // 7) Since summonerIds are not identified in Custom Games, we have to load a .txt file
            // Fxn7()
            // 8) Output it all into an Excel sheet. This will be a bitch.
            // Fxn8()
        }

        // Input: Summoner Name Rosters based on Competition
        // API Output: Summoner IDs
        // Database: Summoner
        // APIKey Usage: 1 per new Name not in Summoners Table loaded from .txt
        private static void LoadSummonerNamesUpdateSummonerIds(RiotApi apiDev) {
            var ofd_Txt = OFD("Open Summoner Names");
            if (ofd_Txt == null) { return; }
            var summonersList = new List<string>(File.ReadLines(ofd_Txt.FileName));
            var summonerMap = new Dictionary<string, Tuple<string, string>>(); // Key: SummonerId -> Values: AccountId, Name
            string competitionName = summonersList[0];

            using (SqlConnection connection = new SqlConnection(connectionString)) {
                connection.Open();

                // If new, register the competition
                string queryComp = "SELECT COUNT(*) FROM " + T_COMP + " WHERE " + C_NAME + "=" + P_NAME;
                using (SqlCommand cmdCompetition = new SqlCommand(queryComp, connection)) {
                    cmdCompetition.Parameters.AddWithValue(P_NAME, competitionName);
                    if ((int)cmdCompetition.ExecuteScalar() == 0) {
                        string queryCompInsert = "INSERT INTO " + T_COMP + " VALUES (" + P_NAME + ")";
                        using (SqlCommand cmdInsert = new SqlCommand(queryCompInsert, connection)) {
                            cmdInsert.Parameters.AddWithValue(P_NAME, competitionName);
                            cmdInsert.ExecuteNonQuery();
                        }
                    }
                }

                // Before calling the API, check to see if summonerName is already in database
                // Add to a list of summoners that will update the database
                string queryName = "SELECT * FROM " + T_SUMM + " WHERE " + C_NAME + "=" + P_NAME;
                using (SqlCommand cmdNameCheck = new SqlCommand(queryName, connection)) {
                    cmdNameCheck.Parameters.Add(P_NAME, SqlDbType.NVarChar);
                    for (int i = 1; i < summonersList.Count; ++i) {
                        string summonerName = summonersList[i];
                        cmdNameCheck.Parameters[P_NAME].Value = summonerName;
                        using (SqlDataReader objRead = cmdNameCheck.ExecuteReader()) {
                            string summid = "", accid = "";
                            if (objRead.Read()) {
                                summid = objRead[C_SUMID].ToString();
                                accid = objRead[C_ACCID].ToString();
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
                    }
                }

                // Check if that accountID exists in the database: If so, they had a name change recently
                foreach (string summID in summonerMap.Keys) {
                    // SELECT COUNT(*) FROM [Table] WHERE [Col]=[Param]
                    string queryID = "SELECT COUNT(*) FROM " + T_SUMM + " WHERE " + C_SUMID + "=" + P_SUMID;
                    using (SqlCommand cmdIDCheck = new SqlCommand(queryID, connection)) {
                        cmdIDCheck.Parameters.AddWithValue(P_SUMID, summID);
                        string querySumm = "";
                        if ((int)cmdIDCheck.ExecuteScalar() == 0) {
                            // ID does not exist. INSERT new summoner
                            // INSERT INTO [Table] ([Col, ...]) VALUES ([Param, ...])
                            querySumm = "INSERT INTO " + T_SUMM + " (" + C_ACCID + ", " + C_SUMID + ", " + C_NAME + ") " +
                                 "VALUES (" + P_ACCID + ", " + P_SUMID + ", " + P_NAME + ")";
                        }
                        else {
                            // ID does exist. UPDATE summoner
                            // UPDATE [Table] SET [Col]=[Param] WHERE [Col]=[Param] AND [Col]=[PARAM]
                            querySumm = "UPDATE " + T_SUMM + " SET " + C_NAME + "=" + P_NAME +
                                " WHERE " + C_ACCID + "=" + P_ACCID + " AND " + C_SUMID + "=" + P_SUMID;
                        }
                        // Execute Query call
                        using (SqlCommand cmdSummoner = new SqlCommand(querySumm, connection)) {
                            var summTuple = summonerMap[summID];
                            string accID = summTuple.Item1;
                            string name = summTuple.Item2;
                            cmdSummoner.Parameters.AddWithValue(P_ACCID, accID);
                            cmdSummoner.Parameters.AddWithValue(P_SUMID, summID);
                            cmdSummoner.Parameters.AddWithValue(P_NAME, name);
                            cmdSummoner.ExecuteNonQuery();
                        }
                    }

                }

                foreach (string summID in summonerMap.Keys) {
                    // Now update RegisteredPlayers based on the Competition names
                    int compID = 0;
                    string queryID = "SELECT * FROM " + T_COMP + " WHERE " + C_NAME + "=" + P_NAME;
                    using (SqlCommand cmdID = new SqlCommand(queryID, connection)) {
                        cmdID.Parameters.AddWithValue(P_NAME, competitionName);
                        using (SqlDataReader objRead = cmdID.ExecuteReader()) {
                            if (objRead.Read()) {
                                compID = (int)objRead[C_ID];
                            }
                        }
                    }
                    queryID = "SELECT COUNT(*) FROM " + T_REGP + " WHERE " +
                        C_COMPNAME + "=" + P_COMPNAME + " AND " + C_SUMID + "=" + P_SUMID;
                    using (SqlCommand cmdRegister = new SqlCommand(queryID, connection)) {
                        cmdRegister.Parameters.AddWithValue(P_COMPNAME, competitionName);
                        cmdRegister.Parameters.AddWithValue(P_SUMID, summID);
                        if ((int)cmdRegister.ExecuteScalar() == 0) {
                            string queryInsert = "INSERT INTO " + T_REGP + " VALUES (" + P_COMPNAME + ", " + P_SUMID + ")";
                            using (SqlCommand cmdInsert = new SqlCommand(queryInsert, connection)) {
                                cmdInsert.Parameters.AddWithValue(P_COMPNAME, competitionName);
                                cmdInsert.Parameters.AddWithValue(P_SUMID, summID);
                                cmdInsert.ExecuteNonQuery();
                            }
                        }
                    }
                }

                connection.Close();
            }
        }

        // Input: Summoner IDs (from Table RegisteredPlayers)
        // API Output: Summoner ranks
        // Database: Summoner
        // APIKey Usage: 1 per summoner ID in the selected Competition
        private static void CompetitionUpdateSummonerRanks(RiotApi apiDev, string competitionName) {

            using (var connection = new SqlConnection(connectionString)) {
                connection.Open();
                var summIdList = new List<string>();

                string queryCompId = "SELECT * FROM " + T_REGP + " WHERE " + C_COMPNAME + "=" + P_COMPNAME;
                using (var cmdCompId = new SqlCommand(queryCompId, connection)) {
                    cmdCompId.Parameters.AddWithValue(P_COMPNAME, competitionName);
                    using (var obj = cmdCompId.ExecuteReader()) {
                        while (obj.Read()) {
                            summIdList.Add(obj[C_SUMID].ToString());
                        }
                    }
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
                            string paramTier = (queueType == SOLO_QUEUE_STRING) ? P_STIER : P_FTIER;
                            string colDiv = (queueType == SOLO_QUEUE_STRING) ? C_SDIV : C_FDIV;
                            string paramDiv = (queueType == SOLO_QUEUE_STRING) ? P_SDIV : P_FDIV;
                            string queryUpdate = "UPDATE " + T_SUMM + 
                                " SET " + colTier + "=" + paramTier + ", " + colDiv + "=" + paramDiv + 
                                " WHERE " + C_SUMID + "=" + P_SUMID;
                            using (var cmdUpate = new SqlCommand(queryUpdate, connection)) {
                                cmdUpate.Parameters.AddWithValue(paramTier, tier);
                                cmdUpate.Parameters.AddWithValue(paramDiv, div);
                                cmdUpate.Parameters.AddWithValue(P_SUMID, summId);
                                cmdUpate.ExecuteNonQuery();
                            }
                        }
                    }
                }

                connection.Close();
            }

            /*
            foreach (string Id in IDList) {
                var LeagueTask = apiDev.League.GetLeagueEntriesBySummonerAsync(Region.Na, Id);
                WaitTaskPassException(LeagueTask, Endpoint.LEAGUES, Id);
                var LeagueList = LeagueTask.Result;

                for (int i = 0; i < LeagueList.Count; ++i) {
                    var league = LeagueList[i];
                    if (league.QueueType == RANKED_SOLO_STRING) {
                        sbTier.AppendLine(league.Tier);
                        sbDiv.AppendLine(league.Rank);
                    }
                }
            }
            */
        }

        // Input: Match IDs, and Teams
        // API Output: Every match stat
        // Database: Matches
        // APIKey Usage: 2 per matchID
        private static void LoadMatchIDsUpdateMatchStats(RiotApi apiDev, string competitionName) {
            var ofd_Txt = OFD("Load Match IDs");
            if (ofd_Txt == null) { return; }
            List<string> strLines = new List<string>(File.ReadLines(ofd_Txt.FileName));

            using (var connection = new SqlConnection(connectionString)) {
                int goodLine = 0, blueTeamID = 0, redTeamID = 0;
                foreach (string line in strLines) {
                    if (!line.StartsWith("//")) {
                        int option = goodLine % 3;
                        if (option == 0) {
                            // Blue Team (100)
                            blueTeamID = GetTeamID(line, connection);
                        }
                        else if (option == 1) {
                            // Red Team (200)
                            redTeamID = GetTeamID(line, connection);
                        }
                        else {
                            // MatchID
                            string Id = line;
                            var matchTimelineTask = apiDev.Match.GetMatchTimelineAsync(Region.Na, long.Parse(Id));
                            var matchInfoTask = apiDev.Match.GetMatchAsync(Region.Na, long.Parse(Id));

                            WaitTaskPassException(matchInfoTask, APIParam.MATCH, Id);
                            var matchInfoObj = matchInfoTask.Result;
                            WaitTaskPassException(matchTimelineTask, APIParam.MATCH, Id);
                            var matchTimelineObj = matchTimelineTask.Result;

                            // CODE SOME SHIT HERE

                            redTeamID = 0; blueTeamID = 0;
                        }
                        option++;
                    }

                }
            }
        }

        #region General Helper Functions

        // In case Task.Wait() returns an exception, we don't want to Exit
        private static bool WaitTaskPassException(dynamic task, APIParam type, params string[] param) {
            bool taskCompleted = false;
            while (!taskCompleted) {
                try { 
                    task.Wait();
                    taskCompleted = true;
                }
                catch (Exception ex) {
                    if (ex.InnerException.Message.Contains("404")) {
                        var sb = new StringBuilder();
                        switch (type) {
                            case APIParam.SUMMONER_NAME:
                                sb.Append("Summoner name(s)"); break;
                            case APIParam.LEAGUES:
                                sb.Append("Summoner ID"); break;
                            case APIParam.MATCH:
                                sb.Append("Match ID"); break;
                            default:    break;
                        }
                        sb.Append(" \"");
                        foreach (string str in param) {
                            sb.Append(str + " ");
                        }
                        sb.Remove(sb.Length - 1, 1);
                        sb.Append("\" does not exist.");
                        Debug.WriteLine(sb.ToString());
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

        // Helper function for OpenFileDialog
        private static OpenFileDialog OFD(string title) {
            using (OpenFileDialog ofd = new OpenFileDialog()) {
                ofd.Filter = "Text File (*.txt)|*.txt";
                ofd.Title = title;
                ofd.RestoreDirectory = true;

                if (ofd.ShowDialog() == DialogResult.OK) {
                    return ofd;
                }
                else {
                    return null;
                }
            }
        }

        // Helper function for SaveFileDialog
        private static SaveFileDialog SFD(string title) {
            using (SaveFileDialog sfd = new SaveFileDialog()) {
                sfd.Filter = "Text File (*.txt)|*.txt";
                sfd.Title = title;
                sfd.OverwritePrompt = true;
                if (sfd.ShowDialog() == DialogResult.OK) {
                    return sfd;
                }
                else {
                    return null;
                }
            }
        }

        // Return team ID
        private static int GetTeamID(string teamName, SqlConnection connection) {
            string queryTeamId = "SELECT * FROM " + T_TEAM + " WHERE " + C_NAME + "=" + P_NAME;
            using (var cmdId = new SqlCommand(queryTeamId, connection)) {
                cmdId.Parameters.AddWithValue(P_NAME, teamName);
                using (var obj = cmdId.ExecuteReader()) {
                    if (obj.Read()) { return (int)obj[C_NAME]; }
                }
            }
            return 0;
        } 

        #endregion

    }
}
