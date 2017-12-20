using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace CompanyYV2.Classes.Users
{
    public class UserManager
    {
        //Listan på medlemmar
        public Dictionary<int, UserData> Users;

        public UserManager()
        {
            Users = new Dictionary<int, UserData>();
        }

        //Kolla ifall usern finns med
        public bool Exist(UserData user)
        {
            return (Users.ContainsValue(user));
        }

        //Kolla ifall usern finns med via key/id
		public bool Exist(int id)
		{
            return (Users.ContainsKey(id));
		}

        //Lägg till user
        public void Add(UserData user)
        {
            if (user != null)
                if (!Exist(user))
                    Users.Add(user.Id, user);
        }

        //Ta bort user
		public void Remove(UserData user)
		{
            if (user != null)
			    if (!Exist(user))
                    Users.Remove(user.Id);
		}

        //Uppdatera gamla user med nya genom id
        public void Update(UserData user)
        {
            if (user != null)
                if (Exist(user.Id))
                    Users[user.Id] = user;
        }

        public void Show(UserData User)
		{
            Console.WriteLine("Namn: " + User.Name);
            Console.WriteLine("Efternamn: " + User.Lastname);
            Console.WriteLine("Ålder: " + Master.Text.GetAge(User.YearofBirth));

            Console.WriteLine("");
        }

        public void AddDB(UserData user)
        {
			Master.DB.Start(Master.Con);
			{
                MySqlCommand cmd = new MySqlCommand("Insert into users (name, lastname, yearofbirth) "
                                                    + " values (@name, @lastname, @yearofbirth) ", Master.Con);

				cmd.Parameters.Add(new MySqlParameter("@name", MySqlDbType.VarChar, 20));
                cmd.Parameters["@name"].Value = user.Name;

				cmd.Parameters.Add(new MySqlParameter("@lastname", MySqlDbType.VarChar, 20));
                cmd.Parameters["@lastname"].Value = user.Lastname;

                cmd.Parameters.Add(new MySqlParameter("@yearofbirth", MySqlDbType.Int16, 5));
                cmd.Parameters["@yearofbirth"].Value = user.YearofBirth;

                // Execute Command (for Delete, Insert or Update).
                cmd.ExecuteNonQuery();

				//stänger anslutningen såklart
				Master.DB.End(Master.Con);
			}
        }

		public void UpdateDB(UserData user)
		{
			Master.DB.Start(Master.Con);
			{
				MySqlCommand cmd = new MySqlCommand("UPDATE users set " +
                                                    "name = @name," +
                                                    "lastname = @lastname," +
                                                    "yearofbirth = @yearofbirth " +
                                                    "WHERE id = @id " 
                                                    , Master.Con);

				cmd.Parameters.Add(new MySqlParameter("@name", MySqlDbType.VarChar, 20));
				cmd.Parameters["@name"].Value = user.Name;

				cmd.Parameters.Add(new MySqlParameter("@lastname", MySqlDbType.VarChar, 20));
				cmd.Parameters["@lastname"].Value = user.Lastname;

				cmd.Parameters.Add(new MySqlParameter("@yearofbirth", MySqlDbType.Int16, 5));
				cmd.Parameters["@yearofbirth"].Value = user.YearofBirth;

                cmd.Parameters.Add(new MySqlParameter("@id", MySqlDbType.Int16, 11));
                cmd.Parameters["@id"].Value = user.Id;

				// Execute Command (for Delete, Insert or Update).
				cmd.ExecuteNonQuery();

				//stänger anslutningen såklart
				Master.DB.End(Master.Con);
			}
		}

		//Returnerar användare ifall den existerar med name och yob
		public UserData Load(int id)
		{
			/* Finns ingen cache kollar vi databasen
             * Öppnar ny anslutning mot databasen
             */
			Master.DB.Start(Master.Con);
			{
				MySqlCommand cmd = new MySqlCommand("SELECT * " +
													"FROM users " +
													"WHERE id = @id LIMIT 1"

													, Master.Con);
				cmd.CommandType = CommandType.Text;

				cmd.Parameters.AddWithValue("@id", id);

				using (DbDataReader reader = cmd.ExecuteReader())
				{
					if (reader.HasRows)
					{
						while (reader.Read())
						{
							//lagrar allt i ett temporärobjekt
							UserData temp = new UserData();

							/* Fy satan, först hämta positionen för index i queryn, sen hämta data
                             * som från positionen och efteråt konvertera till int haha
                             */

							temp.Id = Convert.ToInt16(reader.GetValue(reader.GetOrdinal("id")));
							temp.Name = Master.Text.ToUpperFirstLetter(reader.GetString(reader.GetOrdinal("name")));
							temp.Lastname = Master.Text.ToUpperFirstLetter(reader.GetString(reader.GetOrdinal("lastname")));
							temp.YearofBirth = Convert.ToInt16(reader.GetValue(reader.GetOrdinal("yearofbirth")));
							temp.Rank = Convert.ToInt16(reader.GetValue(reader.GetOrdinal("rank")));

							//returnerar objektet
							return temp;
						}
					}
				}

				//stänger anslutningen såklart
				Master.DB.End(Master.Con);
			}

			return null;
		}

	
        //Returnerar användare ifall den existerar med name och yob
		public UserData Exist(string name, int yob)
		{
            //vi börjar med att kolla cache
            if (Users.Count > 0)
			{
				foreach (var candidate in Users)
				{
                    if (candidate.Value != null)
                        if (candidate.Value.Name.ToLower() == name
                            &&
                            candidate.Value.YearofBirth == yob)
                                 return candidate.Value;
				}
			}

            /* Finns ingen cache kollar vi databasen
             * Öppnar ny anslutning mot databasen
             */ 
			Master.DB.Start(Master.Con);
			{
				MySqlCommand cmd = new MySqlCommand("Select * " +
                                                    "From users " +
                                                    "where name = @name " +
                                                    "AND yearofbirth = @yob LIMIT 1"

													, Master.Con);
				cmd.CommandType = CommandType.Text;

                // Add parameter @lowSalary (more shorter).
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@yob", yob);

				using (DbDataReader reader = cmd.ExecuteReader())
				{
					if (reader.HasRows)
					{
						while (reader.Read())
						{
                            //lagrar allt i ett temporärobjekt
                            UserData temp = new UserData();

                            /* Fy satan, först hämta positionen för index i queryn, sen hämta data
                             * som från positionen och efteråt konvertera till int haha
                             */

                            temp.Id = Convert.ToInt16(reader.GetValue(reader.GetOrdinal("id")));
                            temp.Name = Master.Text.ToUpperFirstLetter(name);
                            temp.Lastname = Master.Text.ToUpperFirstLetter(reader.GetString(reader.GetOrdinal("lastname")));
                            temp.YearofBirth = yob;
                            temp.Rank = Convert.ToInt16(reader.GetValue(reader.GetOrdinal("rank")));

                            //returnerar objektet
                            return temp;
						}
					}
				}

                //stänger anslutningen såklart
				Master.DB.End(Master.Con);
			}

			return null;
		}
    }
}
