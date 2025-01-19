namespace LogisticTracking.Model
{
    public enum DeliveryStatus
    {
        Pending,
        InTransit,
        Delivered,
        Cancelled,
        Emergency
    }
    public enum PartnerStatus
{
    Available,
    Delivering,
    Unavailable
}
}