namespace PortfolioManager.Core.Tests
{
    using System;
    using FluentAssertions;
    using NUnit.Framework;

    public class TransactionTests
    {
        [Test]
        public void ChipAwayBuyAmountTest()
        {
            var transaction = new Transaction()
            {
                RowIndex = 1,
                SellAmount = 100000m,
                SellAsset = Assets.CZK,
                BuyAmount = 10m,
                BuyAsset = Assets.BTC,
                CreatedUtc = new DateTime(2022, 1, 1).Date,
                Order = 10,
                TransactionSource = TransactionSource.LocalBitcoins
            };


            var chipAwayAmount = 7m;
            var newTransaction = transaction.ChipAwayBuyAmount(chipAwayAmount);
            newTransaction.BuyAmount.Should().Be(chipAwayAmount);
            newTransaction.BuyAsset.Should().Be(transaction.BuyAsset);
            newTransaction.SellAmount.Should().Be(chipAwayAmount * 100000m / 10m);

            transaction.BuyAmount.Should().Be(10m - chipAwayAmount);
            transaction.BuyAmount.Should().Be(3m);
        }
    }
}