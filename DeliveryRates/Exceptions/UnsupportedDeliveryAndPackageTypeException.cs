namespace DeliveryRates.Exceptions
{
    using System;

    public class UnsupportedDeliveryAndPackageTypeException : Exception
    {
        public UnsupportedDeliveryAndPackageTypeException(string message = "Unsupported delivery or package types!")
               : base(message)
        { }
    }
}
