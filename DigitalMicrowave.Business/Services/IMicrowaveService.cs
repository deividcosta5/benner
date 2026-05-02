using DigitalMicrowave.Business.Models;

namespace DigitalMicrowave.Business.Services;

public interface IMicrowaveService
{
    HeatingSession GetSession();
    HeatingSession StartHeating(int? timeInSeconds, int? power);
    HeatingSession StartProgram(int programId);
    HeatingSession PauseOrCancel();
    HeatingSession Tick();
}
