using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using CompanyYV2.Classes.Users;
using MySql.Data.MySqlClient;

namespace CompanyYV2.Classes.Jobs
{
    public class JobsManager
    {
        public List<JobsData> Jobs;

        public JobsManager()
        {
            Jobs = new List<JobsData>();
        }

        public void ShowMy(UserData user)
        {
            if (user.MyJobs.Count == 0)
            {
                Console.WriteLine("Du har inte sökt några jobb!");
                Console.WriteLine("");
                return;
            }

            foreach (JobsData job in user.MyJobs)
            {
                Console.WriteLine("Titel: " + job.Title);
                Console.WriteLine("Status: " + Master.Text.JobStage(job.Stage));
                Console.WriteLine("");
            }
		}

		public void ShowAll(UserData user)
		{
            if (Jobs.Count == 0)
			{
				Console.WriteLine("Vi har inte lagt ut några jobbannonser ute just nu tyvärr!");
				Console.WriteLine("");
				return;
			}

            Console.WriteLine("Vi har " + Jobs.Count + " jobbannonser ute just nu: ");
            Console.WriteLine("");
            foreach (JobsData job in Jobs)
			{
				Console.WriteLine("Titel: " + job.Title);
				Console.WriteLine("Beskrivning: Finns för nuvarande ingen beskrivning angående jobbet");
				Console.WriteLine("");
			}
		}

        public List<JobsData> ShowMyAvailable(UserData user)
		{
            List<JobsData> temp = new List<JobsData>();

            Console.WriteLine("Följande jobb finns att söka just nu som du inte har sökt:");
            Console.WriteLine("");

            foreach (JobsData job in Jobs)
			{
                //kollar så vi inte har jobbet i vår jobblista
                if (!Exist(user, job))
                {
                    Console.WriteLine("Titel: " + job.Title);
                    Console.WriteLine("Beskrivning: Finns för nuvarande ingen beskrivning angående jobbet");
                    Console.WriteLine("");

                    if (!temp.Contains(job))
                        temp.Add(job);
                }
			}

            return temp;
		}

        public List<string> ToString(List<JobsData> jobs)
        {
            List<string> temp = new List<string>();

            foreach (JobsData job in jobs)
                if (job != null)
                    temp.Add(job.Title);
            
            return temp;
                
        }

        public void LoadStaticDB()
        {
			Master.DB.Start(Master.Con);
			{
				MySqlCommand cmd = new MySqlCommand("Select * " +
													"From jobs "
													, Master.Con)
				{
					CommandType = CommandType.Text
				};

				using (DbDataReader reader = cmd.ExecuteReader())
				{
					if (reader.HasRows)
					{
						while (reader.Read())
						{
							JobsData temp = new JobsData();

							temp.Id = Convert.ToInt16(reader.GetValue(reader.GetOrdinal("id")));
                            temp.Title = Master.Text.ToUpperFirstLetter(reader.GetString(reader.GetOrdinal("title")));

                            if (temp != null)
                                Jobs.Add(temp);
						}
					}
				}

				//stänger anslutningen såklart
				Master.DB.End(Master.Con);
			}
        }

		public bool ExistList(JobsData job)
		{
			if (Jobs.Count > 0)
				if (Jobs.Contains(job))
					return true;

			return false;
		}

		public void RemoveList(JobsData job)
		{
			if (ExistList(job))
				Jobs.Remove(job);
		}

		public void AddList(JobsData job)
		{
            if (ExistList(job))
                Jobs.Add(job);
		}

        public void Update(UserData user, JobsData job)
        {
            Master.DB.Start(Master.Con);
            {
                MySqlCommand cmd = new MySqlCommand("UPDATE users_jobs " +
                                                    "SET stage = @stage," +
                                                    "points = @points, " +
                                                    "time = @time," +
                                                    "vip = @vip " +
                                                    "WHERE userid = @userid " +
                                                    "AND jobid = @jobid " 
                                                    , Master.Con);

                cmd.Parameters.Add(new MySqlParameter("@stage", MySqlDbType.Int16, 2));
                cmd.Parameters["@stage"].Value = job.Stage;

				cmd.Parameters.Add(new MySqlParameter("@points", MySqlDbType.Int16, 2));
                cmd.Parameters["@points"].Value = job.Points;

				cmd.Parameters.Add(new MySqlParameter("@time", MySqlDbType.Int16, 11));
                cmd.Parameters["@time"].Value = job.Time;

				cmd.Parameters.Add(new MySqlParameter("@vip", MySqlDbType.Int16, 1));
                cmd.Parameters["@vip"].Value = job.Vip;

				cmd.Parameters.Add(new MySqlParameter("@userid", MySqlDbType.Int16, 11));
                cmd.Parameters["@userid"].Value = job.UserId;

				cmd.Parameters.Add(new MySqlParameter("@jobid", MySqlDbType.Int16, 3));
                cmd.Parameters["@jobid"].Value = job.Id;

				// Execute Command (for Delete, Insert or Update).
				cmd.ExecuteNonQuery();

                //stänger anslutningen såklart
                Master.DB.End(Master.Con);
            }
        }

