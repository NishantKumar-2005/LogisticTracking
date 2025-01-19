using Microsoft.AspNetCore.SignalR;
namespace LogisticTracking.ChatHub;
public class LogisticsHub : Hub
{
    public async Task NotifyEmergency(string message)
    {
        await Clients.All.SendAsync("ReceiveEmergency", message);
    }
    public async Task UpdatePartnerStatus(int partnerId, string status)
{
    await Clients.All.SendAsync("PartnerStatusUpdated", new { PartnerId = partnerId, Status = status });
}

}
