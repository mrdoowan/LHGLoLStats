using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoLStatsAPIv4_GUI {
    public class DBWrapper {

        public static string ConnectionString;

        // Does the Table have the queried entry?
        public static bool DBTableHasEntry(string tableName, Dictionary<string, string> condMap, Operator op = Operator.AND) {
            using (SqlConnection connection = new SqlConnection(ConnectionString)) {
                SqlCommand cmd = QueryBuilder(connection, QueryType.COUNT, tableName, condMap, op);
                connection.Open();
                LogClass.WriteSQLCmd(cmd.CommandText);
                return ((int)cmd.ExecuteScalar() > 0) ? true : false;
            }
        }

        public static void DBInsertIntoTable(string tableName, Dictionary<string, string> colMap) {
            using (SqlConnection connection = new SqlConnection(ConnectionString)) {
                SqlCommand cmd = QueryBuilder(connection, QueryType.INSERT, tableName, colMap);
                connection.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                LogClass.WriteSQLCmd(cmd.CommandText, rowsAffected);
            }
        }

        public static List<Dictionary<string, object>> DBReadFromTable(string tableName, Dictionary<string, string> condMap = null) {
            using (SqlConnection connection = new SqlConnection(ConnectionString)) {
                SqlCommand cmd = QueryBuilder(connection, QueryType.SELECT, tableName, condMap);
                var returnMap = new List<Dictionary<string, object>>();
                connection.Open();
                var reader = cmd.ExecuteReader();
                LogClass.WriteSQLCmd(cmd.CommandText);
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

        public static void DBUpdateTable(string tableName, Dictionary<string, string> condMap, Dictionary<string, string> setMap, Operator op = Operator.AND) {
            using (SqlConnection connection = new SqlConnection(ConnectionString)) {
                SqlCommand cmd = QueryBuilder(connection, QueryType.UPDATE, tableName, condMap, op, setMap);
                connection.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                LogClass.WriteSQLCmd(cmd.CommandText, rowsAffected);
            }
        }

        public static void DBDeleteFromTable(string tableName, Dictionary<string, string> condMap, Operator op = Operator.AND) {
            using (SqlConnection connection = new SqlConnection(ConnectionString)) {
                SqlCommand cmd = QueryBuilder(connection, QueryType.DELETE, tableName, condMap, op);
                connection.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                LogClass.WriteSQLCmd(cmd.CommandText, rowsAffected);
            }
        }

        #region Private Functions

        private static SqlCommand QueryBuilder(SqlConnection connection, QueryType type, string table, Dictionary<string, string> condMap, 
            Operator op = Operator.AND, Dictionary<string, string> setMap = null) {
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
                    query += WhereQuery(condMap, op);
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
                    query += WhereQuery(condMap, op);
                    break;
                case QueryType.DELETE:
                    // DELETE FROM table_name WHERE condition
                    query += "DELETE FROM " + table;
                    query += WhereQuery(condMap, op);
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
                    cmd.Parameters.AddWithValue("@" + colName + "1", setMap[colName]);
                }
                // Adding the "1" cuz it's a jank way to make it unique to condMap
            }
            return cmd;
        }

        // Default Where Query function
        private static string WhereQuery(Dictionary<string, string> condMap, Operator op) {
            string query = "";
            string andOrStr = (op == Operator.AND) ? " AND" : " OR";
            char[] andOrChars = (op == Operator.AND) ? new char[] { 'A', 'N', 'D' } : new char[] { 'O', 'R' };
            if (condMap != null) {
                query += " WHERE";
                foreach (string colName in condMap.Keys) {
                    query += " " + colName + "=@" + colName + andOrStr;
                }
                query = query.TrimEnd(andOrChars);
                query = query.TrimEnd(' ');
            }
            return query;
        }

        #endregion
    }

}
