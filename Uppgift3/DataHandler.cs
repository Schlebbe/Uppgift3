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
        public Bank ReadFromFile()
        {
            string path = GetPath();

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
                        CustomerNr = int.Parse(cols[0]),
                        OrgNr = cols[1],
                        CompanyName = cols[2],
                        Adress = cols[3],
                        City = cols[4],
                        Region = cols[5],
                        PostNr = cols[6],
                        Country = cols[7],
                        PhoneNr = cols[8]
                    };

                    bank.Customers.Add(customer);
                }

                int countOfAccounts = int.Parse(reader.ReadLine());

                for (int i = 0; i < countOfAccounts; i++)
                {
                    string line = reader.ReadLine();
                    string[] cols = line.Split(';');
                    //Console.WriteLine(line);
                    //Console.WriteLine(cols[0]);

                    Account account = new Account
                    {
                        AccountNr = int.Parse(cols[0]),
                        Owner = int.Parse(cols[1]),
                        Balance = decimal.Parse(cols[2], CultureInfo.InvariantCulture)
                    };

                    bank.Accounts.Add(account);
                }
            }
            return bank;
        }

        public void SaveToFile(List<Customer> customers, List<Account> accounts)
        {
            string pathDator = @"C:\Users\sebastian.TEMTRON\Source\Repos\Uppgift3\Uppgift3\";
            string pathLaptop = @"C:\Users\sebastian\source\repos\Uppgift3\Uppgift3\";
            DateTime dt = new DateTime();
            dt = DateTime.Now;
            string filename = dt.Year + "" + dt.Month + "" + dt.Day + "-" + dt.Hour + dt.Minute + ".txt";
            pathDator = Path.Combine(pathDator, filename);
            Console.WriteLine("Path to my file: " + pathDator);
            int customerLength = customers.Count();
            int accountsLength = accounts.Count();
            //using (FileStream fs = new FileStream(path, FileMode.Append))

            using (StreamWriter streamWriter = new StreamWriter(pathDator))
            {
                streamWriter.WriteLine(customerLength);
                foreach (var c in customers)
                {
                    //Console.WriteLine($"{c.CustomerNr};{c.OrgNr};{c.CompanyName};{c.Adress};{c.City};{c.Region};{c.PostNr};{c.Country};{c.PhoneNr}");
                    string test = $"{c.CustomerNr};{c.OrgNr};{c.CompanyName};{c.Adress};{c.City};{c.Region};{c.PostNr};{c.Country};{c.PhoneNr}";
                    streamWriter.WriteLine(test);
                    //Console.WriteLine(test);
                }
                streamWriter.WriteLine(accountsLength);
                foreach (var a in accounts)
                {
                    //Console.WriteLine($"{c.CustomerNr};{c.OrgNr};{c.CompanyName};{c.Adress};{c.City};{c.Region};{c.PostNr};{c.Country};{c.PhoneNr}");
                    string test = $"{a.AccountNr};{a.Owner};{a.Balance.ToString(CultureInfo.InvariantCulture)}";
                    streamWriter.WriteLine(test);
                    //Console.WriteLine(test);
                }
            }

        }

        private static string GetPath()
        {
            string path;
            string pathDator = @"C:\Users\sebastian.TEMTRON\Source\Repos\Uppgift3\Uppgift3\";
            string pathLaptop = @"C:\Users\sebastian\source\repos\Uppgift3\Uppgift3\";
            try
            {
                path = pathDator + Console.ReadLine();
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
