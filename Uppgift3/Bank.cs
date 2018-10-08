using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uppgift3
{
    class Bank
    {
        public List<Customer> Customers { get; set; } = new List<Customer>();
        public List<Account> Accounts { get; set; } = new List<Account>();
        public int CurrentNr { get; set; }

        public void AddCustomer(string organisationNumber, string companyNumber, string adress, string city, string region, string postNumber, string country, string phoneNumber)
        {
            var custQuery = (from c in Customers
                             orderby c.CustomerNumber descending
                             select c).ToList().FirstOrDefault();

            if (custQuery != null) CurrentNr = custQuery.CustomerNumber;
            CurrentNr++;

            Customer customer = new Customer()
            {
                CustomerNumber = CurrentNr,
                OrganisationNumber = organisationNumber,
                CompanyName = companyNumber,
                Adress = adress,
                City = city,
                Region = region,
                PostNumber = postNumber,
                Country = country,
                PhoneNumber = phoneNumber
            };
            Customers.Add(customer);
            AddAccount(CurrentNr.ToString());
            Console.WriteLine("En kund har lagts till!");
        }

        public void RemoveCustomer(string query)
        {
            if (!int.TryParse(query, out int parsedQuery)) { Console.WriteLine("Kundnummer kan endast bestå av siffror."); }
            else
            {
                var custQuery = (from c in Customers
                                 where c.CustomerNumber == parsedQuery
                                 select c).ToList();

                var accQuery = (from c in Customers
                                from a in Accounts
                                where a.Owner == c.CustomerNumber
                                select a).ToList();

                bool validCustomer = false;
                bool haveAcc = false;
                foreach (var c in custQuery)
                {
                    foreach (var a in accQuery)
                    {
                        if (a.Owner == c.CustomerNumber)
                        {
                            haveAcc = true;
                            break;
                        }
                    }

                    if (!haveAcc)
                    {
                        Customers.RemoveAt(Customers.IndexOf(c));
                        Console.WriteLine("Kund borttagen!");
                    }
                    else { Console.WriteLine("Ta bort konton först!"); }

                    if (c.CustomerNumber == parsedQuery)
                    {
                        validCustomer = true;
                    }
                }

                if (!validCustomer) { Console.WriteLine("Skriv in ett giltigt kundnummer!"); }
            }
        }

        public void WithdrawMoney(string accountNumber, string amount)
        {
            var accountQuery = (from a in Accounts
                                where a.AccountNumber == int.Parse(accountNumber)
                                select a).ToList().FirstOrDefault();

            if (decimal.Parse(amount) > 0)
            {
                if ((accountQuery.Balance - decimal.Parse(amount)) >= 0)
                {
                    accountQuery.Balance -= decimal.Parse(amount);
                    Console.WriteLine(amount.ToString(CultureInfo.InvariantCulture) + " kr har tagits ut från konto " + accountQuery.AccountNumber + ".");
                    var test = new Transaction()
                    {
                        //Deposit = decimal.Parse(amount)
                        Withdraw = decimal.Parse(amount)
                        //Transfer = decimal.Parse(amount)

                    };
                    //Console.WriteLine(accountQuery.Transactions.Count());
                    accountQuery.Transactions.Add(test);
                    SaveTransaction(accountQuery.Transactions.LastOrDefault(), accountQuery, null);
                    //Console.WriteLine(accountQuery.Transactions[0].Deposit);

                }
                else { Console.WriteLine("För lite saldo på kontot!"); }
            }
            else { Console.WriteLine("Ej giltigt belopp!"); }
        }

        public void DepositMoney(string accountNumber, string amount)
        {
            var accountQuery = (from a in Accounts
                                where a.AccountNumber == int.Parse(accountNumber)
                                select a).ToList().FirstOrDefault();

            if (decimal.Parse(amount) > 0)
            {
                accountQuery.Balance += decimal.Parse(amount);
                Console.WriteLine(amount.ToString(CultureInfo.InvariantCulture) + " kr har satts in på konto " + accountQuery.AccountNumber + ".");
                var test = new Transaction()
                {
                    Deposit = decimal.Parse(amount)
                    //Withdraw = decimal.Parse(amount)
                    //Transfer = decimal.Parse(amount)

                };
                //Console.WriteLine(accountQuery.Transactions.Count());
                accountQuery.Transactions.Add(test);
                SaveTransaction(accountQuery.Transactions.LastOrDefault(), accountQuery, null);
                //Console.WriteLine(accountQuery.Transactions[0].Deposit);
            }
            else { Console.WriteLine("Ej giltigt belopp!"); }
        }

        public void TransferMoney(string fromAccount, string toAccount, string amount)
        {
            var fromAccountQuery = (from a in Accounts
                                    where a.AccountNumber == int.Parse(fromAccount)
                                    select a).ToList().FirstOrDefault();
            var toAccountQuery = (from a in Accounts
                                  where a.AccountNumber == int.Parse(toAccount)
                                  select a).ToList().FirstOrDefault();

            if (decimal.Parse(amount) > 0)
            {
                if (!(fromAccountQuery.AccountNumber == toAccountQuery.AccountNumber))
                {
                    if (fromAccountQuery.Balance >= decimal.Parse(amount))
                    {
                        fromAccountQuery.Balance -= decimal.Parse(amount);
                        toAccountQuery.Balance += decimal.Parse(amount);
                        Console.WriteLine(amount.ToString(CultureInfo.InvariantCulture) + " kr har överförts från konto " + fromAccountQuery.AccountNumber + " till " + toAccountQuery.AccountNumber + ".");
                        var transaction = new Transaction()
                        {
                            //Deposit = decimal.Parse(amount)
                            //Withdraw = decimal.Parse(amount)
                            Transfer = decimal.Parse(amount)

                        };                        
                        //Console.WriteLine(accountQuery.Transactions.Count());
                        fromAccountQuery.Transactions.Add(transaction);
                        SaveTransaction(fromAccountQuery.Transactions.LastOrDefault(), fromAccountQuery, toAccountQuery);
                        SaveTransaction(fromAccountQuery.Transactions.LastOrDefault(), toAccountQuery, fromAccountQuery);
                        //Console.WriteLine(accountQuery.Transactions[0].Deposit);
                    }
                    else { Console.WriteLine("Inte tillräckligt på kontot!"); }
                }
                else { Console.WriteLine("Du kan bara överföra mellan två olika konton."); }
            }
            else { Console.WriteLine("Ej giltigt belopp!"); }
        }

        public void RemoveAccount(string query)
        {
            var accQuery = (from a in Accounts
                            where a.AccountNumber == int.Parse(query)
                            select a).ToList();

            if (accQuery.Count != 0)
            {
                foreach (var a in accQuery)
                {
                    if (a.Balance == 0)
                    {
                        Accounts.RemoveAt(Accounts.IndexOf(a));
                        Console.WriteLine("Konto borttaget!");
                    }
                    else { Console.WriteLine("Kontot måste vara tomt för att tas bort!"); }
                }
            }
            else { Console.WriteLine("Skriv in ett giltigt kontonummer!"); }
        }

        public void ShowInfo(string query)//Kaos på variablernamn
        {
            var custQuery = (from c in Customers
                             where c.CustomerNumber == int.Parse(query)
                             select c).ToList().FirstOrDefault();

            if (custQuery != null)
            {
                PrintCustomerInfo(custQuery, query);
            }
            else
            {
                var custQuery2 = (from c in Customers
                                  from a in Accounts
                                  where a.AccountNumber == int.Parse(query) && a.Owner == c.CustomerNumber
                                  select c).ToList().FirstOrDefault();

                if (custQuery2 != null)
                {
                    PrintCustomerInfo(custQuery2, custQuery2.CustomerNumber.ToString());

                }
                else { Console.WriteLine("Ej giltigt kund- eller kontonummer!"); }
            }
        }

        private void PrintCustomerInfo(Customer custQuery, string query)
        {
            Console.WriteLine("\nKundnummer: " + custQuery.CustomerNumber);
            Console.WriteLine("Organisationsnummer: " + custQuery.OrganisationNumber);
            Console.WriteLine("Namn: " + custQuery.CompanyName);
            Console.WriteLine("Adress: " + custQuery.Adress); //Kanske fler? Typ zip code
            Console.WriteLine("Stad: " + custQuery.City);
            Console.WriteLine("Region: " + custQuery.Region);
            Console.WriteLine("Postnummer: " + custQuery.PostNumber);
            Console.WriteLine("Land: " + custQuery.Country);
            Console.WriteLine("Telefonnummer: " + custQuery.PhoneNumber);

            var accQuery = (from a in Accounts
                            from c in Customers
                            where a.Owner == c.CustomerNumber && c.CustomerNumber == int.Parse(query)
                            select a).ToList();

            decimal totalBalance = 0;
            if (accQuery.Count > 0)
            {
                Console.WriteLine("\nKonton:");
                foreach (var a in accQuery)
                {
                    Console.WriteLine("Konto " + a.AccountNumber + ": " + a.Balance.ToString(CultureInfo.InvariantCulture) + " kr");
                    totalBalance += a.Balance;
                }

                Console.WriteLine("Totalt saldo: " + totalBalance.ToString(CultureInfo.InvariantCulture) + " kr");
            }
        }

        public void SearchCustomer(string query)
        {
            var custQuery = (from c in Customers
                             where c.CompanyName.ToUpper().Contains(query) || c.PostNumber.Contains(query)
                             select c).ToList();

            foreach (var i in custQuery)
            {
                Console.Write(/*"Kundnummer "+*/i.CustomerNumber + ": ");
                Console.WriteLine(i.CompanyName + " ");
            }
        }

        public void AddAccount(string query)
        {
            var accQuery = (from a in Accounts
                            orderby a.AccountNumber descending
                            select a).ToList().FirstOrDefault();

            if (accQuery != null) CurrentNr = accQuery.AccountNumber;
            CurrentNr++;

            var custQuery = (from c in Customers
                             where c.CustomerNumber == int.Parse(query)
                             select c).ToList().FirstOrDefault();

            if (custQuery != null)
            {
                Account account = new Account()
                {
                    AccountNumber = CurrentNr,
                    Owner = custQuery.CustomerNumber,
                    Balance = 0
                };

                Accounts.Add(account);
                Console.WriteLine("Ett konto har lagts till för kundnummer: " + custQuery.CustomerNumber);
            }
            else { Console.WriteLine("Skriv in ett giltigt kundnummer!"); }
        }

        private void SaveTransaction(Transaction transaction, Account account, Account toAccountQuery)
        {
            var datahandler = new DataHandler();
            datahandler.SaveTransaction(transaction, account, toAccountQuery);
        }

        public void SaveToFile()
        {
            var datahandler = new DataHandler();
            datahandler.SaveToFile(Customers, Accounts);
        }
    }
}

