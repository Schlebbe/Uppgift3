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
        public string fileName = "bankdata.txt";

        public Bank ReadFromFile()
        {
            string path = GetPath();
            Console.WriteLine("\n******************************");
            Console.WriteLine("* VÄLKOMMEN TILL BANKAPP 1.0 *");
            Console.WriteLine("******************************\n");
            Console.WriteLine("Läser in " + fileName + "...");
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
                        ZIPCode = cols[6],
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
            //string pathDator = @"C:\Users\sebastian.TEMTRON\Source\Repos\Uppgift3\Uppgift3\";
            //string pathLaptop = @"C:\Users\sebastian\source\repos\Uppgift3\Uppgift3\";
            string defaultPath = @"..\..\Data\";
            DateTime dt = DateTime.Now;
            string filename = dt.ToString("yyyyMMdd") + "-" + dt.ToString("HHmmss") + ".txt";
            defaultPath = Path.Combine(defaultPath, filename);
            //Console.WriteLine("Path to my file: " + pathDator);
            int countOfCustomers = customers.Count();
            int countOfAccounts = accounts.Count();
            decimal totaltBalance = 0;
            //using (FileStream fs = new FileStream(path, FileMode.Append))

            using (StreamWriter streamWriter = new StreamWriter(defaultPath))
            {
                streamWriter.WriteLine(countOfCustomers);
                foreach (var c in customers)
                {
                    string line = $"{c.CustomerNumber};{c.OrganisationNumber};{c.CompanyName};{c.Adress};{c.City};{c.Region};{c.ZIPCode};{c.Country};{c.PhoneNumber}";
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

        public void SaveTransaction(Transaction transaction, Account account1, Account account2)
        {
            //string pathDator = @"C:\Users\sebastian.TEMTRON\Source\Repos\Uppgift3\Uppgift3\";
            //string pathLaptop = @"C:\Users\sebastian\source\repos\Uppgift3\Uppgift3\";
            string defaultPath = @"..\..\Data\";
            DateTime dt = transaction.DateTime;
            string filename = "transaktionslogg.txt";
            defaultPath = Path.Combine(defaultPath, filename);

            using (StreamWriter streamWriter = new StreamWriter(defaultPath, true))
            {
                if (transaction.Deposit > 0)
                {
                    streamWriter.Write($"*Insättning* Datum: {dt.Year}{dt.Month}{dt.Day}-{dt.Hour}{dt.Minute} Konton: {account1.AccountNumber} Belopp: {transaction.Deposit.ToString(CultureInfo.InvariantCulture)} Saldo: {account1.Balance.ToString(CultureInfo.InvariantCulture)}\n");
                }
                else if (transaction.Withdraw > 0)
                {
                    streamWriter.Write($"*Uttag* Datum: {dt.Year}{dt.Month}{dt.Day}-{dt.Hour}{dt.Minute} Konton: {account1.AccountNumber} Belopp: {transaction.Withdraw.ToString(CultureInfo.InvariantCulture)} Saldo: {account1.Balance.ToString(CultureInfo.InvariantCulture)}\n");
                }
                else if (transaction.Transfer > 0)
                {
                    streamWriter.Write($"*Överföring* Datum: {dt.Year}{dt.Month}{dt.Day}-{dt.Hour}{dt.Minute} Konton: {account1.AccountNumber} och {account2.AccountNumber} Belopp: {transaction.Transfer.ToString(CultureInfo.InvariantCulture)} Saldo: {account1.Balance.ToString(CultureInfo.InvariantCulture)}\n");
                }
                else if (transaction.Interest > 0)
                {
                    streamWriter.Write($"*Ränta* Datum: {dt.Year}{dt.Month}{dt.Day}-{dt.Hour}{dt.Minute} Konton: {account1.AccountNumber} Belopp: {transaction.Interest.ToString(CultureInfo.InvariantCulture)} Saldo: {account1.Balance.ToString(CultureInfo.InvariantCulture)}\n");
                }
                else if (transaction.DebtInterest < 0)
                {
                    streamWriter.Write($"*Skuldränta* Datum: {dt.Year}{dt.Month}{dt.Day}-{dt.Hour}{dt.Minute} Konton: {account1.AccountNumber} Belopp: {transaction.DebtInterest.ToString(CultureInfo.InvariantCulture)} Saldo: {account1.Balance.ToString(CultureInfo.InvariantCulture)}\n");
                }
            }
        }

        public string GetPath()
        {
            Console.Write($"Ange fil att läsa ifrån ({fileName}): ");
            string path;
            string defaultPath = @"..\..\Data\";
            try
            {
                string fileQuery = Console.ReadLine();
                if (fileQuery != string.Empty)
                {
                    fileName = fileQuery;
                }
                path = defaultPath + fileName;
                StreamReader testStream = new StreamReader(path);
            }
            catch
            {
                Console.Clear();
                Console.WriteLine("Felaktigt filnamn, försök igen!");
                fileName = "bankdata.txt";
                path = GetPath();
            }

            return path;
        }
    }
}
