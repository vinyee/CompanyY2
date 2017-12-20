using System;
using System.Collections.Generic;
using CompanyYV2.Classes.Users;

namespace CompanyYV2.Floors
{
    public class Floor1
    {
        public void CheckLogin(int level = 0, string name = "", int yob = 0)
        {          
            if (level == 0)
                CollectName(name);
            else
                CollectAge(name, yob);
        }

        public void TryLogin(string name, int yob)
        {
            UserData user = Master.UserManager.Exist(name, yob);

            //Användaren hittas ej i varken cache/db
            if (user == null)
            {
                //Utan return fortsätter koden, då blir det error för user är null
                BadLogin();
                return;
            }

            //sparar i cache så vi slipper ladda om ifall usern reloggar
            Add(user);

            Master.LoggedIn.UserWelcome(user, true);
        }

        public void Add(UserData user)
        {
			//Sparar i cache slipper db sen vid login
			Master.UserManager.Add(user);
        }

        public void BadLogin()
        {
            //antingen fel namn och yob eller ingen användare i både cache/db
            Console.WriteLine("");
            Console.WriteLine("Det verkar som att användaren inte existerar!");
            Console.WriteLine("Vill du försöka igen eller gå tillbaka och registrera dig?"
                              + Environment.NewLine);

            List<string> Options = new List<string>
            {
                "Försök igen",
                "Ta mig tillbaka till framsidan"
            };

            int option = Master.Text.Menu(Options);

            Console.Clear();

            if (option == 0)
                CheckLogin(0);
            else
                Master.LoggedOut.Main();
        }

        public void CollectName(string name)
        {
			Console.WriteLine("Kan jag få ditt förnamn?");
			name = Console.ReadLine().ToLower();

            if (Master.Text.OnlyLetters(name))
                CheckLogin(1, name);
            else
            {
                Console.WriteLine("Något blev fel, ditt namn får inte " +
                    "innehålla siffror eller tecken. Försök igen");

                CheckLogin(0);
            }
        }

        public void CollectAge(string name, int yob)
        {
			Console.WriteLine("Vilket år är du född?");
			yob = Master.Text.GiveInt(Console.ReadLine());

            if (Master.Text.ValidAge(yob))
            {
                TryLogin(name, yob);
                return;
            }
            else
            {
                Console.WriteLine("Något blev fel, ditt födelseår får inte " +
                    "innehålla bokstäver eller tecken. Försök igen");

                CheckLogin(1, name);
            }
        }
    }
}
