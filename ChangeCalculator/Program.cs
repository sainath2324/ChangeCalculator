using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Globalization;

namespace ChangeCalculator
{
    internal class Program
    {
        //Global Variables
        static double itemPrice = 0;
        static string country = string.Empty;
        static string ieft = string.Empty;
        static List<double> denominations = new List<double>();
        static string currency = Constants.CURRENCY;
        
        static void Main(string[] args)
        {
            //Local Variables
            //To Store Coins entered by user in Deonmination:Quantity Format
            string coins = string.Empty;
            //To Store User Amount based on the coins Entered
            double userAmount = 0;
            //To store difference amount based on Item Price and User Provided Amount
            double difference = 0;
            
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();

            //Setting Country and Denomination based on Configuration
            country = config.GetSection("countryName:country").Value;
            ieft = config.GetSection($"IEFTTags:{country}").Value;
            //To get Region Info details like Currency Symbol and display to user
            RegionInfo regInfo = new RegionInfo(country);
            Console.WriteLine($"Curreny - {regInfo.ISOCurrencySymbol} {regInfo.CurrencyEnglishName}");
            Thread.CurrentThread.CurrentCulture = new CultureInfo(ieft);
            denominations = config.GetSection($"denominations:{country}").Get<List<double>>();
            denominations = denominations.OrderByDescending(denomination => denomination).ToList();
            itemPrice = getItemPrice();

            //Create an Object and Set Item Price and Denominations Properties in Money Processor Call
            var moneyProcessor = new MoneyProcessor(itemPrice, denominations);
                       
            Console.WriteLine(Constants.ENTERDENOMINATIONS);
            //Loop to check if user is entering Amount Greater than or equal to Item Price
            while (moneyProcessor.moneyFromCustomer.Sum(money => money.Amount) < itemPrice)
            {
                coins = Console.ReadLine();
                var (result, denom, quantity) = moneyProcessor.AmountCollectedFromCustomer(itemPrice, coins);
                if (result != string.Empty) { Console.WriteLine(result); continue; }

                userAmount = moneyProcessor.moneyFromCustomer.Sum(money => money.Amount);

                difference = Math.Round(itemPrice - userAmount, 2);
                if (difference > 0)
                    Console.WriteLine($"{Constants.AMOUNTENTERED} {denom} * {quantity} = {(denom * quantity).ToString(currency)} {Constants.BALANCE} {difference.ToString(currency)}");
                else if (difference < 0)
                    Console.WriteLine($"{Constants.AMOUNTENTERED} {denom} * {quantity} = {(denom * quantity).ToString(currency)} {Constants.RETURN} {Math.Abs(difference).ToString(currency)}");
            }
            Console.WriteLine(Constants.BILLSPROVIDED);
            print(moneyProcessor.moneyFromCustomer);
            Console.WriteLine($"{Constants.ITEMPRICEIS} {itemPrice.ToString(currency)}");
            Console.WriteLine($"{Constants.TOTALAMOUNTPAIDBYCUSTOMER} {userAmount.ToString(currency)}");
            var output = moneyProcessor.ReturnBillsAndCoinsToCustomer();
            Console.WriteLine($"{Constants.PAYBACKAMOUNT} {Math.Round(userAmount - itemPrice, 2).ToString(currency)}");
            print(moneyProcessor.balanceToCustomer);           
            Console.ReadLine();
        }
        //Print the User Input and Return to Customer List Values in Deonmiation : Quantity = Value Format
        public static void print(List<Money> monies)
        {
            foreach (var money in monies)
            {
                Console.WriteLine(money.formatMoney());
            }
        }
        //Retreive Item Price based on user Input from console
        public static double getItemPrice()
        {
            Console.WriteLine(Constants.ITEMPRICE);
            while (itemPrice == 0 || itemPrice < denominations.Min()) { double.TryParse(Console.ReadLine(), out itemPrice); }
            Console.WriteLine($"{Constants.ITEMPRICEIS} {itemPrice.ToString("C")}");
            return itemPrice;
        }
    }
}