#region Kopia av ShowInfo
//public void ShowInfo(string query)
//{
//    var custQuery = (from c in Customers
//                     where c.CustomerNr == int.Parse(query)
//                     select c).ToList().FirstOrDefault();

//    //foreach (var c in custQuery)
//    //{

//    if (custQuery != null)
//    {
//        Console.WriteLine();
//        Console.WriteLine("Kundnummer: " + custQuery.CustomerNr);
//        Console.WriteLine("Organisationsnummer: " + custQuery.OrgNr);
//        Console.WriteLine("Namn: " + custQuery.CompanyName);
//        Console.WriteLine("Adress: " + custQuery.Adress); //Kanske fler? Typ zip code
//        Console.WriteLine("Stad: " + custQuery.City);
//        Console.WriteLine("Region: " + custQuery.Region);
//        Console.WriteLine("Postnummer: " + custQuery.PostNr);
//        Console.WriteLine("Land: " + custQuery.Country);
//        Console.WriteLine("Telefonnummer: " + custQuery.PhoneNr);
//    }
//    else
//    {
//        Console.WriteLine("Finns ingen kund med detta kundnummer!");
//    }


//    //}

//    var accQuery = (from a in Accounts
//                    from c in Customers
//                    where a.Owner == c.CustomerNr && c.CustomerNr == int.Parse(query)
//                    select a).ToList();

//    decimal totalBalance = 0;
//    if (accQuery.Count > 0)
//    {
//        Console.WriteLine("\nKonton:");
//        foreach (var a in accQuery)
//        {
//            Console.WriteLine("Konto " + a.AccountNr + ": " + a.Balance.ToString(CultureInfo.InvariantCulture) + " kr");
//            totalBalance += a.Balance;
//        }

//        Console.WriteLine("Totalt saldo: " + totalBalance.ToString(CultureInfo.InvariantCulture) + " kr");
//    }
//}
#endregion