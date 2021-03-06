﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uppgift3
{
    public class Account
    {
        public int AccountNumber { get; set; }
        public int Owner { get; set; }
        public decimal Balance { get; set; }
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
        public decimal Interest { get; set; }
        public decimal Overdraft { get; set; }
        public decimal DebtInterest { get; set; }
    }
}
