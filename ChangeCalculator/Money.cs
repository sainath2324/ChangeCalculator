using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeCalculator
{
    //Money Class to have Same Datastruture for Amount Entered and Amount to be Returned to customer
    public class Money
    {
        public double Denomination { get; }
        public int Quantity { get; }

        public Money(double denomination, int quantity)
        {
            Denomination = denomination;
            Quantity = quantity;
        }

        //To get User Entered Amount based on Denomination and Quantity
        public double Amount =>
            Denomination * Quantity;
        //To display Currency Format
        public override string ToString()
        {
            return Amount.ToString("C");
        }
        //To Format the Money and display to user in Denomination : Quantity = Amount
        public string formatMoney() {
            return $"{Denomination} {Constants.COINSSEPERATOR} {Quantity} = {ToString()}";
        }

    
    }
}
