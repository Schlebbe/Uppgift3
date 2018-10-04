using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uppgift3
{
    class Program
    {
        static void Main(string[] args)
        {

            //var path = "";
            //path = GetPath();
            DataHandler dataHandler = new DataHandler();
            var bank = dataHandler.ReadFromFile();
            Menu menu = new Menu(bank);
            //Console.WriteLine(bank.Customers[0].CustomerNr);
        }

        //private static string GetPath()
        //{
        //    string path;
        //    try
        //    {
        //        path = @"C:\Users\sebastian\source\repos\Uppgift3\Uppgift3\" + Console.ReadLine();
        //        StreamReader testStream = new StreamReader(path);
        //    }
        //    catch
        //    {
        //        Console.Clear();
        //        Console.WriteLine("Felaktigt filnamn, försök igen!");
        //        path = GetPath();
        //    }

        //    return path;
        //}

        //private static Bank ReadFromFile(string path)
        //{
        //    var bank = new Bank();

        //    using (StreamReader reader = new StreamReader(path))//Lägg till try catch
        //    {
        //        int countOfCustomers = int.Parse(reader.ReadLine());

        //        for (int i = 0; i < countOfCustomers; i++)
        //        {
        //            string line = reader.ReadLine();
        //            string[] cols = line.Split(';');

        //            Customer customer = new Customer
        //            {
        //                CustomerNr = int.Parse(cols[0]),
        //                OrgNr = cols[1],
        //                CompanyName = cols[2],
        //                Adress = cols[3],
        //                City = cols[4],
        //                Region = cols[5],
        //                PostNr = cols[6],
        //                Country = cols[7],
        //                PhoneNr = cols[8]
        //            };

        //            bank.Customers.Add(customer);
        //        }

        //        int countOfAccounts = int.Parse(reader.ReadLine());

        //        for (int i = 0; i < countOfAccounts; i++)
        //        {
        //            string line = reader.ReadLine();
        //            string[] cols = line.Split(';');
        //            //Console.WriteLine(line);
        //            //Console.WriteLine(cols[0]);

        //            Account account = new Account
        //            {
        //                AccountNr = int.Parse(cols[0]),
        //                Owner = int.Parse(cols[1]),
        //                Balance = decimal.Parse(cols[2], CultureInfo.InvariantCulture)
        //            };

        //            bank.Accounts.Add(account);
        //        }
        //    }
        //    return bank;
        //}
    }
}
