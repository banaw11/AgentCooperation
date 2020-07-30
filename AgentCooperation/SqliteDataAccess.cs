using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentCooperation
{
    public class SqliteDataAccess
    {
        private static string Id = "Default";
        private static IDbConnection dbConnection = new SQLiteConnection(LoadConnectionString());
        public static List<Agent> LoadAgents()
        {
            using (dbConnection) 
            {
                var output = dbConnection.Query<Agent>("SELECT * From AGENTS", new DynamicParameters());
                return output.ToList();
            }
        }

        public static void AddAgent(Agent agent)
        {
            using (dbConnection)
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
            using (dbConnection)
            {
                dbConnection.Execute
                    ("IF EXISTS (SELECT * FROM AGENTS" +
                                "WHERE AGENT_CODE = @Agent_Code)" +
                     "BEGIN" +
                        "DELETE From AGENTS WHERE AGENT_CODE = @Agent_Code" +
                      "END", agent);
            }
        }

        public static List<Agent> SearchAgents(string condition, string value)
        {
            using (dbConnection)
            {
                var output = dbConnection.Query<Agent>(("SELECT * From AGENTS WHERE {0} = {1}", condition, value).ToString(), new DynamicParameters());
                return output.ToList();
            }
        }

        private static string LoadConnectionString()
        {
            return ConfigurationManager.ConnectionStrings[Id].ConnectionString;
        }
    }
}
