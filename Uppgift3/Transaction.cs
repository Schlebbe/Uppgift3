using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uppgift3
{
    class Transaction
    {
        public decimal Deposit { get; set; }
        public decimal Withdraw { get; set; }
        public decimal Transfer { get; set; }
        public decimal Interest { get; set; }
        public decimal DebtInterest { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public int FromAccount { get; set; }
        public int ToAccount { get; set; }
    }
}
