using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AgentCooperation
{
    public class SqliteDataAccess
    {
        private static string Id = "Default";
        private static IDbConnection dbConnection;
        public static List<Agent> LoadAgents()
        {
            using (DbConnection()) 
            {
                var output = dbConnection.Query<Agent>("SELECT * From AGENTS", new DynamicParameters());
                return output.ToList();
            }
        }

        public static void AddAgent(Agent agent)
        {
            using (DbConnection())
            {
                dbConnection.Execute
                    ("If NOT EXIST ( SELECT * From AGENTS" +
                                    "WHERE AGENT_CODE = @Agent_Code )" +
                    "BEGIN" +
                        "INSERT INTO AGENTS (AGENT_CODE, AGENT_NAME, WORKING_AREA, COMMISSION, PHONE_NUMBER, COUNTRY)" +
                        "VALUES(@Agent_Code, @Agent_Name, @Working_Area, @Commision, @Phone_NO, @Country)"+
                    "END", agent);
            }
        }

        public static void RemoveAgent(Agent agent)
        {
            using (DbConnection())
            {
                dbConnection.Execute
                    ("IF EXISTS (SELECT * FROM AGENTS" +
                                "WHERE AGENT_CODE = @Agent_Code)" +
                     "BEGIN" +
                        "DELETE From AGENTS WHERE AGENT_CODE = @Agent_Code" +
                      "END", agent);
            }
        }

        public static List<Agent> SearchAgents(int indexOfCondition, string value)
        {
            string condition, query;
            using (DbConnection())
            {
                query = "SELECT name FROM pragma_table_info('AGENTS') WHERE cid = @Index";
                var info = dbConnection.Query<string>(query, new { Index = indexOfCondition});
                condition = info.ToList()[0];
                query = "SELECT * From AGENTS WHERE "+condition+" = @Value";
                var output = dbConnection.Query<Agent>(query, new {  Value = value });
                return output.ToList();
            }
        }

        private static string LoadConnectionString()
        {
            return ConfigurationManager.ConnectionStrings[Id].ConnectionString;
        }

        private static IDbConnection DbConnection()
        {
            dbConnection = new SQLiteConnection(LoadConnectionString());
            return dbConnection;
        }
    }
}
