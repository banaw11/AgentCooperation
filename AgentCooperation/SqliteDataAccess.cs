using Dapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
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
            try
            {
                using (DbConnection())
                {
                    agent.Agent_Code = NewAgentCode();
                    DbConnection();
                    string query = "INSERT OR IGNORE INTO AGENTS (AGENT_CODE, AGENT_NAME, WORKING_AREA, COMMISSION, PHONE_NO, COUNTRY)" +
                            "SELECT '"+agent.Agent_Code+"', '"+agent.Agent_Name+"', '"+agent.Working_Area+"', "+agent.Commission.ToString().Replace(',','.')+", '"+agent.Phone_No+"', '"+agent.Country+"'";
                    dbConnection.Query(query, new DynamicParameters());
                }
            }
            catch(SQLiteException e)
            {
                    MessageBox.Show(("Something went wrong. Record not added \n " + e.Message),"Error",MessageBoxButton.OK,MessageBoxImage.Error);
            }
            finally
            {
                Agents.AgentsList = LoadAgents();
            }
        }

        public static void RemoveAgent(Agent agent)
        {
            try
            {
                using (DbConnection())
                {
                    DbConnection();
                    string query = "DELETE FROM AGENTS WHERE AGENT_CODE = @Agent_Code";
                    dbConnection.Query(query, new { Agent_Code = agent.Agent_Code });
                }
            }
            catch(SQLiteException e)
            {
                MessageBox.Show(("Something went wrong. Record not deleted \n " + e.Message), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Agents.AgentsList = LoadAgents();
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

        /* Acces to the Users Table in database */
        public static bool CheckId(string id)
        {
            bool check = false;
            using (DbConnection())
            {
                string query = "SELECT EXISTS (SELECT NULL FROM USERS WHERE AGENT_CODE = @Id)";
                var output = dbConnection.Query(query, new { Id = id });
                if (output.ToList()[0] == 1)
                    check = true;
            }
            return check;
        }

        public static bool CheckPassword(string id, string pswd)
        {
            bool check = false;
            using (DbConnection())
            {
                string query = "SELECT EXISTS (SELECT NULL FROM USERS WHERE AGENT_CODE = @Id AND PASSWORD = @Pswd)";
                var output = dbConnection.Query(query, new { Id = id, Pswd = pswd });
                if (output.ToList()[0] == 1)
                    check = true;
            }
            return check;
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

        private static string NewAgentCode()
        {
            string code;
            using (DbConnection())
            {
                var lastID = dbConnection.Query<string>("SELECT MAX(AGENT_CODE) FROM AGENTS");
                int id = int.Parse(lastID.ToList()[0].Trim('A'));
                id++;
                code = Regex.Replace(lastID.ToList()[0], @"[\d-]", string.Empty);
                string idStr = id.ToString();
                idStr = idStr.PadLeft(3, '0');
                MessageBox.Show(idStr);
                code += idStr;
            }
            return code;
        }
    }
}
