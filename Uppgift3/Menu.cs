using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uppgift3
{
    class Menu
    {
        public Bank Bank { get; set; }

        public Menu(Bank bank)
        {
            Bank = bank;

            //Bank baaank = bank
            //bool onlyFirst = false;
            AskForInput(false);
        }

        public void AskForInput(bool onlyFirst)
        {
            if (!onlyFirst)
            {
                Console.WriteLine("HUVUDMENY");
                Console.WriteLine("0) Avsluta och spara\n" + "1) Sök kund\n" +
                    "2) Visa kundbild\n" +
                    "3) Skapa kund\n" +
                    "4) Ta bort kund\n" +
                    "5) Skapa konto\n" +
                    "6) Ta bort konto\n" +
                    "8) Insättning\n" +
                    "8) Uttag\n" +
                    "9) Överföring");
                Console.Write("> ");
                //onlyFirst = true;
            }
            else { Console.Write("> "); }

            string input = Console.ReadLine();

            if (!int.TryParse(input, out int x))
            {
                Console.WriteLine("Skriv in en siffra!");
                AskForInput(true);
                return;
            }

            switch (x)
            {
                case 0:
                    Console.WriteLine("* Avsluta och spara *");
                    //Bank.SaveToFile, antagligen en metod där som anropar datahandler
                    Bank.SaveToFile();
                    break;
                case 1:
                    Console.WriteLine("* Sök kund *");
                    Console.Write("Namn eller postort? ");
                    string query1 = Console.ReadLine().ToUpper();
                    Bank.SearchCustomer(query1);
                    AskForInput(true);
                    break;
                case 2:
                    Console.WriteLine("* Visa kundbild *");
                    Console.Write("Kundnummer? ");
                    string query2 = Console.ReadLine();
                    Bank.ShowInfo(query2);
                    AskForInput(true);
                    break;
                case 3:
                    AddCustomer();
                    break;
                case 4: //Inte helt klar, får inte finnas några konton som tillhör kunden. tror de blev klart, testa lite till.
                    Console.WriteLine("* Ta bort kund *");
                    Console.Write("Kundnummer att ta bort? ");
                    string query3 = Console.ReadLine();
                    Bank.RemoveCustomer(query3);
                    AskForInput(true);
                    break;
                case 5:
                    Console.Write("Kundnummer? ");
                    string query4 = Console.ReadLine();
                    Bank.AddAccount(query4);
                    AskForInput(true);
                    break;
                case 6:
                    break;
                case 7:
                    break;
                case 8:
                    break;
                case 9:
                    break;
                default:
                    Console.WriteLine("Skriv en siffra mellan 0-9");
                    AskForInput(true);
                    break;
            }
            //if (9 >= x && x >= 0)//Byt till switch sats för i helvete
            //{
            //    if (x == 0)//Kod för att anropa kommandon 0-9
            //    {
            //        //Spara
            //    }
            //    else if (x == 1)
            //    {
            //        Console.Write("Namn eller postort? ");
            //        string query = Console.ReadLine();
            //        Bank.SearchCustomer(query);
            //    }
            //    else if (x == 5)
            //    {
            //        AddCustomer();
            //    }
            //}
            //else { Console.WriteLine("Skriv en siffra mellan 0-9"); }
        }

        private void AddCustomer()
        {
            Console.WriteLine("* Skapa kund *");
            Console.Write("Org namn: ");
            string orgNr = Console.ReadLine();//Fixa kan ej vara blankt!
            Console.Write("Företagsnamn: ");
            string compNr = Console.ReadLine();
            Console.Write("Adress: ");
            string adress = Console.ReadLine();
            Console.Write("Stad: ");
            string city = Console.ReadLine();
            Console.Write("Län: ");
            string region = Console.ReadLine();
            Console.Write("Postnummer: ");
            string postNr = Console.ReadLine();
            Console.Write("Land: ");
            string country = Console.ReadLine();
            Console.Write("Telefonnummer: ");
            string phoneNr = Console.ReadLine();

            Bank.AddCustomer(orgNr, compNr, adress, city, region, postNr, country, phoneNr);
            AskForInput(true);
        }
    }
}

//HUVUDMENY
//0) Avsluta och spara
//1) Sök kund
//2) Visa kundbild
//3) Skapa kund
//4) Ta bort kund
//5) Skapa konto
//6) Ta bort konto
//8) Insättning
//8) Uttag
//9) Överföring