using LogisticTracking.Model;

public interface IDeliveryService
{
    Task<List<DeliveryModel>> GetAllDeliveriesAsync();
    Task AssignDeliveryPartnerAsync(int deliveryId, int partnerId);
    Task HandleEmergencyAsync(int deliveryId, string emergencyDetails);

    Task HandleEmergencyAndReassignAsync(int deliveryId, string emergencyDetails);
    Task CompleteDeliveryAsync(int deliveryId);
    Task AddDeliveryAsync(DeliveryModel delivery);
    Task<DeliveryModel> GetDeliveryByIdAsync(int deliveryId);

    Task<List<DeliveryModel>> GetAllDeliveriesWithNoPartnerAssign();
}