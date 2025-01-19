using LogisticTracking.Model;
using LogisticTracking.Services;
using Microsoft.AspNetCore.Mvc;
namespace LogisticTracking.Controllers;
[ApiController]
[Route("api/[controller]")]
public class DeliveryPartnerController : ControllerBase
{
    private readonly IDeliveryPartnerService _partnerService;

    public DeliveryPartnerController(IDeliveryPartnerService partnerService)
    {
        _partnerService = partnerService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPartners()
    {
        var partners = await _partnerService.GetAllPartnersAsync();
        return Ok(partners);
    }

    [HttpPost("calculate-payment")]
    public async Task<IActionResult> CalculateTotalPayment(int partnerId)
    {
        await _partnerService.CalculateTotalPaymentAsync(partnerId);
        return Ok();
    }
    [HttpGet("partners-with-status")]
    public async Task<IActionResult> GetAllPartnersWithStatus()
    {
        var partners = await _partnerService.GetAllPartnersAsync();
        return Ok(partners.Select(p => new
        {
            p.DeliveryPartnerId,
            p.Name,
            p.Status
        }));
    }
    [HttpPost]
    public async Task<IActionResult> AddPartner(DeliveryPartner partner)
    {
        await _partnerService.AddDeliveryPartnerAsync(partner);
        return Ok();
    }

}
