using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uppgift3
{
    class DataHandler
    {
        public string test = "";

        public Bank ReadFromFile()
        {
            string path = GetPath();
            Console.WriteLine("\n******************************");
            Console.WriteLine("* VÄLKOMMEN TILL BANKAPP 1.0 *");
            Console.WriteLine("******************************\n");
            Console.WriteLine("Läser in " + test + "...");
            var bank = new Bank();

            using (StreamReader reader = new StreamReader(path))//Lägg till try catch
            {
                int countOfCustomers = int.Parse(reader.ReadLine());

                for (int i = 0; i < countOfCustomers; i++)
                {
                    string line = reader.ReadLine();
                    string[] cols = line.Split(';');

                    Customer customer = new Customer
                    {
                        CustomerNumber = int.Parse(cols[0]),
                        OrganisationNumber = cols[1],
                        CompanyName = cols[2],
                        Adress = cols[3],
                        City = cols[4],
                        Region = cols[5],
                        PostNumber = cols[6],
                        Country = cols[7],
                        PhoneNumber = cols[8]
                    };

                    bank.Customers.Add(customer);
                }

                int countOfAccounts = int.Parse(reader.ReadLine());
                decimal totalBalance = 0;

                for (int i = 0; i < countOfAccounts; i++)
                {
                    string line = reader.ReadLine();
                    string[] cols = line.Split(';');
                    //Console.WriteLine(line);
                    //Console.WriteLine(cols[0]);


                    Account account = new Account
                    {
                        AccountNumber = int.Parse(cols[0]),
                        Owner = int.Parse(cols[1]),
                        Balance = decimal.Parse(cols[2], CultureInfo.InvariantCulture)
                    };
                    totalBalance += account.Balance;

                    bank.Accounts.Add(account);
                }
                Console.WriteLine("Antal kunder: " + countOfCustomers);
                Console.WriteLine("Antal konton: " + countOfAccounts);
                Console.WriteLine("Totalt saldo: " + totalBalance + "\n");
            }
            return bank;
        }

        public void SaveToFile(List<Customer> customers, List<Account> accounts)
        {
            string pathDator = @"C:\Users\sebastian.TEMTRON\Source\Repos\Uppgift3\Uppgift3\";
            string pathLaptop = @"C:\Users\sebastian\source\repos\Uppgift3\Uppgift3\";
            DateTime dt = DateTime.Now;
            string filename = dt.Year + "" + dt.Month + "" + dt.Day + "-" + dt.Hour + dt.Minute + ".txt";
            pathDator = Path.Combine(pathDator, filename);
            //Console.WriteLine("Path to my file: " + pathDator);
            int countOfCustomers = customers.Count();
            int countOfAccounts = accounts.Count();
            decimal totaltBalance = 0;
            //using (FileStream fs = new FileStream(path, FileMode.Append))

            using (StreamWriter streamWriter = new StreamWriter(pathDator))
            {
                streamWriter.WriteLine(countOfCustomers);
                foreach (var c in customers)
                {
                    string line = $"{c.CustomerNumber};{c.OrganisationNumber};{c.CompanyName};{c.Adress};{c.City};{c.Region};{c.PostNumber};{c.Country};{c.PhoneNumber}";
                    streamWriter.WriteLine(line);
                }
                streamWriter.WriteLine(countOfAccounts);
                foreach (var a in accounts)
                {
                    string line = $"{a.AccountNumber};{a.Owner};{a.Balance.ToString(CultureInfo.InvariantCulture)}";
                    streamWriter.WriteLine(line);
                    totaltBalance += a.Balance;
                }
            }
            Console.WriteLine("Sparar till " + filename + "...");
            Console.WriteLine("Antal kunder: " + countOfCustomers);
            Console.WriteLine("Antal konton: " + countOfAccounts);
            Console.WriteLine("Totalt saldo: " + totaltBalance);

        }

        public string GetPath()
        {
            string path;
            string pathDator = @"C:\Users\sebastian.TEMTRON\Source\Repos\Uppgift3\Uppgift3\";
            string pathLaptop = @"C:\Users\sebastian\source\repos\Uppgift3\Uppgift3\";
            try
            {
                test = Console.ReadLine();
                path = pathDator + test;
                StreamReader testStream = new StreamReader(path);
            }
            catch
            {
                Console.Clear();
                Console.WriteLine("Felaktigt filnamn, försök igen!");
                path = GetPath();
            }

            return path;
        }
    }
}
