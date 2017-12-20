using System;
using System.Collections.Generic;
using CompanyYV2.Classes.Jobs;
using CompanyYV2.Classes.Users;

namespace CompanyYV2.Floors
{
    public class Floor2
    {
        public Floor2()
        {
		}

        public void RecruitWelcome(UserData user, bool firsttime = false)
        {
            if (firsttime)
			{
                Console.Clear();
				Console.WriteLine("Välkommen " + user.Name + "!");
				Console.WriteLine("Här kan du som rekryterare hitta potentiella arbetssökare, " +
								  "kolla vilka som har sökt jobb samt kalla till intervju!" +
								  Environment.NewLine);
			}

			List<string> Options = new List<string>
			{
                "Min information",
				"Visa mig våra jobbannonser",
				"Visa mig våra nuvarande ansökningar som har kommit in",
				"Logga ut"
			};

			int option = Master.Text.Menu(Options);

			switch (option)
			{
				case 0:
					ShowInfo(user);
					break;

				case 1:
                    ShowOurJobs(user);
					break;

				case 2:
					ShowCandidates(user);
					break;

				case 3:
					Master.LoggedOut.Logout();
					break;

			}
		}

        public void ShowCandidates(UserData user)
        {
			List<JobsData> jobs = Master.JobsManager.LoadAllDB(user);
			List<UserData> users = new List<UserData>();

			Console.Clear();

			foreach (JobsData tempjob in jobs)
			{
				UserData tempuser = Master.UserManager.Load(tempjob.UserId);

				if (tempuser != null)
				{
					Console.WriteLine("Title: " + tempjob.Title);
					Console.WriteLine("Namn: " + tempuser.Name);
					Console.WriteLine("Efternamn: " + tempuser.Lastname);
					Console.WriteLine("Ålder: " + Master.Text.GetAge(tempuser.YearofBirth));
					Console.WriteLine("Status: " + Master.Text.JobStage(tempjob.Stage));
					Console.WriteLine("");

                    if (!users.Contains(tempuser))
                    {
                        if (tempjob.Stage == 0)
                        {
                            //vi måste lagra tempjobbet så vi kan uppdatera jobbets läge sen
                            if (!tempuser.MyJobs.Contains(tempjob))
                                tempuser.MyJobs.Add(tempjob);

                            users.Add(tempuser);
                        }
                    }
				}
			}

			UserData exit = new UserData() { Name = "Ta mig tillbaka till menyn", Id = -1 };
			users.Add(exit);

			//Listar alternativ
            UserData candidate = Master.Text.Menus(users, "Kalla till intervju, ", true);

            if (candidate.Name.ToLower().Contains("tillbaka"))
            {
                Console.Clear();
				UserWelcome(user);
            }
            else
            {
                Console.Clear();
                CallToInterview(user, candidate);
            }
        }

        public void CallToInterview(UserData user, UserData target)
        {
            target.MyJobs[0].Stage = 1;
            Master.JobsManager.Update(target, target.MyJobs[0]);

            Console.Clear();
            Console.WriteLine("Du har nu kallat " + target.Name + " till intervju för " +
                              target.MyJobs[0].Title);
            
            Console.WriteLine("");
            ShowCandidates(user);
        }

        public void UserWelcome(UserData user, bool firsttime = false)
        {
            if (user.Rank > 1)
            {
                RecruitWelcome(user, firsttime);
                return;
            }

            if (firsttime)
            {
                //Vi laddar mina jobb
                Master.JobsManager.LoadMyDB(user);

                Console.Clear();
                Console.WriteLine("Här är din sida med flera alternativ " +
                                  "som gör att du enkelt håller dig själv uppdaterad." +
                                  Environment.NewLine);

                Console.WriteLine("Kanske har du tur och en intervju väntar på dig? " +
                                  "Är du redo så är det bara att påbörja :)" +
                                  Environment.NewLine);
            }

            List<string> Options = new List<string>
            {
                "Min information",
                "Visa mig lediga jobb",
                "Mina pågående jobbansökan",
                "Logga ut"
            };

            int option = Master.Text.Menu(Options);

            switch (option)
            {
                case 0:
                    ShowInfo(user);
                    break;

				case 1:
                    ShowAvailableJobs(user);
					break;

                case 2:
                    ShowJobs(user);
                    break;

                case 3:
                    Master.LoggedOut.Logout();
                    break;
                    
            }
        }

        public void ShowJobs(UserData user)
        {
            Console.Clear();
            Master.JobsManager.ShowMy(user);
			UserWelcome(user);
        }

		public void ShowOurJobs(UserData user)
		{
			Console.Clear();
            Master.JobsManager.ShowAll(user);
            UserWelcome(user);
		}

        public void ShowAvailableJobs(UserData user)
        {
			Console.Clear();
            List<JobsData> temp = Master.JobsManager.ShowMyAvailable(user);

            JobsData exit = new JobsData() { Title = "Ta mig tillbaka till menyn" };
            temp.Add(exit);

            //Listar alternativ
            JobsData job = Master.Text.Menus(temp, "Ansök till ", true);

            if (job.Title.Contains("tillbaka"))
            {
                Console.Clear();
                UserWelcome(user);
            }

            //Sparar jobbet i vår jobblista samt db
            Master.JobsManager.Add(user, job, true);
            Console.Clear();
            Console.WriteLine("Du har precis ansökt till " + job.Title + "" +
                              ", vi håller tummarna!");
            Console.WriteLine("");

            ShowJobs(user);
        }

        public void ShowInfo(UserData user)
        {
            Console.Clear();
            Master.UserManager.Show(user);

			List<string> Options = new List<string>
			{
				"Ändra min information",
				"Ta mig tillbaka till framsidan"
			};

			int option = Master.Text.Menu(Options);

            if (option == 0)
                ChangeInfo(user);
            else
            {
                Console.Clear();
                UserWelcome(user);
            }
        }

        public void ChangeInfo(UserData user)
        {
            while (true)
            {
                Console.Clear();
                Master.UserManager.Show(user);

                List<string> Options = new List<string>
                {
                    "Namn",
                    "Efternamn",
                    "Födelseår",
                    "Jag är klar!"
                };

                int option = Master.Text.Menu(Options);

                switch (option)
                {
                    case 0:
                        Master.LoggedOut.CollectName(user, 0, true);
                        break;

                    case 1:
                        Master.LoggedOut.CollectLastname(user, 1, true);
                        break;

                    case 2:
                        Master.LoggedOut.CollectAge(user, 2, true);
                        break;

                    case 3:
                        {
                            //uppdaterar med våra nya uppgifter i db
                            Master.UserManager.UpdateDB(user);

                            Console.Clear();
                            UserWelcome(user);
                            return;
                        }
                }
            }
        }
    }
}
