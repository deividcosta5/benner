using DigitalMicrowave.Business.Models;
using DigitalMicrowave.Business.Services;
using Microsoft.AspNetCore.SignalR;

namespace DigitalMicrowave.Web.Hubs;

public class MicrowaveHub : Hub
{
    private readonly IMicrowaveService _microwaveService;

    public MicrowaveHub(IMicrowaveService microwaveService)
    {
        _microwaveService = microwaveService;
    }

    public override async Task OnConnectedAsync()
    {
        var session = _microwaveService.GetSession();
        await Clients.Caller.SendAsync("sessionUpdated", SessionDto(session));
        await base.OnConnectedAsync();
    }

    public static object SessionDto(HeatingSession s) => new
    {
        status = s.Status.ToString(),
        remainingTime = s.FormattedRemainingTime,
        totalTime = s.TotalTimeInSeconds,
        power = s.Power,
        heatingString = s.HeatingString,
        programName = s.Program?.Name
    };
}
