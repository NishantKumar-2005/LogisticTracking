using LogisticTracking.Model;

namespace LogisticTracking.Services;

public interface IDeliveryPartnerService{
    Task<List<DeliveryPartner>> GetAllPartnersAsync();
    Task CalculateTotalPaymentAsync(int partnerId);
    Task AddDeliveryPartnerAsync(DeliveryPartner partner);
    
}