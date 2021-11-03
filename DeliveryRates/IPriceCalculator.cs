namespace DeliveryRates
{
    using DeliveryRates.Models;
    using System.Collections.Generic;

    public interface IPriceCalculator
    {
        /// <summary>
        /// Calculate price and discount for collection of orders
        /// </summary>
        /// <param name="inputOrders"></param>
        /// <returns></returns>
        List<Order> CalculateOrdersPrice(List<Order> inputOrders);
    }
}
