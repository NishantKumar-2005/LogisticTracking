using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LogisticTracking.ChatHub;
using LogisticTracking.Db;
using LogisticTracking.Model;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LogisticTracking.Services
{
    public class DeliveryService : IDeliveryService
    {
        private readonly LogisticDbContext _dbContext;
        private readonly IHubContext<LogisticsHub> _hubContext;
        private readonly ILogger<DeliveryService> _logger;

        public DeliveryService(LogisticDbContext dbContext, IHubContext<LogisticsHub> hubContext, ILogger<DeliveryService> logger)
        {
            _dbContext = dbContext;
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task<List<DeliveryModel>> GetAllDeliveriesAsync()
        {
            try
            {
                var Deliveries = await _dbContext.Deliveries.Where(d => d.DeliveryPartnerId != 0).ToListAsync();
                if(Deliveries == null)
                {
                    throw new Exception("No Deliveries found");
                }
                return Deliveries;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all deliveries.");
                throw;
            }
        }

        public async Task<List<DeliveryModel>> GetAllDeliveriesWithNoPartnerAssign(){
            try
            {
                var Deliveries = await _dbContext.Deliveries.Where(d => d.DeliveryPartnerId == 0).ToListAsync();
                if(Deliveries == null)
                {
                    throw new Exception("No Deliveries found");
                }
                return Deliveries;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all deliveries.");
                throw;
            }
        }

        public async Task AssignDeliveryPartnerAsync(int deliveryId, int partnerId)
        {
            try
            {
                var delivery = await _dbContext.Deliveries.FindAsync(deliveryId);
                var partner = await _dbContext.DeliveryPartners.FindAsync(partnerId);
                if (delivery != null && partner != null)
                {
                    delivery.DeliveryPartnerId = partnerId;
                    delivery.Status = DeliveryStatus.InTransit;
                    partner.Status = PartnerStatus.Delivering; // Mark partner as delivering

                    await _dbContext.SaveChangesAsync();
                    await _hubContext.Clients.All.SendAsync("PartnerStatusUpdated", 
                        new { PartnerId = partner.DeliveryPartnerId, Status = partner.Status.ToString() });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while assigning delivery partner.");
                throw;
            }
        }

        public async Task HandleEmergencyAsync(int deliveryId, string emergencyDetails)
        {
            try
            {
                var delivery = await _dbContext.Deliveries.FindAsync(deliveryId);
                if (delivery != null)
                {
                    delivery.Status = DeliveryStatus.Emergency;
                    delivery.EmergencyDetails = emergencyDetails;
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while handling emergency.");
                throw;
            }
        }

        public async Task HandleEmergencyAndReassignAsync(int deliveryId, string emergencyDetails)
        {
            try
            {
                var delivery = await _dbContext.Deliveries.FindAsync(deliveryId);
                if (delivery != null)
                {
                    // Mark the delivery as emergency
                    delivery.Status = DeliveryStatus.Emergency;
                    delivery.EmergencyDetails = emergencyDetails;

                    // Find a new delivery partner
                    var newPartner = await _dbContext.DeliveryPartners
                        .OrderBy(dp => dp.DeliveriesId.Count) // Example criteria: least deliveries
                        .FirstOrDefaultAsync();

                    if (newPartner != null)
                    {
                        delivery.DeliveryPartnerId = newPartner.DeliveryPartnerId;
                        delivery.Status = DeliveryStatus.InTransit; // Update status
                    }

                    await _dbContext.SaveChangesAsync();

                    // Notify via SignalR
                    await _hubContext.Clients.All.SendAsync("ReceiveEmergency", 
                        $"Emergency handled. New partner assigned: {newPartner?.Name}");
                    await _hubContext.Clients.All.SendAsync("PartnerStatusUpdated", 
                        new { PartnerId = newPartner.DeliveryPartnerId, Status = newPartner.Status.ToString() });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while handling emergency and reassigning.");
                throw;
            }
        }

        public async Task CompleteDeliveryAsync(int deliveryId)
        {
            try
            {
                var delivery = await _dbContext.Deliveries.Include(d => d.DeliveryPartnerId).FirstOrDefaultAsync(d => d.DeliveryId == deliveryId);
                if (delivery != null)
                {
                    delivery.Status = DeliveryStatus.Delivered;
                    var partner = await _dbContext.DeliveryPartners.FindAsync(delivery.DeliveryPartnerId);
                    partner.Status = PartnerStatus.Available; // Mark partner as available

                    await _dbContext.SaveChangesAsync();
                    await _hubContext.Clients.All.SendAsync("PartnerStatusUpdated", 
                        new { PartnerId = partner.DeliveryPartnerId, Status = partner.Status.ToString() });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while completing delivery.");
                throw;
            }
        }

        public async Task AddDeliveryAsync(DeliveryModel delivery)

        {
            try
            {
                _dbContext.Deliveries.Add(delivery);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding delivery.");
                throw;
            }
        }
        public async Task<DeliveryModel> GetDeliveryByIdAsync(int deliveryId)
        {
            try
            {
                
                var Delivery = await _dbContext.Deliveries.FindAsync(deliveryId);
                if(Delivery == null)
                {
                    throw new Exception("Delivery not found");
                }
                return Delivery;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting delivery by ID {DeliveryId}.", deliveryId);
                throw;
            }
        }
    }
}