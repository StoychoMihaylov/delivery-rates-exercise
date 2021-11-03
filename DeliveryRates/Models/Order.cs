namespace DeliveryRates.Models
{
    public class Order
    {
        public decimal Price { get; set; }

        public string SenderAddress { get; set; }
        public string RecipientAddress { get; set; }

        public DeliveryType DeliveryType { get; set; }

        public PackageType PackageType { get; set; }

        public ClientType ClientType { get; set; }
    }

    public enum DeliveryType
    {       
        SameDay,
        TwoDays
    }

    public enum PackageType
    {
        Documents,
        SmallParcel,
        LargeParcel,
    }

    public enum ClientType
    { 
        Normal,
        Premium
    }
}
