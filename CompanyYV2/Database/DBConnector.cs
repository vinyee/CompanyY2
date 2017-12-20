using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace CompanyYV2.Database
{
	/*
     * http://o7planning.org/en/10513/connecting-to-mysql-database-using-csharp#a1814968
     * 
     */

	public class DBConnector
    {
		public MySqlConnection
				 GetDBConnection(string host, int port, string database, string username, string password)
		{
			// Connection String.
			String connString = "Server=" + host + ";Database=" + database
				+ ";port=" + port + ";User Id=" + username + ";password=" + password;

			MySqlConnection conn = new MySqlConnection(connString);

			return conn;
		}

		public MySqlConnection GetDBConnection()
		{
			string host = "localhost";
			int port = 3306;
			string database = "company";
			string username = "root";
			string password = "";

            return GetDBConnection(host, port, database, username, password);
		}

        public void Start(MySqlConnection con, bool firsttime = false, bool showerror = false)
        {
            try
            {
                if (firsttime)
                    Console.WriteLine("Försöker ansluta till databas..");

                con.Open();

				if (firsttime)
                    if (con.State == ConnectionState.Open)
                        Console.WriteLine("Anslutningen lyckades!");

            }
            catch (Exception e)
            {
                if (showerror)
                    Console.WriteLine("Error: " + e.Message);
            }
        }

		public void End(MySqlConnection con, bool showerror = false)
		{
			try
			{
                con.Close();
                con.Dispose();
			}
			catch (Exception e)
			{
                if (showerror)
				    Console.WriteLine("Error: " + e.Message);
			}
		}
    }
}
