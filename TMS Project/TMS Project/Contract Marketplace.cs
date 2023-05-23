using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace TMS_Project
{
    internal class Contract_Marketplace
    {
        MySqlConnection conn = null;

        public string[] ConnectDB()
        {
            string connectString = "server=159.89.117.198;port=3306;database=cmp;uid=DevOSHT;pwd=Snodgr4ss!;";

            conn = new MySqlConnection(connectString);

            const int MAX_STRINGS = 10; //Probably won't be more than 10 different contracts.

            String[] contractStrings = new string[MAX_STRINGS];

            try
            {
                conn.Open();
                contractStrings = ContractInfo();
                return contractStrings;
            }
            catch (Exception) 
            {
                MessageBox.Show("Unable to connect to this database.");
            }

            return null;
        }

        private string[] ContractInfo() 
        {
            const int MAX_STRINGS = 10; //Probably won't be more than 10 different contracts.

            String[] contractStrings = new string[MAX_STRINGS];
            string query;

            query = "select * from Contract";

            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataReader reader = cmd.ExecuteReader();

            int i = 0;


            //Gets all the information from each client.
            while(reader.Read())
            {
                contractStrings[i] += reader.GetString("Client_Name");
                contractStrings[i] += ";";
                contractStrings[i] += reader.GetString("Job_Type");
                contractStrings[i] += ";";
                contractStrings[i] += reader.GetString("Quantity");
                contractStrings[i] += ";";
                contractStrings[i] += reader.GetString("Origin");
                contractStrings[i] += ";";
                contractStrings[i] += reader.GetString("Destination");
                contractStrings[i] += ";";
                contractStrings[i] += reader.GetString("Van_Type");
                i++;
            }

            return contractStrings;
        }

    }
}
