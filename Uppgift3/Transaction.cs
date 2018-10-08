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
        public decimal Transfer { get; set; } //kanske som en lista som innehåller vilka konton och belopp?
        public DateTime DateTime { get; set; } = DateTime.Now;
        public int FromAccount { get; set; }
        public int ToAccount { get; set; }
    }
}
