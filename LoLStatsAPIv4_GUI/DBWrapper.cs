using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoLStatsAPIv4_GUI {
    public class DBWrapper {

        private readonly static string connectionString =
            ConfigurationManager.ConnectionStrings["LoLStatsAPIv4_GUI.Properties.Settings.LHGDatabaseConnectionString"].ConnectionString;

        // Does the Table have the queried entry?
        public static bool DBTableHasEntry(string tableName, Dictionary<string, string> condMap) {
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                SqlCommand cmd = QueryBuilder(connection, QueryType.COUNT, tableName, condMap);
                connection.Open();
                LogClass.WriteLine("Query - \"" + cmd.CommandText + "\"");
                return ((int)cmd.ExecuteScalar() > 0) ? true : false;
            }
        }

        public static void DBInsertIntoTable(string tableName, Dictionary<string, string> condMap) {
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                SqlCommand cmd = QueryBuilder(connection, QueryType.INSERT, tableName, condMap);
                connection.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                LogClass.WriteLine("Query - \"" + cmd.CommandText + "\". " + rowsAffected + " rows affected.");
            }
        }

        public static List<Dictionary<string, object>> DBReadFromTable(string tableName, Dictionary<string, string> condMap = null) {
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                SqlCommand cmd = QueryBuilder(connection, QueryType.SELECT, tableName, condMap);
                var returnMap = new List<Dictionary<string, object>>();
                connection.Open();
                var reader = cmd.ExecuteReader();
                LogClass.WriteLine("Query - \"" + cmd.CommandText + "\"");
                while (reader.Read()) {
                    var element = new Dictionary<string, object>();
                    for (int i = 0; i < reader.FieldCount; ++i) {
                        string colName = reader.GetName(i);
                        var value = reader.GetValue(i);
                        try { element[colName] = value; }
                        catch { element.Add(colName, value); }
                    }
                    returnMap.Add(element);
                }
                return returnMap;
            }
        }

        public static void DBUpdateTable(string tableName, Dictionary<string, string> condMap, Dictionary<string, string> setMap) {
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                SqlCommand cmd = QueryBuilder(connection, QueryType.UPDATE, tableName, condMap, setMap);
                connection.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                LogClass.WriteLine("Query - \"" + cmd.CommandText + "\". " + rowsAffected + " rows affected.");
            }
        }

        #region Private Functions

        private static SqlCommand QueryBuilder(SqlConnection connection, QueryType type, string table, Dictionary<string, string> condMap,
            Dictionary<string, string> setMap = null) {
            string query = "";
            switch (type) {
                case QueryType.SELECT:
                case QueryType.COUNT:
                    // SELECT column_name FROM table_name WHERE condition;
                    query += "SELECT ";
                    if (type == QueryType.COUNT) { query += "COUNT("; }
                    query += "*";
                    if (type == QueryType.COUNT) { query += ")"; }
                    query += " FROM " + table;
                    if (condMap != null) { 
                        query += " WHERE";
                        foreach (string colName in condMap.Keys) {
                            query += " " + colName + "=@" + colName + " AND";
                        }
                        query = query.TrimEnd('A', 'N', 'D');
                        query = query.TrimEnd(' ');
                    }
                    break;
                case QueryType.INSERT:
                    // INSERT INTO table_name (column1, column2, column3, ...)
                    // VALUES(value1, value2, value3, ...);
                    query += "INSERT INTO " + table;
                    string colQuery = "", valQuery = "";
                    foreach (string colName in condMap.Keys) {
                        colQuery += colName + ", ";
                        valQuery += "@" + colName + ", ";
                    }
                    colQuery = colQuery.TrimEnd(',', ' ');
                    valQuery = valQuery.TrimEnd(',', ' ');
                    query += "(" + colQuery + ") VALUES (" + valQuery + ")";
                    break;
                case QueryType.UPDATE:
                    // UPDATE table_name
                    // SET column1 = value1, column2 = value2, ...
                    // WHERE condition;
                    query += "UPDATE " + table + " SET ";
                    foreach (string colName in setMap.Keys) {
                        query += colName + "=@" + colName + ", ";
                    }
                    query = query.TrimEnd(',', ' ');
                    query += " WHERE";
                    foreach (string colName in condMap.Keys) {
                        query += " " + colName + "=@" + colName + " AND";
                    }
                    query = query.TrimEnd('A', 'N', 'D');
                    query = query.TrimEnd(' ');
                    break;
                default:
                    break;
            }
            SqlCommand cmd = new SqlCommand(query, connection);
            if (condMap != null) {
                foreach (string colName in condMap.Keys) {
                    cmd.Parameters.AddWithValue("@" + colName, condMap[colName]);
                }
            }
            if (setMap != null) {
                foreach (string colName in setMap.Keys) {
                    cmd.Parameters.AddWithValue("@" + colName, setMap[colName]);
                }
            }
            return cmd;
        }

        #endregion
    }

}
