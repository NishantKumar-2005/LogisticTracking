using LogisticTracking.Model;
using LogisticTracking.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LogisticTracking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliveryService _deliveryService;
        private readonly ILogger<DeliveryController> _logger;

        public DeliveryController(IDeliveryService deliveryService, ILogger<DeliveryController> logger)
        {
            _deliveryService = deliveryService;
            _logger = logger;
        }

        [HttpGet("get-all-deliveries")]
        public async Task<IActionResult> GetAllDeliveries()
        {
            try
            {
                var deliveries = await _deliveryService.GetAllDeliveriesAsync();
                return Ok(deliveries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all deliveries.");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("get-DeliveryById")]
        public async Task<IActionResult> GetDeliveryById(int deliveryId)
        {
            try
            {
                var delivery = await _deliveryService.GetDeliveryByIdAsync(deliveryId);
                return Ok(delivery);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting delivery by ID {DeliveryId}.", deliveryId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("handle-emergency")]
        public async Task<IActionResult> HandleEmergencyAndReassign(int deliveryId, string emergencyDetails)
        {
            try
            {
                await _deliveryService.HandleEmergencyAndReassignAsync(deliveryId, emergencyDetails);
                _logger.LogInformation("Emergency handled and new partner assigned for delivery ID {DeliveryId}.", deliveryId);
                return Ok("Emergency handled and new partner assigned.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while handling emergency and reassigning for delivery ID {DeliveryId}.", deliveryId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("complete-delivery")]
        public async Task<IActionResult> CompleteDelivery(int deliveryId)
        {
            try
            {
                await _deliveryService.CompleteDeliveryAsync(deliveryId);
                _logger.LogInformation("Delivery marked as completed for delivery ID {DeliveryId}.", deliveryId);
                return Ok("Delivery marked as completed, and partner is now available.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while completing delivery for delivery ID {DeliveryId}.", deliveryId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("add-delivery")]
        public async Task<IActionResult> AddDelivery(DeliveryModel delivery)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for delivery.");
                return BadRequest(ModelState);
            }

            try
            {
                await _deliveryService.AddDeliveryAsync(delivery);
                _logger.LogInformation("Delivery added successfully.");
                return Ok("Delivery added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding delivery.");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("get-all-deliveries-with-no-partner-assign")]
        public async Task<IActionResult> GetAllDeliveriesWithNoPartnerAssign()
        {
            try
            {
                var deliveries = await _deliveryService.GetAllDeliveriesWithNoPartnerAssign();
                return Ok(deliveries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all deliveries with no partner assigned.");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost("assign-delivery-partner")]
        public async Task<IActionResult> AssignDeliveryPartner(int deliveryId, int partnerId)
        {
            try
            {
                await _deliveryService.AssignDeliveryPartnerAsync(deliveryId, partnerId);
                _logger.LogInformation("Delivery partner assigned for delivery ID {DeliveryId}.", deliveryId);
                return Ok("Delivery partner assigned successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while assigning delivery partner for delivery ID {DeliveryId}.", deliveryId);
                return StatusCode(500, "Internal server error");
            }
        }

        // [HttpPost("handle-emergency")]
        // public async Task<IActionResult> HandleEmergency(int deliveryId, string emergencyDetails)
        // {
        //     try
        //     {
        //         await _deliveryService.HandleEmergencyAsync(deliveryId, emergencyDetails);
        //         _logger.LogInformation("Emergency handled for delivery ID {DeliveryId}.", deliveryId);
        //         return Ok("Emergency handled successfully.");
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "Error occurred while handling emergency for delivery ID {DeliveryId}.", deliveryId);
        //         return StatusCode(500, "Internal server error");
        //     }
        // }
    }
}