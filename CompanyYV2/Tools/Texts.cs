using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using CompanyYV2.Classes.Jobs;
using CompanyYV2.Classes.Users;

namespace CompanyYV2.Tools
{
    public class Texts
    {

        public int GetAge(int year)
        {
            return DateTime.Now.Year - year;
        }

        public bool ValidName(string name)
        {
            return (name.Length > Master.NameMin && name.Length < Master.NameMax && OnlyLetters(name));
        }

        public bool ValidAge(int year)
        {
            return (GetAge(year) > Master.AgeMin && GetAge(year) < Master.AgeMax);
        }

        public string ToUpperFirstLetter(string input)
        {
            //https://stackoverflow.com/questions/4135317/make-first-letter-of-a-string-upper-case-with-maximum-performance

            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower());
        }

        public int GiveInt(string input)
        {
            int.TryParse(input, out int output);

            return output;
        }

        public bool OnlyLetters(string input)
        {
            //https://stackoverflow.com/questions/1181419/verifying-that-a-string-contains-only-letters-in-c-sharp

            return (Regex.IsMatch(input, @"^[a-öA-Ö]+$"));
        }

        public string JobStage(int input)
        {
            switch (input)
            {
                case 0:
                    return "Ansökt";

                case 1:
                    return "Kallad till intervju";

                case 2:
                    return "Kallad till IQ-test";

                case 3:
                    return "Gick till någon annan tyvärr :(";

            }

            return "Jobbstage Error";
        }

        public int Menu(List<string> inArray)
        {
            //Källa
            //https://gist.github.com/Brogie/34dba368a34c57049b46

            bool loopComplete = false;
            int topOffset = Console.CursorTop;
            int bottomOffset = 0;
            int selectedItem = 0;
            ConsoleKeyInfo kb;

            Console.CursorVisible = false;

            /**
             * Drawing phase
             * */
            while (!loopComplete)
            {//This for loop prints the array out
                for (int i = 0; i < inArray.Count; i++)
                {

                    if (i == selectedItem)
                    {//This section is what highlights the selected item
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine("- " + inArray[i]);
                        Console.ResetColor();
                    }
                    else
                    {//this section is what prints unselected items
                        Console.WriteLine("- " + inArray[i]);
                    }
                }

                bottomOffset = Console.CursorTop;

                /*
                 * User input phase
                 * */

                kb = Console.ReadKey(true); //read the keyboard

                switch (kb.Key)
                { //react to input
                    case ConsoleKey.UpArrow:
                        if (selectedItem > 0)
                        {
                            selectedItem--;
                        }
                        else
                        {
                            selectedItem = (inArray.Count - 1);
                        }
                        break;

                    case ConsoleKey.DownArrow:
                        if (selectedItem < (inArray.Count - 1))
                        {
                            selectedItem++;
                        }
                        else
                        {
                            selectedItem = 0;
                        }
                        break;

                    case ConsoleKey.Enter:
                        {
                            loopComplete = true;
                        }
                        break;
                }
                //Reset the cursor to the top of the screen
                Console.SetCursorPosition(0, topOffset);
            }
            //set the cursor just after the menu so that the program can continue after the menu
            Console.SetCursorPosition(0, bottomOffset);

            Console.CursorVisible = true;

            return selectedItem;
        }

        public UserData Menus(List<UserData> inArray, string title = "", bool end = false)
        {
            //Källa
            //https://gist.github.com/Brogie/34dba368a34c57049b46

            bool loopComplete = false;
            int topOffset = Console.CursorTop;
            int bottomOffset = 0;
            int selectedItem = 0;
            ConsoleKeyInfo kb;

            Console.CursorVisible = false;

            /**
             * Drawing phase
             * */
            while (!loopComplete)
            {//This for loop prints the array out
                for (int i = 0; i < inArray.Count; i++)
                {

                    if (i == selectedItem)
                    {//This section is what highlights the selected item
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.Black;
                        if (i == (inArray.Count - 1))
                            if (end)
                                Console.WriteLine("- " + inArray[i].Name);
                            else
                                Console.WriteLine("- " + title + inArray[i].Name);
                        else
                            Console.WriteLine("- " + title + inArray[i].Name);
                        Console.ResetColor();
                    }
                    else
                    {//this section is what prints unselected items
                        if (i == (inArray.Count - 1))
                            if (end)
                                Console.WriteLine("- " + inArray[i].Name);
                            else
                                Console.WriteLine("- " + title + inArray[i].Name);
                        else
                            Console.WriteLine("- " + title + inArray[i].Name);
                    }
                }

                bottomOffset = Console.CursorTop;

                /*
                 * User input phase
                 * */

                kb = Console.ReadKey(true); //read the keyboard

                switch (kb.Key)
                { //react to input
                    case ConsoleKey.UpArrow:
                        if (selectedItem > 0)
                        {
                            selectedItem--;
                        }
                        else
                        {
                            selectedItem = (inArray.Count - 1);
                        }
                        break;

                    case ConsoleKey.DownArrow:
                        if (selectedItem < (inArray.Count - 1))
                        {
                            selectedItem++;
                        }
                        else
                        {
                            selectedItem = 0;
                        }
                        break;

                    case ConsoleKey.Enter:
                        {
                            loopComplete = true;
                        }
                        break;
                }
                //Reset the cursor to the top of the screen
                Console.SetCursorPosition(0, topOffset);
            }
            //set the cursor just after the menu so that the program can continue after the menu
            Console.SetCursorPosition(0, bottomOffset);

            Console.CursorVisible = true;

            return inArray[selectedItem];
        }


