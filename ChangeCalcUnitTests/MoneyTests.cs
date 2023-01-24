using ChangeCalculator;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeCalculatorTest
{
    //Classto Handle Monry Class Unit Test Cases
    public class MoneyTests
    {
        //Validate if Money Class provides correct Amount
        [Fact]
        public void MoneyShouldGiveExpectedAmount()
        {
            //Arrange
            var money = new Money(100, 2);
            //Act
            var amount = money.Amount;
            //Assert
            amount.Should().Be(200);   
        }
        //Validdate if Money is displayed to user with correct currency symbol
        [Fact]
        public void MoneyShouldBeDisplyedWithCurrencySymbol()
        {
            //Arrange
            var money = new Money(100, 2);
            //Act
            var amountWithCurrencySymbol = money.ToString();
            //Assert
            amountWithCurrencySymbol.Should().Be("$200.00");
        }
        //Validate if the Message displayed to user after entering coins is in Denomination : Quantity = Value format
        [Fact]
        public void MoneyShouldBeDisplayedInRequiredFormat()
        {
            //Arrange
            var money = new Money(100, 2);
            //Act
            var moneyInRequiredFormat = money.formatMoney();
            //Assert
            moneyInRequiredFormat.Should().Be("100 : 2 = $200.00");
        }
    }
}
