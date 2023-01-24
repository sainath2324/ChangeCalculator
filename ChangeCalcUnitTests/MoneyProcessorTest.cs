using ChangeCalculator;
using FluentAssertions;
using Microsoft.Extensions.Configuration;

namespace ChangeCalulatorTest
{
    
    //Test class to handle unit test cases for Money Processor Class
    public class MoneyProcessorTest
    {
        //Test Cases for Amount Collected from user Function
        //Validate if the coin seperator is : or not
        [Fact]
        public void AmountCollectedReturnIncorrectCoinSeperator()
        {
            var moneyProcessor = new MoneyProcessor(100, new List<double> { 100, 50 });

            var (result, denom, quantity) = moneyProcessor.AmountCollectedFromCustomer(100,"100*2");

            result.Should().Be(Constants.ENTEREDCURRENCYFORMATERROR);
            denom.Should().Be(0);
            quantity.Should().Be(0);
        }
        //Validate if the Coins Inut by customer are in required format or not
        [Fact]
        public void AmountCollectedReturnInvalidCoinInput()
        {
            var moneyProcessor = new MoneyProcessor(100, new List<double> { 100, 50 });

            var (result, denom, quantity) = moneyProcessor.AmountCollectedFromCustomer(100, "100");

            result.Should().Be(Constants.ENTEREDCURRENCYFORMATERROR);
            denom.Should().Be(0);
            quantity.Should().Be(0);
        }
        //Validate if the denomination entered by user can be parsed to double or not
        [Fact]
        public void AmountCollectedReturnInvalidDenomiation()
        {
            var moneyProcessor = new MoneyProcessor(100, new List<double> { 100, 50 });

            var (result, denom, quantity) = moneyProcessor.AmountCollectedFromCustomer(100,"10a:2");

            result.Should().Be(Constants.ERRORDENOMINATION);
            denom.Should().Be(0);
            quantity.Should().Be(0);
        }
        //Validate if the quantity entered by user can be parsed as int or not
        [Fact]
        public void AmountCollectedReturnInvalidQuantity()
        {
            var moneyProcessor = new MoneyProcessor(100, new List<double> { 100, 50 });

            var (result, denom, quantity) = moneyProcessor.AmountCollectedFromCustomer(100, "100:a");

            result.Should().Be(Constants.ERRORQUANTITY);
            denom.Should().Be(0);
            quantity.Should().Be(0);
        }
        //Valididate if the denomination entered exists in configuration or not
        [Fact]
        public void MoneyProcessorShouldnotBeCreatedWithEmptyDenominations()
        {
            Action action= () => new MoneyProcessor(100, new());

            action.Should().Throw<ArgumentException>()
                .WithMessage($"{Constants.EMPTYDENOM} (Parameter 'denominations')")
                .WithParameterName("denominations");
        }
        //validate if the Coins entered provide correct Denomiation and Quantity
        [Fact]
        public void AmountCollectedReturnEmptyStringAndCorrectDenomQty()
        {
            var moneyProcessor = new MoneyProcessor(100, new List<double>{ 100,50});

            var (result, denom, quantity) = moneyProcessor.AmountCollectedFromCustomer(100, "100:2");

            result.Should().Be(string.Empty);
            denom.Should().Be(100);
            quantity.Should().Be(2);
        }
        //Validate of Amount Collected from customer functions adds correctly to the list or not
        [Fact]
        public void AmountCollectedShouldAddToMoneyFromCustomerList()
        {
            var moneyProcessor = new MoneyProcessor(100, new List<double> { 100, 50 });

            var (_,_,_) = moneyProcessor.AmountCollectedFromCustomer(100, "100:2");
            
            moneyProcessor.moneyFromCustomer.Count.Should().Be(1);
            moneyProcessor.moneyFromCustomer[0].Denomination.Should().Be(100);
            moneyProcessor.moneyFromCustomer[0].Quantity.Should().Be(2);
        }

        //Validate of Amount Collected from customer functions adds correctly to the list - Multiple Inut
        [Fact]
        public void AmountCollectedShouldAddMultipleToMoneyFromCustomerList()
        {
            var moneyProcessor = new MoneyProcessor(100, new List<double> { 100, 50 });

            moneyProcessor.AmountCollectedFromCustomer(100, "50:1");
            moneyProcessor.AmountCollectedFromCustomer(100, "100:1");

            moneyProcessor.moneyFromCustomer.Count.Should().Be(2);
            moneyProcessor.moneyFromCustomer[0].Denomination.Should().Be(50);
            moneyProcessor.moneyFromCustomer[0].Quantity.Should().Be(1);
            moneyProcessor.moneyFromCustomer[1].Denomination.Should().Be(100);
            moneyProcessor.moneyFromCustomer[1].Quantity.Should().Be(1);
        }

