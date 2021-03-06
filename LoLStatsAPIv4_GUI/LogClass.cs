﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoLStatsAPIv4_GUI {
    public class LogClass {
        // External static class for Logging actions, cuz why not

        private static string DBLogString;
        private static string LogString;
        private static int APICalls;
        private static bool LogDBCalls;

        private static void WriteDBLogLine(string str) {
            DBLogString += str += '\n';
        }

        public static void WriteLogLine(string str) {
            LogString += str += '\n';
        }

        public static void ClearLog() {
            DBLogString = "";
            LogString = "";
            APICalls = 0;
        }

        public static void WriteSQLCmd(string cmdTxt, int rowsAffected = 0) {
            string str = "Query - \"" + cmdTxt + "\"";
            if (rowsAffected > 0) { str += ". " + rowsAffected + " rows affected."; }
            WriteDBLogLine(str);
        }

        public static void APICalled(APIParam type, string param, bool failed) {
            var sb = new StringBuilder();
            if (!failed) { sb.Append("API Request successful on "); }
            else { sb.Append("API Request FAILED on "); }
            switch (type) {
                case APIParam.SUMMONER_NAME: sb.Append("Summoner Endpoint. With name "); break;
                case APIParam.LEAGUES: sb.Append("League Endpoint. With Summoner ID "); break;
                case APIParam.MATCH: sb.Append("Match Endpoint. With Match ID "); break;
                case APIParam.TIMELINE: sb.Append("MatchTimeline Endpoint. With Match ID "); break;
                default: break;
            }
            sb.Append("\"" + param + "\"");
            WriteLogLine(sb.ToString());
            APICalls++;
        }

        public static void SetEnableDBLogs(bool check) {
            LogDBCalls = check;
        }

        public static string GetReport() {
            return ((LogDBCalls) ? DBLogString : "") + 
                LogString + APICalls.ToString() + " API GET Requests were called.";
        }
    }
}
