using DigitalMicrowave.Business.Exceptions;
using DigitalMicrowave.Business.Models;
using DigitalMicrowave.Business.Services;
using DigitalMicrowave.Web.Hubs;
using DigitalMicrowave.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace DigitalMicrowave.Web.Controllers;

public class HomeController : Controller
{
    private readonly IMicrowaveService _microwaveService;
    private readonly IProgramService _programService;
    private readonly IHubContext<MicrowaveHub> _hub;

    public HomeController(IMicrowaveService microwaveService, IProgramService programService, IHubContext<MicrowaveHub> hub)
    {
        _microwaveService = microwaveService;
        _programService = programService;
        _hub = hub;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var vm = new MicrowaveViewModel
        {
            Session = _microwaveService.GetSession(),
            Programs = _programService.GetAll().ToList()
        };
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Start(int? timeInSeconds, int? power)
    {
        try
        {
            var session = _microwaveService.StartHeating(timeInSeconds, power);
            await Broadcast(session);
        }
        catch (MicrowaveBusinessException ex)
        {
            TempData["Error"] = ex.Message;
            TempData["ErrorField"] = ex.Field;
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> StartProgram(int programId)
    {
        try
        {
            var session = _microwaveService.StartProgram(programId);
            await Broadcast(session);
        }
        catch (MicrowaveBusinessException ex)
        {
            TempData["Error"] = ex.Message;
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> PauseOrCancel()
    {
        var session = _microwaveService.PauseOrCancel();
        await Broadcast(session);
        return RedirectToAction(nameof(Index));
    }

    private Task Broadcast(HeatingSession session)
        => _hub.Clients.All.SendAsync("sessionUpdated", MicrowaveHub.SessionDto(session));
}