        //Validate of Amount Collected from customer functions adds correctly to the list - Decimal Input
        [Fact]
        public void AmountCollectedShouldAddDecimalToMoneyFromCustomerList()
        {
            var moneyProcessor = new MoneyProcessor(10.23, new List<double> { 100, 50, 10, 0.1, 0.05, 0.01 });

            var (result, denom, quantity) = moneyProcessor.AmountCollectedFromCustomer(10.23, "10:1");
            var (result1, denom1, quantity1) = moneyProcessor.AmountCollectedFromCustomer(10.23, "0.1:2");
            var (result2, denom2, quantity2) = moneyProcessor.AmountCollectedFromCustomer(10.23, "0.01:3");

            moneyProcessor.moneyFromCustomer.Count.Should().Be(3);
            moneyProcessor.moneyFromCustomer[0].Denomination.Should().Be(10);
            moneyProcessor.moneyFromCustomer[0].Quantity.Should().Be(1);
            moneyProcessor.moneyFromCustomer[1].Denomination.Should().Be(0.1);
            moneyProcessor.moneyFromCustomer[1].Quantity.Should().Be(2);
            moneyProcessor.moneyFromCustomer[2].Denomination.Should().Be(0.01);
            moneyProcessor.moneyFromCustomer[2].Quantity.Should().Be(3);
        }

        //Unit Tests for ReturnBillsAndCoinsToCustomer
        //validate if the Return Bills function returns correct amount to be provided to customer
        [Fact]
        public void ReturnBillsShouldGiveCorrectReturnAmount()
        {
            var moneyProcessor = new MoneyProcessor(120, new List<double> { 100, 50 });

            var (result, denom, quantity) = moneyProcessor.AmountCollectedFromCustomer(120,"100:2");
            var returnResult = moneyProcessor.ReturnBillsAndCoinsToCustomer();

            returnResult.Should().Be("80");
        }
        //Validate lesser amount provided by customer
        [Fact]
        public void ReturnBillsShouldGiveNegativeAmount()
        {
            var moneyProcessor = new MoneyProcessor(190, new List<double> { 100, 50 });

            var (result, denom, quantity) = moneyProcessor.AmountCollectedFromCustomer(190, "50:2");
            var returnResult = moneyProcessor.ReturnBillsAndCoinsToCustomer();

            returnResult.Should().Be("-90");
        }
        //Validate if return bills are getting added to list or not
        [Fact]
        public void RerutnBillShouldAddToBalanceToCustomerList()
        {
            var moneyProcessor = new MoneyProcessor(130, new List<double> { 100, 50, 20 });

            var (result, denom, quantity) = moneyProcessor.AmountCollectedFromCustomer(130, "100:2");
            var returnResult = moneyProcessor.ReturnBillsAndCoinsToCustomer();

            returnResult.Should().Be("70");
            moneyProcessor.balanceToCustomer.Count.Should().Be(2);
            moneyProcessor.balanceToCustomer[0].Denomination.Should().Be(50);
            moneyProcessor.balanceToCustomer[0].Quantity.Should().Be(1);
            moneyProcessor.balanceToCustomer[1].Denomination.Should().Be(20);
            moneyProcessor.balanceToCustomer[1].Quantity.Should().Be(1);
        }

        //Validate if return bills are getting added to list for Decimal Input
        [Fact]
        public void RerutnBillShouldAddToDecimalBalanceToCustomerList()
        {
            var moneyProcessor = new MoneyProcessor(40.44, new List<double> { 100, 50, 20, 10, 5, 2, 0.5, 0.05, 0.01 });

            var (result, denom, quantity) = moneyProcessor.AmountCollectedFromCustomer(40.44, "50:1");
            var returnResult = moneyProcessor.ReturnBillsAndCoinsToCustomer();

            returnResult.Should().Be("9.56");
            moneyProcessor.balanceToCustomer.Count.Should().Be(5);
            moneyProcessor.balanceToCustomer[0].Denomination.Should().Be(5);
            moneyProcessor.balanceToCustomer[0].Quantity.Should().Be(1);
            moneyProcessor.balanceToCustomer[1].Denomination.Should().Be(2);
            moneyProcessor.balanceToCustomer[1].Quantity.Should().Be(2);
            moneyProcessor.balanceToCustomer[2].Denomination.Should().Be(0.5);
            moneyProcessor.balanceToCustomer[2].Quantity.Should().Be(1);
            moneyProcessor.balanceToCustomer[3].Denomination.Should().Be(0.05);
            moneyProcessor.balanceToCustomer[3].Quantity.Should().Be(1);
            moneyProcessor.balanceToCustomer[4].Denomination.Should().Be(0.01);
            moneyProcessor.balanceToCustomer[4].Quantity.Should().Be(1);
        }

        //Validate UserAmount Received
        [Fact]
        public void ReturnUserAmountShouldGiveCorrectAmount()
        {
            var moneyProcessor = new MoneyProcessor(120, new List<double> { 100, 50 });

            var (result, denom, quantity) = moneyProcessor.AmountCollectedFromCustomer(120,"50:3");
            var userAmount = moneyProcessor.AmountCollected();

            userAmount.Should().Be(150.00);
        }

        //Validate Denomination Count
        [Fact]
        public void ReturnDenomCountShouldGiveCorrectDenomiationCount()
        {
            var moneyProcessor = new MoneyProcessor(120, new List<double> { 100, 50, 20,10 });

            moneyProcessor.AmountCollectedFromCustomer(120, "50:3");
            var denomCount = moneyProcessor.DenomiationCount();

            denomCount.Should().Be(4);
        }

        //Validate Return Amount to Customer
        [Fact]
        public void ReturnAmountToCustmerShouldBeCorrect()
        {
            var moneyProcessor = new MoneyProcessor(190, new List<double> { 100, 50, 20, 10 });

            var (result, denom, quantity) = moneyProcessor.AmountCollectedFromCustomer(190,"50:3");
            var userAmount = moneyProcessor.ReturnAmountToCustomer();

            userAmount.Should().Be(-40);
        }
    }
}
