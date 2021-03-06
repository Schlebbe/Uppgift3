﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uppgift3
{
    public class Bank
    {
        public List<Customer> Customers { get; set; } = new List<Customer>();
        public List<Account> Accounts { get; set; } = new List<Account>();
        public int CurrentNumber { get; set; }

        public void SaveToFile()
        {
            var datahandler = new DataHandler();
            datahandler.SaveToFile(Customers, Accounts);
        }//0

        public void SearchCustomer(string query)
        {
            var customerQuery = (from c in Customers
                                 where c.CompanyName.ToUpper().Contains(query) || c.ZIPCode.Contains(query)
                                 select c).ToList();

            foreach (var c in customerQuery)
            {
                Console.Write(c.CustomerNumber + ": ");
                Console.WriteLine(c.CompanyName + " ");
            }
        }//1

        public void ShowInfo(string query)
        {
            var customerQuery = (from c in Customers
                                 where c.CustomerNumber == int.Parse(query)
                                 select c).ToList().FirstOrDefault();

            if (customerQuery != null)
            {
                PrintCustomerInfo(customerQuery, query);
            }
            else
            {
                var accountQuery = (from c in Customers
                                    from a in Accounts
                                    where a.AccountNumber == int.Parse(query) && a.Owner == c.CustomerNumber
                                    select c).ToList().FirstOrDefault();

                if (accountQuery != null)
                {
                    PrintCustomerInfo(accountQuery, accountQuery.CustomerNumber.ToString());

                }
                else { Console.WriteLine("Ej giltigt kund- eller kontonummer!"); }
            }
        }//2

        public void AddCustomer(string organisationNumber, string companyNumber, string adress, string city, string region, string zipCode, string country, string phoneNumber)
        {
            var customerQuery = (from c in Customers
                                 orderby c.CustomerNumber descending
                                 select c).ToList().FirstOrDefault();

            if (customerQuery != null) CurrentNumber = customerQuery.CustomerNumber;
            CurrentNumber++;

            Customer customer = new Customer()
            {
                CustomerNumber = CurrentNumber,
                OrganisationNumber = organisationNumber,
                CompanyName = companyNumber,
                Adress = adress,
                City = city,
                Region = region,
                ZIPCode = zipCode,
                Country = country,
                PhoneNumber = phoneNumber
            };
            Customers.Add(customer);
            AddAccount(CurrentNumber.ToString(), "0");
            Console.WriteLine("En kund har lagts till!");
        }//3

        public void RemoveCustomer(string query)
        {
            if (!int.TryParse(query, out int parsedQuery)) { Console.WriteLine("Kundnummer kan endast bestå av siffror!"); }
            else
            {
                var customerQuery = (from c in Customers
                                     where c.CustomerNumber == parsedQuery
                                     select c).ToList();

                var accountQuery = (from c in Customers
                                    from a in Accounts
                                    where a.Owner == c.CustomerNumber
                                    select a).ToList();

                bool validCustomer = false;
                bool haveAccount = false;
                foreach (var c in customerQuery)
                {
                    foreach (var a in accountQuery)
                    {
                        if (a.Owner == c.CustomerNumber)
                        {
                            haveAccount = true;
                            break;
                        }
                    }

                    if (!haveAccount)
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

                if (!validCustomer) { Console.WriteLine("\nSkriv in ett giltigt kundnummer!"); }
            }
        }//4

        public void AddAccount(string query, string interest)
        {
            var accountQuery = (from a in Accounts
                                orderby a.AccountNumber descending
                                select a).ToList().FirstOrDefault();

            if (accountQuery != null) CurrentNumber = accountQuery.AccountNumber;
            CurrentNumber++;

            var customerQuery = (from c in Customers
                                 where c.CustomerNumber == int.Parse(query)
                                 select c).ToList().FirstOrDefault();

            if (customerQuery != null)
            {
                Account account = new Account()
                {
                    AccountNumber = CurrentNumber,
                    Owner = customerQuery.CustomerNumber,
                    Balance = 0,
                    Interest = decimal.Parse(interest)
                };

                Accounts.Add(account);
                Console.WriteLine("\nEtt konto har lagts till för kundnummer: " + customerQuery.CustomerNumber + "!");
            }
            else { Console.WriteLine("\nSkriv in ett giltigt kundnummer!"); }
        }//5

        public void RemoveAccount(string query)
        {
            var accountQuery = (from a in Accounts
                                where a.AccountNumber == int.Parse(query)
                                select a).ToList();

            if (accountQuery.Count != 0)
            {
                foreach (var a in accountQuery)
                {
                    if (a.Balance == 0)
                    {
                        Accounts.RemoveAt(Accounts.IndexOf(a));
                        Console.WriteLine("\nKonto borttaget!");
                    }
                    else { Console.WriteLine("\nKontot måste vara tomt för att tas bort!"); }
                }
            }
            else { Console.WriteLine("\nSkriv in ett giltigt kontonummer!"); }
        }//6

        public void DepositMoney(string accountNumber, string amount)
        {
            var accountQuery = (from a in Accounts
                                where a.AccountNumber == int.Parse(accountNumber)
                                select a).ToList().FirstOrDefault();

            if (accountQuery != null)
            {
                if (decimal.Parse(amount) > 0)
                {
                    accountQuery.Balance += decimal.Parse(amount);
                    Console.WriteLine("\n" + amount.ToString(CultureInfo.InvariantCulture) + " kr har satts in på konto " + accountQuery.AccountNumber + "!");
                    var test = new Transaction()
                    {
                        Deposit = decimal.Parse(amount)

                    };
                    accountQuery.Transactions.Add(test);
                    SaveTransaction(accountQuery.Transactions.LastOrDefault(), accountQuery, null);
                }
                else { Console.WriteLine("\nEj giltigt belopp!"); }
            }
            else { Console.WriteLine("\nSkriv in ett giltigt kontonummer!"); }

        }//7

        public void WithdrawMoney(string accountNumber, string amount)
        {
            var accountQuery = (from a in Accounts
                                where a.AccountNumber == int.Parse(accountNumber)
                                select a).ToList().FirstOrDefault();

            if (accountQuery != null)
            {
                if (decimal.Parse(amount) > 0)
                {
                    if ((accountQuery.Balance - decimal.Parse(amount)) >= -accountQuery.Overdraft)
                    {
                        accountQuery.Balance -= decimal.Parse(amount);
                        Console.WriteLine("\n" + amount.ToString(CultureInfo.InvariantCulture) + " kr har tagits ut från konto " + accountQuery.AccountNumber + "!");
                        var test = new Transaction()
                        {
                            Withdraw = decimal.Parse(amount)

                        };
                        accountQuery.Transactions.Add(test);
                        SaveTransaction(accountQuery.Transactions.LastOrDefault(), accountQuery, null);

                    }
                    else { Console.WriteLine("\nFör lite saldo på kontot!"); }
                }
                else { Console.WriteLine("\nEj giltigt belopp!"); }
            }
            else { Console.WriteLine("\nSkriv in ett giltigt kontonummer!"); }
        }//8

        public void TransferMoney(string fromAccount, string toAccount, string amount)
        {
            var fromAccountQuery = (from a in Accounts
                                    where a.AccountNumber == int.Parse(fromAccount)
                                    select a).ToList().FirstOrDefault();

            var toAccountQuery = (from a in Accounts
                                  where a.AccountNumber == int.Parse(toAccount)
                                  select a).ToList().FirstOrDefault();

            if (fromAccountQuery != null && toAccountQuery != null)
            {
                if (decimal.Parse(amount) > 0)
                {
                    if (!(fromAccountQuery.AccountNumber == toAccountQuery.AccountNumber))
                    {
                        if (fromAccountQuery.Balance >= decimal.Parse(amount) - fromAccountQuery.Overdraft)
                        {
                            fromAccountQuery.Balance -= decimal.Parse(amount);
                            toAccountQuery.Balance += decimal.Parse(amount);
                            Console.WriteLine("\n" + amount.ToString(CultureInfo.InvariantCulture) + " kr har överförts från konto " + fromAccountQuery.AccountNumber + " till " + toAccountQuery.AccountNumber + "!");
                            var fromAccountTransaction = new Transaction()
                            {
                                Transfer = decimal.Parse(amount),
                                FromAccount = int.Parse(fromAccount),
                                ToAccount = int.Parse(toAccount)
                            };
                            var toAccountTransaction = new Transaction()
                            {
                                Transfer = decimal.Parse(amount),
                                FromAccount = int.Parse(toAccount),
                                ToAccount = int.Parse(fromAccount)
                            };
                            fromAccountQuery.Transactions.Add(fromAccountTransaction);
                            toAccountQuery.Transactions.Add(toAccountTransaction);
                            SaveTransaction(fromAccountQuery.Transactions.LastOrDefault(), fromAccountQuery, toAccountQuery);
                            SaveTransaction(fromAccountQuery.Transactions.LastOrDefault(), toAccountQuery, fromAccountQuery);
                        }
                        else { Console.WriteLine("Inte tillräckligt på kontot!"); }
                    }
                    else { Console.WriteLine("Du kan bara överföra mellan två olika konton!"); }
                }
                else { Console.WriteLine("\nEj giltigt belopp!"); }
            }
            else { Console.WriteLine("\nSkriv in ett giltigt kontonummer!"); }
        }//9

        public void ShowTransactions(string accountNumber)
        {
            var accountQuery = (from a in Accounts
                                where a.AccountNumber == int.Parse(accountNumber)
                                select a).ToList().FirstOrDefault();
            if (accountQuery != null)
            {
                if (accountQuery.Transactions.Count() > 0)
                {
                    foreach (var t in accountQuery.Transactions)
                    {
                        var dt = t.DateTime;
                        var currentDateTime = dt.ToString("yyyyMMdd") + "-" + dt.ToString("HHmmss");

                        if (t.Deposit > 0)
                        {
                            Console.WriteLine($"*Insättning* Datum: {currentDateTime} Konton: {accountQuery.AccountNumber} Belopp: {t.Deposit.ToString(CultureInfo.InvariantCulture)} Saldo: {accountQuery.Balance.ToString(CultureInfo.InvariantCulture)}");
                        }
                        else if (t.Withdraw > 0)
                        {
                            Console.WriteLine($"*Uttag* Datum: {currentDateTime} Konton: {accountQuery.AccountNumber} Belopp: {t.Withdraw.ToString(CultureInfo.InvariantCulture)} Saldo: {accountQuery.Balance.ToString(CultureInfo.InvariantCulture)}");
                        }
                        else if (t.Transfer > 0)
                        {
                            Console.WriteLine($"*Överföring* Datum: {currentDateTime} Konton: {t.FromAccount} och {t.ToAccount} Belopp: {t.Transfer.ToString(CultureInfo.InvariantCulture)} Saldo: {accountQuery.Balance.ToString(CultureInfo.InvariantCulture)}");
                        }
                        else if (t.Interest > 0)
                        {
                            Console.Write($"*Ränta* Datum: {currentDateTime} Konton: {accountQuery.AccountNumber} Belopp: {t.Interest.ToString(CultureInfo.InvariantCulture)} Saldo: {accountQuery.Balance.ToString(CultureInfo.InvariantCulture)}\n");
                        }
                        else if (t.DebtInterest < 0)
                        {
                            Console.Write($"*Skuldränta* Datum: {currentDateTime} Konton: {accountQuery.AccountNumber} Belopp: {t.DebtInterest.ToString(CultureInfo.InvariantCulture)} Saldo: {accountQuery.Balance.ToString(CultureInfo.InvariantCulture)}\n");
                        }

                    }
                }
                else { Console.WriteLine("\nFinns inga transaktioner!"); }
            }
            else { Console.WriteLine("\nSkriv in ett giltigt kontonummer!"); }
        }//10

        public void ChangeInterest(string accountNumber, string newInterest)
        {
            var accountQuery = (from a in Accounts
                                where a.AccountNumber == int.Parse(accountNumber)
                                select a).ToList().FirstOrDefault();

            if (accountQuery != null)
            {
                accountQuery.Interest = decimal.Parse(newInterest);
                Console.WriteLine($"\nRäntesatsen för {accountQuery.AccountNumber} har ändrats till {accountQuery.Interest}%");
            }
            else { Console.WriteLine("\nSkriv in ett giltigt kontonummer!"); }

        }//11

        public void CalculateInterest()
        {
            foreach (var a in Accounts)
            {
                if (a.Balance < 0)
                {
                    a.Balance += Math.Round(a.Balance * (a.DebtInterest / 100 / 365), 5);
                    var transaction = new Transaction()
                    {
                        DebtInterest = Math.Round(a.Balance * (a.DebtInterest / 100 / 365), 5)
                    };
                    a.Transactions.Add(transaction);
                    SaveTransaction(a.Transactions.LastOrDefault(), a, null);
                }
                else
                {
                    a.Balance += Math.Round(a.Balance * (a.Interest / 100 / 365), 5);
                    var transaction = new Transaction()
                    {
                        Interest = Math.Round(a.Balance * (a.Interest / 100 / 365), 5)
                    };
                    a.Transactions.Add(transaction);
                    SaveTransaction(a.Transactions.LastOrDefault(), a, null);
                }
            }
        }//12

        public void ChangeCredit(string account, string newCredit)
        {
            var accountQuery = (from a in Accounts
                                where a.AccountNumber == int.Parse(account)
                                select a).ToList().FirstOrDefault();

            if (accountQuery != null)
            {
                accountQuery.Overdraft = decimal.Parse(newCredit);
                Console.WriteLine($"\nKrediten har ändrats till {newCredit}");
            }
            else { Console.WriteLine("\nSkriv in ett giltigt kontonummer!"); }
        }//13

        public void ChangeDebtInterest(string account, string newDebtInterest)
        {
            var accountQuery = (from a in Accounts
                                where a.AccountNumber == int.Parse(account)
                                select a).ToList().FirstOrDefault();

            if (accountQuery != null)
            {
                accountQuery.DebtInterest = decimal.Parse(newDebtInterest);
                Console.WriteLine($"\nSkuldräntan har ändrats till {newDebtInterest}%");
            }
            else { Console.WriteLine("\nSkriv in ett giltigt kontonummer!"); }
        }//14

        private void PrintCustomerInfo(Customer custQuery, string query)
        {
            Console.WriteLine("\nKundnummer: " + custQuery.CustomerNumber);
            Console.WriteLine("Organisationsnummer: " + custQuery.OrganisationNumber);
            Console.WriteLine("Namn: " + custQuery.CompanyName);
            Console.WriteLine("Adress: " + custQuery.Adress);
            Console.WriteLine("Stad: " + custQuery.City);
            Console.WriteLine("Region: " + custQuery.Region);
            Console.WriteLine("Postnummer: " + custQuery.ZIPCode);
            Console.WriteLine("Land: " + custQuery.Country);
            Console.WriteLine("Telefonnummer: " + custQuery.PhoneNumber);

            var accountQuery = (from a in Accounts
                            from c in Customers
                            where a.Owner == c.CustomerNumber && c.CustomerNumber == int.Parse(query)
                            select a).ToList();

            decimal totalBalance = 0;
            if (accountQuery.Count > 0)
            {
                Console.WriteLine("\nKonton:");
                foreach (var a in accountQuery)
                {
                    Console.WriteLine("Konto " + a.AccountNumber + ": " + a.Balance.ToString(CultureInfo.InvariantCulture) + " kr");
                    totalBalance += a.Balance;
                }

                Console.WriteLine("Totalt saldo: " + totalBalance.ToString(CultureInfo.InvariantCulture) + " kr");
            }
        }//2

        private void SaveTransaction(Transaction transaction, Account account, Account toAccountQuery)
        {
            var datahandler = new DataHandler();
            datahandler.SaveTransaction(transaction, account, toAccountQuery);
        }//7 8 9
    }
}