using DigitalMicrowave.Business.Models;
using DigitalMicrowave.Business.Services;
using Microsoft.AspNetCore.SignalR;

namespace DigitalMicrowave.Web.Hubs;

public class HeatingBackgroundService : BackgroundService
{
    private readonly IMicrowaveService _microwaveService;
    private readonly IHubContext<MicrowaveHub> _hub;

    public HeatingBackgroundService(IMicrowaveService microwaveService, IHubContext<MicrowaveHub> hub)
    {
        _microwaveService = microwaveService;
        _hub = hub;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);

            var session = _microwaveService.GetSession();
            if (session.Status != HeatingStatus.Running) continue;

            session = _microwaveService.Tick();
            await _hub.Clients.All.SendAsync("sessionUpdated", MicrowaveHub.SessionDto(session), stoppingToken);
        }
    }
}
