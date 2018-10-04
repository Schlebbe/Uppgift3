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

        public Bank()
        {

        }

        public void AddCustomer(string orgNr, string compNr, string adress, string city, string region, string postNr, string country, string phoneNr)
        {
            var custQuery = (from c in Customers
                             orderby c.CustomerNr descending
                             select c).ToList().FirstOrDefault();
            //Console.WriteLine(test.Max());
            //Console.WriteLine("högsta är: "+test.CustomerNr);

            //var max = 0;
            //foreach (var c in Customers)
            //{
            //    Console.WriteLine(c.CustomerNr);
            //    if (c.CustomerNr > max)
            //    {
            //        //c.CustomerNr = max;
            //        max = c.CustomerNr;
            //    }

            //}
            //Console.WriteLine(max);
            if (custQuery != null) CurrentNr = custQuery.CustomerNr;
            CurrentNr++;


            //Bank.AddCustomer(OrgNr, compNr, adress, region, postNr, phoneNr);
            Customer customer = new Customer()
            {
                CustomerNr = CurrentNr,
                OrgNr = orgNr,
                CompanyName = compNr,
                Adress = adress,
                City = city,
                Region = region,
                PostNr = postNr,
                Country = country,
                PhoneNr = phoneNr
            };
            Customers.Add(customer);
        }

        public void RemoveCustomer(string query)
        {
            //var test = (from a in Accounts
            //            select a).ToList();

            //foreach (var a in test)
            //{
            //    Console.WriteLine(a.Balance);
            //}

            ;
            if (!int.TryParse(query, out int parsedQuery))
            {
                Console.WriteLine("Kundnummer kan endast bestå av siffror.");
            }

            var custQuery = (from c in Customers
                             where c.CustomerNr == parsedQuery
                             select c).ToList();

            var accQuery = (from c in Customers
                            from a in Accounts
                            where a.Owner == c.CustomerNr
                            select a).ToList();

            bool validCustomer = false;
            bool haveAcc = false;
            foreach (var c in custQuery)
            {
                //Console.WriteLine("kördes");
                //Console.WriteLine(c.CustomerNr);
                foreach (var a in accQuery)
                {
                    //Console.WriteLine(a.Owner);
                    //Console.WriteLine(c.CustomerNr);
                    if (a.Owner == c.CustomerNr)
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
                else
                {
                    Console.WriteLine("Ta bort konton först!");
                }

                if (c.CustomerNr == parsedQuery)
                {
                    validCustomer = true;
                }
            }

            if (!validCustomer)
            {
                Console.WriteLine("Skriv in ett giltigt kundnummer!");
            }
            //if (haveAcc)
            //{
            //}
        }

        public void ShowInfo(string query)
        {
            var custQuery = (from c in Customers
                             where c.CustomerNr == int.Parse(query)
                             select c).ToList().FirstOrDefault();

            //foreach (var c in custQuery)
            //{

            if (custQuery != null)
            {
                Console.WriteLine();
                Console.WriteLine("Kundnummer: " + custQuery.CustomerNr);
                Console.WriteLine("Organisationsnummer: " + custQuery.OrgNr);
                Console.WriteLine("Namn: " + custQuery.CompanyName);
                Console.WriteLine("Adress: " + custQuery.Adress); //Kanske fler? Typ zip code
                Console.WriteLine("City: " + custQuery.City);
                Console.WriteLine("Region: " + custQuery.Region);
                Console.WriteLine("Postnummer: " + custQuery.PostNr);
                Console.WriteLine("Land: " + custQuery.Country);
                Console.WriteLine("Telefonnummer: " + custQuery.PhoneNr);
            }
            else
            {
                Console.WriteLine("Finns ingen kund med detta kundnummer!");
            }


            //}

            var accQuery = (from a in Accounts
                            from c in Customers
                            where a.Owner == c.CustomerNr && c.CustomerNr == int.Parse(query)
                            select a).ToList();

            decimal totalBalance = 0;
            if (accQuery.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Konton:");
                foreach (var a in accQuery)
                {
                    Console.WriteLine("Konto " + a.AccountNr + ": " + a.Balance.ToString(CultureInfo.InvariantCulture) + " kr");
                    totalBalance += a.Balance;
                }

                Console.WriteLine("Totalt saldo: " + totalBalance.ToString(CultureInfo.InvariantCulture)+" kr");
            }
        }

        public void SearchCustomer(string query)
        {
            var custQuery = (from c in Customers
                             where c.CompanyName.ToUpper().Contains(query) || c.PostNr.Contains(query)
                             select c).ToList();

            foreach (var i in custQuery)
            {
                Console.Write(i.CustomerNr + ": ");
                Console.WriteLine(i.CompanyName + " ");
            }
        }

        public void AddAccount(string query)
        {
            var accQuery = (from a in Accounts
                            orderby a.AccountNr descending
                            select a).ToList().FirstOrDefault();

            if (accQuery != null) CurrentNr = accQuery.AccountNr;
            CurrentNr++;

            var custQuery = (from c in Customers
                             where c.CustomerNr == int.Parse(query)
                             select c).ToList().FirstOrDefault();

            if (custQuery != null)
            {
                Account account = new Account()
                {
                    AccountNr = CurrentNr,
                    Owner = custQuery.CustomerNr,
                    Balance = 0
                };

                Accounts.Add(account);
                Console.WriteLine("Ett konto har lagts till för kundnummer: " + custQuery.CustomerNr);
            }
            else
            {
                Console.WriteLine("Skriv in ett giltigt kundnummer!");
            }
        }

        public void SaveToFile()
        {
            var datahandler = new DataHandler();
            datahandler.SaveToFile(Customers, Accounts);
        }
    }
}