        public JobsData Menus(List<JobsData> inArray, string title = "", bool end = false)
        {
            //Källa
            //https://gist.github.com/Brogie/34dba368a34c57049b46

            bool loopComplete = false;
            int topOffset = Console.CursorTop;
            int bottomOffset = 0;
            int selectedItem = 0;
            ConsoleKeyInfo kb;

            Console.CursorVisible = false;

            /**
             * Drawing phase
             * */
            while (!loopComplete)
            {//This for loop prints the array out
                for (int i = 0; i < inArray.Count; i++)
                {

                    if (i == selectedItem)
                    {//This section is what highlights the selected item
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.Black;
                        if (i == (inArray.Count - 1))
                            if (end)
                                Console.WriteLine("- " + inArray[i].Title);
                            else
                                Console.WriteLine("- " + title + inArray[i].Title);
                        else
                            Console.WriteLine("- " + title + inArray[i].Title);
                        Console.ResetColor();
                    }
                    else
                    {//this section is what prints unselected items
                        if (i == (inArray.Count - 1))
                            if (end)
                                Console.WriteLine("- " + inArray[i].Title);
                            else
                                Console.WriteLine("- " + title + inArray[i].Title);
                        else
                            Console.WriteLine("- " + title + inArray[i].Title);
                    }
                }

                bottomOffset = Console.CursorTop;

                /*
                 * User input phase
                 * */

                kb = Console.ReadKey(true); //read the keyboard

                switch (kb.Key)
                { //react to input
                    case ConsoleKey.UpArrow:
                        if (selectedItem > 0)
                        {
                            selectedItem--;
                        }
                        else
                        {
                            selectedItem = (inArray.Count - 1);
                        }
                        break;

                    case ConsoleKey.DownArrow:
                        if (selectedItem < (inArray.Count - 1))
                        {
                            selectedItem++;
                        }
                        else
                        {
                            selectedItem = 0;
                        }
                        break;

                    case ConsoleKey.Enter:
                        {
                            loopComplete = true;
                        }
                        break;
                }
                //Reset the cursor to the top of the screen
                Console.SetCursorPosition(0, topOffset);
            }
            //set the cursor just after the menu so that the program can continue after the menu
            Console.SetCursorPosition(0, bottomOffset);

            Console.CursorVisible = true;

            return inArray[selectedItem];
        }

        public int Menu(string[] inArray)
        {
            //Källa
            //https://gist.github.com/Brogie/34dba368a34c57049b46

            bool loopComplete = false;
            int topOffset = Console.CursorTop;
            int bottomOffset = 0;
            int selectedItem = 0;
            ConsoleKeyInfo kb;

            Console.CursorVisible = false;

            /**
             * Drawing phase
             * */
            while (!loopComplete)
            {//This for loop prints the array out
                for (int i = 0; i < inArray.Length; i++)
                {

                    if (i == selectedItem)
                    {//This section is what highlights the selected item
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine("- " + inArray[i]);
                        Console.ResetColor();
                    }
                    else
                    {//this section is what prints unselected items
                        Console.WriteLine("- " + inArray[i]);
                    }
                }

                bottomOffset = Console.CursorTop;

                /*
                 * User input phase
                 * */

                kb = Console.ReadKey(true); //read the keyboard

                switch (kb.Key)
                { //react to input
                    case ConsoleKey.UpArrow:
                        if (selectedItem > 0)
                        {
                            selectedItem--;
                        }
                        else
                        {
                            selectedItem = (inArray.Length - 1);
                        }
                        break;

                    case ConsoleKey.DownArrow:
                        if (selectedItem < (inArray.Length - 1))
                        {
                            selectedItem++;
                        }
                        else
                        {
                            selectedItem = 0;
                        }
                        break;

                    case ConsoleKey.Enter:
                        {
                            loopComplete = true;
                        }
                        break;
                }
                //Reset the cursor to the top of the screen
                Console.SetCursorPosition(0, topOffset);
            }
            //set the cursor just after the menu so that the program can continue after the menu
            Console.SetCursorPosition(0, bottomOffset);

            Console.CursorVisible = true;

            return selectedItem;
        }
    }
}
