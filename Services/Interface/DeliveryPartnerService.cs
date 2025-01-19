using LogisticTracking.Db;
using LogisticTracking.Model;
using Microsoft.EntityFrameworkCore;

namespace LogisticTracking.Services;
public class DeliveryPartnerService : IDeliveryPartnerService
{
    private readonly LogisticDbContext _context;

    public DeliveryPartnerService(LogisticDbContext context)
    {
        _context = context;
    }

    public async Task<List<DeliveryPartner>> GetAllPartnersAsync()
    {
        return await _context.DeliveryPartners.ToListAsync();
    }

    public async Task CalculateTotalPaymentAsync(int partnerId)
    {
        var partner = await _context.DeliveryPartners
            .Include(dp => dp.DeliveriesId)
            .FirstOrDefaultAsync(dp => dp.DeliveryPartnerId == partnerId);

        if (partner != null)
        {
            foreach (var deliveryId in partner.DeliveriesId)
            {
                var delivery = await _context.Deliveries.FindAsync(deliveryId);
                partner.TotalPayment += delivery.Payment;
            }
            await _context.SaveChangesAsync();
        }
    }

    public async Task AddDeliveryPartnerAsync(DeliveryPartner partner)
    {
        await _context.DeliveryPartners.AddAsync(partner);
        await _context.SaveChangesAsync();
    }
}
