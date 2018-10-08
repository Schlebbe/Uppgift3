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
                Console.WriteLine("0) Avsluta och spara\n" +
                    "1) Sök kund\n" +
                    "2) Visa kundbild\n" +
                    "3) Skapa kund\n" +
                    "4) Ta bort kund\n" +
                    "5) Skapa konto\n" +
                    "6) Ta bort konto\n" +
                    "7) Insättning\n" +
                    "8) Uttag\n" +
                    "9) Överföring\n" +
                    "10) Visa transaktioner\n" +
                    "11) Ändra ränta\n" +
                    "12) Räkna ränta\n" +
                    "13) Ändra kredit\n" +
                    "14) Ändra skuldränta\n");
                Console.Write("> ");
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
                    DepositMoney();
                    break;
                case 8:
                    WithdrawMoney();
                    break;
                case 9:
                    TransferMoney();
                    break;
                case 10:
                    ShowTransactions();
                    break;
                case 11:
                    ChangeInterest();
                    break;
                case 12:
                    CalculateInterest();
                    break;
                case 13:
                    ChangeCredit();
                    break;
                case 14:
                    ChangeDebtInterest();
                    break;
                default:
                    Console.WriteLine("Skriv en siffra från 0-14");
                    AskForInput(true);
                    break;
            }
        }

        private void SearchCustomer()
        {
            Console.WriteLine("* Sök kund *");
            Console.Write("Namn eller postort? ");
            string query = Console.ReadLine().ToUpper();
            Bank.SearchCustomer(query);
            AskForInput(true);
        } //1

        private void ShowCustomerInfo()
        {
            Console.WriteLine("* Visa kundbild *");
            Console.Write("Kundnummer eller kontonummer? ");
            string query = ParseQuery();
            Bank.ShowInfo(query);
            AskForInput(true);
        } //2

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
            string zipCode = Console.ReadLine();
            Console.Write("Land: ");
            string country = Console.ReadLine();
            Console.Write("Telefonnummer: ");
            string phoneNr = Console.ReadLine();

            Bank.AddCustomer(orgNr, compNr, adress, city, region, zipCode, country, phoneNr);
            AskForInput(true);
        } //3

        private void RemoveCustomer()
        {
            Console.WriteLine("* Ta bort kund *");
            Console.Write("Kundnummer? ");
            string query = ParseQuery();
            Bank.RemoveCustomer(query);
            AskForInput(true);
        } //4

        private void AddAccount()
        {
            Console.Write("Kundnummer? ");
            string customerNumber = ParseQuery();
            Console.Write("Räntesats? ");
            string interest = ParseQuery();//MÅSTE VARA POSITIV!!!
            if (decimal.Parse(interest) > 0)
            {
                Bank.AddAccount(customerNumber, interest);
            }
            else
            {
                Console.WriteLine("Räntesatsen måste vara positiv, försök igen.");
                AddAccount();
            }
            AskForInput(true);
        } //5

        private void RemoveAccount()
        {
            Console.WriteLine("* Ta bort konto *");
            Console.Write("Kontonummer? ");
            var account = ParseQuery();
            Bank.RemoveAccount(account);
            AskForInput(true);
        } //6

        private void DepositMoney()
        {
            Console.WriteLine("* Insättning *");
            Console.Write("Kontonummer? ");
            var accountNumber = ParseQuery();
            Console.Write("Summa? ");
            var amount = ParseQuery();
            Bank.DepositMoney(accountNumber, amount);
            AskForInput(true);
        } //7

        private void WithdrawMoney()
        {
            Console.WriteLine("* Uttag *");
            Console.Write("Kontonummer? ");
            var accountNumber = ParseQuery();
            Console.Write("Summa? ");
            var amount = ParseQuery();
            Bank.WithdrawMoney(accountNumber, amount);
            AskForInput(true);
        } //8

        private void TransferMoney()
        {
            Console.WriteLine("* Överföring *");
            Console.Write("Från kontonummer? ");
            var fromAccount = ParseQuery();
            Console.Write("Till kontonummer? ");
            var toAccount = ParseQuery();
            Console.Write("Summa? ");
            var amount = ParseQuery();
            Bank.TransferMoney(fromAccount, toAccount, amount);
            AskForInput(true);
        } //9

        private void ShowTransactions()
        {
            Console.WriteLine("* Visa transaktioner *");
            Console.Write("Kontonummer? ");
            var account = ParseQuery();
            Bank.ShowTransactions(account);
            AskForInput(true);
        } //10

        private void ChangeInterest()
        {
            Console.WriteLine("* Ändra räntesatsen *");
            Console.Write("Kontonummer? ");
            var account = ParseQuery();
            Console.Write("Nya räntesatsen? ");
            var newInterest = ParseQuery();
            if (decimal.Parse(newInterest) > 0)
            {
                Bank.ChangeInterest(account, newInterest);
            }
            else
            {
                Console.WriteLine("Räntesatsen måste vara positiv, försök igen.");
                ChangeInterest();
            }
            AskForInput(true);
        } //11

        private void CalculateInterest()
        {
            Console.WriteLine("* Räkna ränta *");
            Bank.CalculateInterest();
            Console.WriteLine("\nRäntesatsen har lagts till på alla konton.");
            AskForInput(true);
        } //12

        private void ChangeCredit()
        {
            Console.WriteLine("* Ändra kredit *");
            Console.Write("Kontonummer? ");
            var account = ParseQuery();
            Console.Write("Nya krediten? ");
            var newCredit = ParseQuery();
            Bank.ChangeCredit(account, newCredit);
            AskForInput(true);
        } //13

        private void ChangeDebtInterest()
        {
            Console.WriteLine("* Ändra skuldräntan *");
            Console.Write("Kontonummer? ");
            var account = ParseQuery();
            Console.Write("Nya skuldräntan? ");
            var newDebtInterest = ParseQuery();
            Bank.ChangeDebtInterest(account, newDebtInterest);
            AskForInput(true);
        } //14

        private string ParseQuery()
        {
            var query = Console.ReadLine();
            if (!int.TryParse(query, out int result))
            {
                Console.Write("Måste vara ett heltal!\nFörsök igen: "); query = ParseQuery();
            }

            return query;
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
//7) Insättning
//8) Uttag
//9) Överföring
//10) Ränta