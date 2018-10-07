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
            Console.ReadKey();
            //Console.WriteLine(bank.Customers[0].CustomerNr);
        }
    }
}
