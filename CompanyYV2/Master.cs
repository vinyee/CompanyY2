using System;
using System.Data;
using CompanyYV2.Classes.Jobs;
using CompanyYV2.Classes.Users;
using CompanyYV2.Database;
using CompanyYV2.Floors;
using CompanyYV2.Tools;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace CompanyYV2
{
    public static class Master
    {
        //Upp till var och en att bestämma :)
        public static readonly int NameMin = 3;
        public static readonly int NameMax = 15;
        public static readonly int AgeMax = 80;
        public static readonly int AgeMin = 16;

        public static UserManager UserManager = new UserManager();
        public static JobsManager JobsManager = new JobsManager();
        public static Texts Text = new Texts();

        public static Floor LoggedOut = new Floor();
        public static Floor1 Login = new Floor1();
        public static Floor2 LoggedIn = new Floor2();
        public static Floor3 Interview = new Floor3();

        public static DBConnector DB = new DBConnector();
        public static MySqlConnection Con = DB.GetDBConnection();

		public static void Run()
		{
            //Testar anslutning till databasen
            DB.Start(Con, true);
            DB.End(Con);

            //Läser jobb från databasen och sparar
            JobsManager.LoadStaticDB();

            //Välkomnar användaren
            LoggedOut.WelcomeMessage();
            LoggedOut.Main();
        }
    }
}
