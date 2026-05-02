using DigitalMicrowave.Business.Exceptions;
using DigitalMicrowave.Business.Models;
using DigitalMicrowave.Business.Repositories;
using DigitalMicrowave.Business.Validators;

namespace DigitalMicrowave.Business.Services;

public class MicrowaveService : IMicrowaveService
{
    private readonly IProgramRepository _repository;
    private HeatingSession _session;
    private readonly object _lock = new();

    public MicrowaveService(IProgramRepository repository)
    {
        _repository = repository;
        _session = new HeatingSession();
    }

    public HeatingSession GetSession()
    {
        lock (_lock) return Clone(_session);
    }

    public HeatingSession StartHeating(int? timeInSeconds, int? power)
    {
        lock (_lock)
        {
            if (_session.IsRunning)
            {
                if (_session.IsPreDefinedProgram)
                    throw new MicrowaveBusinessException("Não é permitido adicionar tempo em programas pré-definidos.");

                _session.RemainingTimeInSeconds += MicrowaveConstants.AddTimeSeconds;
                _session.TotalTimeInSeconds += MicrowaveConstants.AddTimeSeconds;
                return Clone(_session);
            }

            if (_session.IsPaused)
            {
                _session.Status = HeatingStatus.Running;
                return Clone(_session);
            }

            bool quickStart = (timeInSeconds is null or 0) && (power is null or 0);

            int time = quickStart
                ? MicrowaveConstants.QuickStartSeconds
                : MicrowaveInputValidator.ValidateTime(timeInSeconds);

            int pot = quickStart
                ? MicrowaveConstants.DefaultPower
                : MicrowaveInputValidator.ValidatePower(power);

            _session = new HeatingSession
            {
                TotalTimeInSeconds = time,
                RemainingTimeInSeconds = time,
                Power = pot,
                Status = HeatingStatus.Running
            };

            return Clone(_session);
        }
    }

    public HeatingSession StartProgram(int programId)
    {
        lock (_lock)
        {
            if (_session.IsPaused && _session.Program?.Id == programId)
            {
                _session.Status = HeatingStatus.Running;
                return Clone(_session);
            }

            var program = _repository.GetById(programId)
                ?? throw new MicrowaveBusinessException($"Programa {programId} não encontrado.");

            _session = new HeatingSession
            {
                TotalTimeInSeconds = program.TimeInSeconds,
                RemainingTimeInSeconds = program.TimeInSeconds,
                Power = program.Power,
                Status = HeatingStatus.Running,
                Program = program
            };

            return Clone(_session);
        }
    }

    public HeatingSession PauseOrCancel()
    {
        lock (_lock)
        {
            if (_session.IsRunning)
                _session.Status = HeatingStatus.Paused;
            else
                _session = new HeatingSession();

            return Clone(_session);
        }
    }

    public HeatingSession Tick()
    {
        lock (_lock)
        {
            if (!_session.IsRunning) return Clone(_session);

            char ch = _session.Program?.HeatingChar ?? MicrowaveConstants.DefaultHeatingChar;
            _session.HeatingString += new string(ch, _session.Power);
            _session.RemainingTimeInSeconds--;

            if (_session.RemainingTimeInSeconds <= 0)
            {
                _session.RemainingTimeInSeconds = 0;
                _session.HeatingString += "Aquecimento concluído";
                _session.Status = HeatingStatus.Completed;
            }

            return Clone(_session);
        }
    }

    private static HeatingSession Clone(HeatingSession s) => new()
    {
        TotalTimeInSeconds = s.TotalTimeInSeconds,
        RemainingTimeInSeconds = s.RemainingTimeInSeconds,
        Power = s.Power,
        Status = s.Status,
        HeatingString = s.HeatingString,
        Program = s.Program
    };
}
