using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ChangeCalculator
{
    //Class to store all Constant Values to display messages to user
    public static class Constants
    {
        public const string ITEMPRICE = "Please enter the price of the Item";
        public const string ITEMPRICEIS = "The Price of the Item is: ";
        public const string ENTERDENOMINATIONS = "Kinldy enter the customer provided denominations in Denomination:Quantity format";
        public const string ENTEREDCURRENCYFORMATERROR = "Please enter amount in correct format";
        public const string ERRORDENOMINATION = "Please enter proper denomination";
        public const string ERRORQUANTITY = "Please enter proper quantity for denomiation";
        public const string AMOUNTENTERED = "The amount entered is:";
        public const string BALANCE = "and the balance to be entered is";
        public const string RETURN = "and amount to be returned to customer is";
        public const string BILLSPROVIDED = "The bills/coins provided by customer are as below:";
        public const string THANKYOU = "Thank you for the purchase. Have a nice day.....";
        public const string PAYBACKAMOUNT = "The Amount to be paid back to customer is:";
        public const string TOTALAMOUNTPAIDBYCUSTOMER = "The total amount paid by customer is: ";
        public const string ENTERCORRECTPRICE = "Please enter correct price of the item";
        public const string INVALIDINPUT = "Invalid Input";
        public const string EMPTYDENOM = "Denomination required";
        public const string YES = "Y";
        public const string NO = "N";
        public const string CURRENCY = "C";
        public const char COINSSEPERATOR = ':';
        public const int COINSLENGTH = 2;
    }
}
