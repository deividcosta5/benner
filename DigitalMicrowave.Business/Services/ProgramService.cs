using DigitalMicrowave.Business.Exceptions;
using DigitalMicrowave.Business.Models;
using DigitalMicrowave.Business.Repositories;
using DigitalMicrowave.Business.Validators;

namespace DigitalMicrowave.Business.Services;

public class ProgramService : IProgramService
{
    private readonly IProgramRepository _repository;

    public ProgramService(IProgramRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<HeatingProgram> GetAll() => _repository.GetAll();

    public HeatingProgram? GetById(int id) => _repository.GetById(id);

    public HeatingProgram Create(HeatingProgram program)
    {
        if (string.IsNullOrWhiteSpace(program.Name))
            throw new MicrowaveBusinessException("O nome do programa é obrigatório.", "name");

        if (string.IsNullOrWhiteSpace(program.Food))
            throw new MicrowaveBusinessException("O alimento é obrigatório.", "food");

        MicrowaveInputValidator.ValidateTime(program.TimeInSeconds);
        MicrowaveInputValidator.ValidatePower(program.Power);
        MicrowaveInputValidator.ValidateHeatingChar(program.HeatingChar, _repository.GetAllHeatingChars());

        program.IsCustom = true;
        return _repository.Add(program);
    }

    public bool Delete(int id)
    {
        var program = _repository.GetById(id)
            ?? throw new MicrowaveBusinessException($"Programa {id} não encontrado.");

        if (program.IsPreDefined)
            throw new MicrowaveBusinessException("Programas pré-definidos não podem ser excluídos.");

        return _repository.Delete(id);
    }
}
