using System;
using System.Collections.Generic;
using CompanyYV2.Classes.Users;

namespace CompanyYV2.Floors
{
    public class Floor
    {
        public Floor()
        {
        }

		public void Main()
		{
            List<string> Options = new List<string>
            {
                "Logga in",
                "Registrera",
                "Tipsa rekryterare"
            };

            int option = Master.Text.Menu(Options);

            Console.Clear();

			switch (option)
			{
				case 0: 
                    Login(); 
                    break;

                case 1: 
                    Register(); 
                    break;

                case 2: 
                    RecommendToRecruit(); 
                    break;
			}
		}

        public void Login()
        {
			Console.WriteLine("Nu ska vi logga in, då behöver vi två av dina " +
	            "uppgifter för att kontrollera vem du är."
	            + Environment.NewLine);

            Master.Login.CheckLogin();
        }

        public void Logout()
        {
            Console.Clear();
            Console.WriteLine("Du har nu loggat ut, välkommen åter!" + Environment.NewLine);
            Master.LoggedOut.Main();
        }

        public void Register()
        {
            UserData user = new UserData();
            RegisterStage(user);
        }

		public void RegisterStage(UserData user, int level = 0, bool finished = false)
		{
			switch (level)
			{
				case 0:
					CollectName(user, level, finished);
					break;

				case 1:
					CollectLastname(user, level, finished);
					break;

				case 2:
					CollectAge(user, level, finished);
					break;

                case 3:
                    RegisterFinish(user);
                    break;
			}
		}

        public void RegisterFinish(UserData user)
        {
            //Lägg till i listan på ny medlem
            Console.Clear();
            Console.WriteLine("Grattis du är nu medlem, " +
                              "du kan logga in!" +
                              Environment.NewLine);

            //egentligen kan vi köra såhär men det är bra att kontrollera
            //att allt sparades i databasen så vi skippar
			//Master.UserManager.Add(user);

            Master.UserManager.AddDB(user);
            Master.LoggedOut.Main();
        }

		public void CollectName(UserData user, int level, bool finished = false)
		{
			Console.WriteLine("Vad heter du i förnamn?");

            user.Name = Master.Text.ToUpperFirstLetter(Console.ReadLine());

            if (!Master.Text.ValidName(user.Name))
			{
				Console.WriteLine("Något blev fel, " +
								  "se till att inga tecken och siffror finns med!");

                RegisterStage(user, level, finished);
			}
			else
			{
				if (!finished)
				{
					level++;
                    RegisterStage(user, level);
				}
			}
		}

		public void CollectLastname(UserData user, int level, bool finished = false)
		{
			Console.WriteLine("Vad heter du i efternamn?");

            user.Lastname = Master.Text.ToUpperFirstLetter(Console.ReadLine());

            if (!Master.Text.ValidName(user.Lastname))
			{
				Console.WriteLine("Något blev fel, " +
								  "se till att inga tecken och siffror finns med!");

				RegisterStage(user, level, finished);
			}
			else
			{
				if (!finished)
				{
					level++;
					RegisterStage(user, level);
				}
			}
		}

		public void CollectAge(UserData user, int level, bool finished = false)
		{
			Console.WriteLine("Vilket år är du född då?");

            user.YearofBirth = Master.Text.GiveInt(Console.ReadLine());

            if (Master.Text.ValidAge(user.YearofBirth))
			{
				if (!finished)
				{
					level++;
                    RegisterStage(user, level);
				}
			}
			else
			{
				Console.WriteLine("Något blev fel, " +
				  "se till att inga tecken och bokstäver finns med!");

				RegisterStage(user, level, finished);
			}
		}

        public void RecommendToRecruit()
        {
            
        }

		public void WelcomeMessage()
		{
			Console.WriteLine("\t\t\tCompany Environment" +
							  Environment.NewLine +
							  Environment.NewLine);

			Console.WriteLine("Välkommen till Company Environment. " +
							  "I vår Environment kan du som rekryterare lägga " +
							  "till och ta bort kandidater. Söka upp specfika " +
							  "roller för att lista vilka kandidater som finns " +
							  "och eventuellt spara ihop listan i en fil om så " +
							  "önskas." +
							  Environment.NewLine);

			Console.WriteLine("Som kandidat kan du ifall en rekryterare har lagt " +
							  "till dig som intressant påbörja intervju. Har du " +
							  "redan varit på intervjun är du välkommen för nästa " +
							  "del som testar dina kunskaper inom området " +
							  "som du söker." + Environment.NewLine);

			Console.WriteLine("Är du varken en rekryterare eller kandidat? " +
							  "Ingen fara, då kan du tipsa om intressanta " +
							  "kandidater till våra rekryterare!" +
							  Environment.NewLine);
		}
    }
}
