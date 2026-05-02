using DigitalMicrowave.Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalMicrowave.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MicrowaveController : ControllerBase
{
    private readonly IMicrowaveService _service;

    public MicrowaveController(IMicrowaveService service) => _service = service;

    [HttpGet("session")]
    public IActionResult GetSession() => Ok(_service.GetSession());

    [HttpPost("start")]
    public IActionResult Start([FromBody] StartRequest? request)
        => Ok(_service.StartHeating(request?.TimeInSeconds, request?.Power));

    [HttpPost("start/program/{programId:int}")]
    public IActionResult StartProgram(int programId)
        => Ok(_service.StartProgram(programId));

    [HttpPost("pause-cancel")]
    public IActionResult PauseOrCancel()
        => Ok(_service.PauseOrCancel());
}

public record StartRequest(int? TimeInSeconds, int? Power);
