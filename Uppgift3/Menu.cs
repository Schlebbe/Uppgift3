﻿using System;
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
                Console.Write("\n> ");
            }
            else { Console.Write("\n> "); }

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
                    Bank.SaveToFile();
                    break;
                case 1:
                    SearchCustomer();
                    break;
                case 2:
                    ShowCustomerInfo();
                    break;
                case 3:
                    AddCustomer();
                    break;
                case 4: //Kunna ta bort om saldot på kontona är 0? Lägg till kod för det ifall att?
                    RemoveCustomer();
                    break;
                case 5:
                    AddAccount();
                    break;
                case 6:
                    RemoveAccount();
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

        private void SearchCustomer()
        {
            Console.WriteLine("* Sök kund *");
            Console.Write("Namn eller postort? ");
            string query = Console.ReadLine().ToUpper();
            Bank.SearchCustomer(query);
            AskForInput(true);
        }

        private void ShowCustomerInfo()
        {
            Console.WriteLine("* Visa kundbild *");
            Console.Write("Kundnummer eller kontonummer? ");
            string query = Console.ReadLine();
            Bank.ShowInfo(query);
            AskForInput(true);
        }

        private void AddCustomer()
        {
            Console.WriteLine("* Skapa kund *");
            Console.Write("Organisationsnummer: ");
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

        private void RemoveCustomer()
        {
            Console.WriteLine("* Ta bort kund *");
            Console.Write("Kundnummer? ");
            string query = Console.ReadLine();
            Bank.RemoveCustomer(query);
            AskForInput(true);
        }

        private void AddAccount()
        {
            Console.Write("Kundnummer? ");
            string query = Console.ReadLine();
            Bank.AddAccount(query);
            AskForInput(true);
        }

        private void RemoveAccount()
        {
            Console.WriteLine("* Ta bort konto *");
            Console.Write("Kontonummer? ");
            string query = Console.ReadLine();
            Bank.RemoveAccount(query);
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