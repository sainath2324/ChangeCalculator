using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ChangeCalculator
{
    public class MoneyProcessor
    {
        public double ItemPrice { get; private set; }
        public List<double> Denominations { get; }
        public List<Money> moneyFromCustomer { get; private set; } = new List<Money>();
        public List<Money> balanceToCustomer { get; private set; } = new List<Money>();

        public MoneyProcessor(double itemPrice, List<double> denominations)
        {
            if (denominations.Count == 0) { throw new ArgumentException(Constants.EMPTYDENOM,nameof(denominations)); }
            ItemPrice = itemPrice;
            Denominations = denominations;
        }

        //To add amount received from customer to a List
        public (string,double, int) AmountCollectedFromCustomer(double itemPrice, string coins)
        {
            double denom=0;
            int quantity=0;
            string[] customerCoins;
            //Validate if user entered correct coins or not
            if (string.IsNullOrWhiteSpace(coins)) return (Constants.INVALIDINPUT,0,0);
            //Validate if user entered coins have Coin Seperator
            customerCoins = coins.Split(Constants.COINSSEPERATOR);
            if (!coins.Contains(Constants.COINSSEPERATOR) || customerCoins.Length != Constants.COINSLENGTH) { return (Constants.ENTEREDCURRENCYFORMATERROR,0,0); }
            //Validate if user entered coins have correct Denomination
            var success = double.TryParse(customerCoins[0], out denom);
            if (!success) return (Constants.ERRORDENOMINATION, 0, 0);
            //try { denom = double.Parse(customerCoins[0]); }
            //catch (Exception) { return (Constants.ERRORDENOMINATION,0,0); }
            //Validate if user entered coins have correct Quantity
            try { quantity = int.Parse(customerCoins[1]); }
            catch (Exception) { return (Constants.ERRORQUANTITY,0, 0); }
            //Validate if user entered coins have correct Denomination
            if (!Denominations.Contains(denom)) return (Constants.ERRORDENOMINATION, denom, quantity);

            moneyFromCustomer.Add(new Money(denom,quantity));
            return (string.Empty,denom,quantity);  
        }
        //Calculate Return Amount to customer in add Money to a List to display to user
        public string ReturnBillsAndCoinsToCustomer()
        {
            double userAmount = AmountCollected();
            //double userAmount = moneyFromCustomer.Sum(money=> money.Amount);
            int[] billReturn = new int[DenomiationCount()];
            if (userAmount == ItemPrice) return Constants.THANKYOU;

            double returnAmount = ReturnAmountToCustomer();
            //double returnAmount = Math.Round(userAmount - ItemPrice, 2);
            //Main Logic to check for the least number of bills/coins to return to customer
            //Looping through the denominations in descending order to get highest denomination first
            foreach (var denomination in Denominations)
            {
                if (returnAmount >= denomination)
                {
                    //Quantity for particular denomination
                    var quantity = Convert.ToInt32(Math.Truncate(returnAmount / denomination)); 
                    //Add Denominaation and Quantity to Money List
                    balanceToCustomer.Add(new Money(denomination, quantity));
                    //Get the remaining return amount and check through remaining denominations
                    returnAmount = Math.Round(returnAmount % denomination, 2);
                }
            }
            return Math.Round(userAmount - ItemPrice, 2).ToString();
        }

        //Return Amount Collected from Customer using Money List
        public double AmountCollected()
        {
            return moneyFromCustomer.Sum(money => money.Amount);
        }
        //Return Denomination Count
        public int DenomiationCount()
        {
            return Denominations.Count;
        }
        //Return the Amount to be returned to customer
        public double ReturnAmountToCustomer()
        {
            return Math.Round(AmountCollected() - ItemPrice, 2);
        }
    }
}