		public void Add(UserData user, JobsData job, bool db = false)
		{
            if (!Exist(user, job))
            {
                user.MyJobs.Add(job);

                if (db)
                {
                    Master.DB.Start(Master.Con);
                    {
                        MySqlCommand cmd = new MySqlCommand("INSERT INTO users_jobs (userid, jobid) "
                                                            + " values (@userid, @jobid) ", Master.Con);

                        cmd.Parameters.Add(new MySqlParameter("@userid", MySqlDbType.Int16, 11));
                        cmd.Parameters["@userid"].Value = user.Id;

                        cmd.Parameters.Add(new MySqlParameter("@jobid", MySqlDbType.Int16, 11));
                        cmd.Parameters["@jobid"].Value = job.Id;

                        // Execute Command (for Delete, Insert or Update).
                        cmd.ExecuteNonQuery();

                        //stänger anslutningen såklart
                        Master.DB.End(Master.Con);
                    }
                }
            }
		}

		public void Remove(UserData user, JobsData job, bool db = false)
		{
            if (Exist(user, job))
            {
                user.MyJobs.Remove(job);

                if (db)
                {
                    Master.DB.Start(Master.Con);
                    {
                        MySqlCommand cmd = new MySqlCommand("DELETE FROM users_jobs WHERE userid = @userid" +
                                                            "AND jobid = @jobid", Master.Con);

                        cmd.Parameters.Add(new MySqlParameter("@userid", MySqlDbType.Int16, 11));
                        cmd.Parameters["@userid"].Value = user.Id;

                        cmd.Parameters.Add(new MySqlParameter("@jobid", MySqlDbType.Int16, 11));
                        cmd.Parameters["@jobid"].Value = job.Id;

                        // Execute Command (for Delete, Insert or Update).
                        cmd.ExecuteNonQuery();

                        //stänger anslutningen såklart
                        Master.DB.End(Master.Con);
                    }
                }
            }
		}

        public bool Exist(UserData user, JobsData job)
        {
            return (user.MyJobs.Contains(job));
        }

		public int Count(UserData user)
		{
            return (user.MyJobs.Count);
		}

        public JobsData LoadById(int id)
        {
            foreach (JobsData job in Jobs)
            {
                if (job != null)
                    if (job.Id == id)
                        return job;
            }

            return null;
        }

		//Laddar alla jobb där någon har sökt
		public List<JobsData> LoadAllDB(UserData user)
		{
            List<JobsData> users = new List<JobsData>();

			Master.DB.Start(Master.Con);
			{
				MySqlCommand cmd = new MySqlCommand("SELECT * " +
													"FROM users_jobs "
													, Master.Con)
				{
					CommandType = CommandType.Text
				};

				using (DbDataReader reader = cmd.ExecuteReader())
				{
					if (reader.HasRows)
					{
						while (reader.Read())
						{
							JobsData temp = this.LoadById(Convert.ToInt16(reader.GetValue(reader.GetOrdinal("jobid"))));

                            //ansökarens stage
							temp.Stage = Convert.ToInt16(reader.GetValue(reader.GetOrdinal("stage")));
                            temp.UserId = Convert.ToInt16(reader.GetValue(reader.GetOrdinal("userid")));

                            if (temp != null)
                                if (!users.Contains(temp))
                                    users.Add(temp);
						}
					}
				}

				//stänger anslutningen såklart
				Master.DB.End(Master.Con);
			}

            return users;
		}

        //Laddar dina jobb från db, kollar sedan listan från allmänna jobs
        public void LoadMyDB(UserData user)
        {
            Master.DB.Start(Master.Con);
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * " +
                                                    "FROM users_jobs " +
                                                    "WHERE userid = @userid "
                                                    , Master.Con)
                {
                    CommandType = CommandType.Text
                };

                cmd.Parameters.AddWithValue("@userid", user.Id);

                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            JobsData temp = this.LoadById(Convert.ToInt16(reader.GetValue(reader.GetOrdinal("jobid"))));

                            //din nivå i jobbet, intervju > iqtest osv..
                            temp.Stage = Convert.ToInt16(reader.GetValue(reader.GetOrdinal("stage")));


                            if (temp != null)
                            if (!Exist(user, temp))
                                    Add(user, temp);
                        }
                    }
                }

                //stänger anslutningen såklart
                Master.DB.End(Master.Con);
            }
        }
    }
}
