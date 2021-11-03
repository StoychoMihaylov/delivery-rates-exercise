namespace DeliveryRates.UnitTests
{
    using DeliveryRates.Models;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class PriceCalculatorTest
    {
        // Single order with delivery type "same day" and package with different types".
        [Theory]
        [InlineData(4, PackageType.Documents)]
        [InlineData(7, PackageType.SmallParcel)]
        [InlineData(9, PackageType.LargeParcel)]
        public void CalculateOrdersPrice_SingleSameDayOrderDifferentPackageType_ShouldReturnCorrectEuroPrice(decimal expectedPrice, PackageType packageType)
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order
                { 
                    SenderAddress = "Vorgartenstrasse 128, Vienna",
                    RecipientAddress = "Marktgraben 12 Innsbruck",
                    DeliveryType = DeliveryType.SameDay,
                    PackageType = packageType
                }
            };

            var priceCalculator = new PriceCalculator();

            // Act
            var calculatedPrices = priceCalculator.CalculateOrdersPrice(orders);

            // Assert
            Assert.Equal(expectedPrice, calculatedPrices.First().Price);
        }

        // Single order with delivery type "two days" and package with different types".
        [Theory]
        [InlineData(1, PackageType.Documents)]
        [InlineData(2.5, PackageType.SmallParcel)]
        [InlineData(3, PackageType.LargeParcel)]
        public void CalculateOrdersPrice_SingleTwoDaysOrderDifferentPackageType_ShouldReturnCorrectEuroPrice(decimal expectedPrice, PackageType packageType)
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order
                {
                    SenderAddress = "Vorgartenstrasse 128, Vienna",
                    RecipientAddress = "Marktgraben 12 Innsbruck",
                    DeliveryType = DeliveryType.TwoDays,
                    PackageType = packageType
                }
            };

            var priceCalculator = new PriceCalculator();

            // Act
            var calculatedPrices = priceCalculator.CalculateOrdersPrice(orders);

            // Assert
            Assert.Equal(expectedPrice, calculatedPrices.First().Price);
        }

        [Fact]
        public void CalculateOdersDiscountPriceForSameDay_ThreeOrdersForheSameDay_ShouldReturnFivePercentDiscount()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order
                {
                    SenderAddress = "Vorgartenstrasse 128, Vienna",
                    RecipientAddress = "Marktgraben 12 Innsbruck",
                    DeliveryType = DeliveryType.SameDay,
                    PackageType = PackageType.LargeParcel
                },
                 new Order
                {
                    SenderAddress = "Vorgartenstrasse 128, Vienna",
                    RecipientAddress = "Marktgraben 12 Innsbruck",
                    DeliveryType = DeliveryType.SameDay,
                    PackageType = PackageType.LargeParcel
                },
                  new Order
                {
                    SenderAddress = "Vorgartenstrasse 128, Vienna",
                    RecipientAddress = "Marktgraben 12 Innsbruck",
                    DeliveryType = DeliveryType.SameDay,
                    PackageType = PackageType.LargeParcel
                },
            };

            var priceCalculator = new PriceCalculator();

            // Act
            var calculatedPrices = priceCalculator.CalculateOrdersPrice(orders);

            // Assert
            Assert.Equal(8.55m, calculatedPrices.First().Price);
        }

        [Fact]
        public void FilterVorrarlbergAndTirolRecepinAddresses_DeliveryTypeSameDay_ShouldChangeDeliveryTypeToTwoDays()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order
                {
                    SenderAddress = "Vorgartenstrasse 128, Vienna",
                    RecipientAddress = "Marktgraben 12 Innsbruck, Tirol", // Tirol not possible for same day
                    DeliveryType = DeliveryType.SameDay,
                    PackageType = PackageType.Documents
                }
            };

            var priceCalculator = new PriceCalculator();

            // Act
            var calculatedPrices = priceCalculator.CalculateOrdersPrice(orders);

            // Assert
            Assert.Equal(DeliveryType.TwoDays, calculatedPrices.First().DeliveryType);
        }

        [Theory]
        [InlineData(3.7, PackageType.Documents)]
        [InlineData(6.475, PackageType.SmallParcel)]
        [InlineData(9, PackageType.LargeParcel)]
        public void PremiumClinetDelivery_PremiumDelivery_ShouldReturnPremiumClientDiscount(decimal expectedPrice, PackageType packageType)
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order
                {
                    SenderAddress = "Vorgartenstrasse 128, Vienna",
                    RecipientAddress = "Marktgraben 12 Innsbruck",
                    DeliveryType = DeliveryType.SameDay,
                    PackageType = packageType,
                    ClientType = ClientType.Premium
                }
            };

            var priceCalculator = new PriceCalculator();

            // Act
            var calculatedPrices = priceCalculator.CalculateOrdersPrice(orders);

            // Assert
            Assert.Equal(expectedPrice, calculatedPrices.First().Price);
        }
    }
}
