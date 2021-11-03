namespace DeliveryRates
{
    using DeliveryRates.Exceptions;
    using DeliveryRates.Models;
    using System.Collections.Generic;
    using System.Linq;

    public class PriceCalculator : IPriceCalculator
    {
        const string vorarlberg = "Vorarlberg";
        const string tirol = "Tirol";

        public List<Order> CalculateOrdersPrice(List<Order> inputOrders) 
        {
            var orders = this.FilterOrders(inputOrders);
            var groupedOders = this.GroupOdersBySender(orders);
            var calculatedPrices = this.CalculatePrices(groupedOders);
            var samteDayItemOrderDiscounts = this.AddDiscountForSameDayItemOrder(calculatedPrices);
            var premiumClinetDiscounts = this.AddDiscountForPremiumClinets(samteDayItemOrderDiscounts);

            return this.GetAllOders(premiumClinetDiscounts);
        }

        private Dictionary<string, List<Order>> AddDiscountForPremiumClinets(Dictionary<string, List<Order>> samteDayItemOrderDiscounts)
        {
            foreach (var sender in samteDayItemOrderDiscounts.Keys)
            {
                var groupedOrdersForSender = samteDayItemOrderDiscounts[sender];

                foreach (var premiumOrder in groupedOrdersForSender.Where(x => x.ClientType == ClientType.Premium && x.PackageType != PackageType.LargeParcel))
                {
                    premiumOrder.Price = premiumOrder.Price - (premiumOrder.Price * 0.075m);
                }
            }

            return samteDayItemOrderDiscounts;
        }

        private List<Order> FilterOrders(List<Order> inputOrders)
        {
            foreach (var order in inputOrders)
            {
                if (order.RecipientAddress.Contains(vorarlberg) || order.RecipientAddress.Contains(tirol))
                {
                    order.DeliveryType = DeliveryType.TwoDays;
                }
            }

            return inputOrders;
        }

        private List<Order> GetAllOders(Dictionary<string, List<Order>> groupedOders)
        {
            var listOfOrders = new List<Order>();
            foreach (var sender in groupedOders.Keys)
            {
                listOfOrders.AddRange(groupedOders[sender]);
            }

            return listOfOrders;
        }

        private Dictionary<string, List<Order>> AddDiscountForSameDayItemOrder(Dictionary<string, List<Order>> groupedOders)
        {
            foreach (var sender in groupedOders.Keys)
            {
                var groupedOrdersForSender = groupedOders[sender];

                var sameDayOrdersCount = groupedOrdersForSender.Where(x => x.DeliveryType == DeliveryType.SameDay).Count();
                if (sameDayOrdersCount >= 3)
                {
                    this.AddFivePercentPriceDiscount(groupedOrdersForSender);
                }
            }

            return groupedOders;
        }

        private void AddFivePercentPriceDiscount(List<Order> groupedOrdersForSender)
        {
            foreach (var order in groupedOrdersForSender)
            {
                order.Price = order.Price - (order.Price * 0.05m);
            }
        }

        private Dictionary<string, List<Order>> CalculatePrices(Dictionary<string, List<Order>> groupedOders)
        {
            foreach (var sender in groupedOders.Keys)
            {
                var groupedOrdersForSender = groupedOders[sender];

                foreach (var order in groupedOrdersForSender)
                {
                    order.Price = this.ChooseSuitablePrice(order);
                }
            }

            return groupedOders;
        }
    
        private Dictionary<string, List<Order>> GroupOdersBySender(List<Order> orders)
        {
            var groupedOders = new Dictionary<string, List<Order>>();
            foreach (var order in orders)
            {
                if (!groupedOders.ContainsKey(order.SenderAddress))
                {
                    groupedOders.Add(order.SenderAddress, new List<Order> { order });
                }
                else
                {
                    groupedOders[order.SenderAddress].Add(order);
                }
            }

            return groupedOders;
        }

        private decimal ChooseSuitablePrice(Order order)
        {
            if (order.DeliveryType == DeliveryType.SameDay)
            {
                switch (order.PackageType)
                {
                    case PackageType.Documents: return 4m;
                    case PackageType.SmallParcel: return 7m;
                    case PackageType.LargeParcel: return 9m;
                }
            }
            else if (order.DeliveryType == DeliveryType.TwoDays)
            {
                switch (order.PackageType)
                {
                    case PackageType.Documents: return 1m;
                    case PackageType.SmallParcel: return 2.5m;
                    case PackageType.LargeParcel: return 3m;
                }
            }

            throw new UnsupportedDeliveryAndPackageTypeException();
        }
    }
}
