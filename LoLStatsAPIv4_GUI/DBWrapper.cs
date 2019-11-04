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
        public static bool DBTableHasEntry(string tableName, Dictionary<string, Tuple<DB, string>> param, Operator op = Operator.AND) {
            using (SqlConnection connection = new SqlConnection(ConnectionString)) {
                SqlCommand cmd = QueryBuilder(connection, QueryType.COUNT, tableName, param, op);
                connection.Open();
                LogClass.WriteSQLCmd(cmd.CommandText);
                return ((int)cmd.ExecuteScalar() > 0) ? true : false;
            }
        }

        public static void DBInsertIntoTable(string tableName, Dictionary<string, Tuple<DB, string>> param) {
            using (SqlConnection connection = new SqlConnection(ConnectionString)) {
                SqlCommand cmd = QueryBuilder(connection, QueryType.INSERT, tableName, param);
                connection.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                LogClass.WriteSQLCmd(cmd.CommandText, rowsAffected);
            }
        }

        public static List<Dictionary<string, object>> DBReadFromTable(string tableName, Dictionary<string, Tuple<DB, string>> param = null, bool unique = false,
            Operator op = Operator.AND) {
            using (SqlConnection connection = new SqlConnection(ConnectionString)) {
                SqlCommand cmd = (unique) ? QueryBuilder(connection, QueryType.UNIQUE, tableName, param, op) : 
                    QueryBuilder(connection, QueryType.SELECT, tableName, param, op);
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

        public static void DBUpdateTable(string tableName, Dictionary<string, Tuple<DB, string>> param, Operator op = Operator.AND) {
            using (SqlConnection connection = new SqlConnection(ConnectionString)) {
                SqlCommand cmd = QueryBuilder(connection, QueryType.UPDATE, tableName, param, op);
                connection.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                LogClass.WriteSQLCmd(cmd.CommandText, rowsAffected);
            }
        }

        public static void DBDeleteFromTable(string tableName, Dictionary<string, Tuple<DB, string>> param, Operator op = Operator.AND) {
            using (SqlConnection connection = new SqlConnection(ConnectionString)) {
                SqlCommand cmd = QueryBuilder(connection, QueryType.DELETE, tableName, param, op);
                connection.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                LogClass.WriteSQLCmd(cmd.CommandText, rowsAffected);
            }
        }

        #region Private Functions

        private static SqlCommand QueryBuilder(SqlConnection connection, QueryType type, string table, Dictionary<string, Tuple<DB, string>> param, 
            Operator op = Operator.AND) {
            string query = "";
            switch (type) {
                case QueryType.SELECT:
                case QueryType.COUNT:
                    // SELECT column_name FROM table_name WHERE condition;
                    query += "SELECT ";
                    if (type == QueryType.COUNT) { query += "COUNT("; }
                    else if (type == QueryType.UNIQUE) { query += "DISTINCT"; }
                    string columnBuild = ColumnQuery(param, DB.COLUMN);
                    query += (columnBuild.Length == 0) ? "*" : columnBuild;
                    if (type == QueryType.COUNT) { query += ")"; }
                    query += " FROM " + table;
                    query += WhereQuery(param, op);
                    break;
                case QueryType.INSERT:
                    // INSERT INTO table_name (column1, column2, column3, ...)
                    // VALUES(value1, value2, value3, ...);
                    query += "INSERT INTO " + table;
                    string colQuery = "", valQuery = "";
                    foreach (string colName in param.Keys) {
                        if (param[colName].Item1 == DB.INSERT) {
                            colQuery += colName + ", ";
                            valQuery += "@" + colName + ", ";
                        }
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
                    query += ColumnQuery(param, DB.SET);
                    query += WhereQuery(param, op);
                    break;
                case QueryType.DELETE:
                    // DELETE FROM table_name WHERE condition
                    query += "DELETE FROM " + table;
                    query += WhereQuery(param, op);
                    break;
                default:
                    break;
            }
            SqlCommand cmd = new SqlCommand(query, connection);
            if (param != null) {
                foreach (string colName in param.Keys) {
                    cmd.Parameters.AddWithValue("@" + colName, param[colName].Item2);
                }
            }
            return cmd;
        }

        // Default Where Query function
        private static string WhereQuery(Dictionary<string, Tuple<DB, string>> param, Operator op) {
            string query = "";
            string andOrStr = (op == Operator.AND) ? " AND" : " OR";
            char[] andOrChars = (op == Operator.AND) ? new char[] { 'A', 'N', 'D' } : new char[] { 'O', 'R' };
            if (param != null) {
                query += " WHERE";
                foreach (string colName in param.Keys) {
                    if (param[colName].Item1 == DB.WHERE) {
                        query += " " + colName + "=@" + colName + andOrStr;
                    }
                }
                query = query.TrimEnd(andOrChars);
                query = query.TrimEnd(' ');
            }
            return query;
        }

        private static string ColumnQuery(Dictionary<string, Tuple<DB, string>> param, DB queryType) {
            var sb = new StringBuilder();
            if (param != null) {
                if (queryType == DB.COLUMN) {
                    foreach (string colName in param.Keys) {
                        if (param[colName].Item1 == DB.COLUMN) {
                            sb.Append(colName + ", ");
                        }
                    }
                }
                else if (queryType == DB.SET) {
                    foreach (string colName in param.Keys) {
                        if (param[colName].Item1 == DB.SET) {
                            sb.Append(colName + "=@" + colName + ", ");
                        }
                    }
                }
            }
            string query = sb.ToString();
            return query.TrimEnd(',', ' ');
        }

        #endregion


    }
}
