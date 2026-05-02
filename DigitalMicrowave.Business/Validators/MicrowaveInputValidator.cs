using DigitalMicrowave.Business.Exceptions;

namespace DigitalMicrowave.Business.Validators;

public static class MicrowaveInputValidator
{
    public static int ValidateTime(int? timeInSeconds)
    {
        if (timeInSeconds is null or 0)
            throw new MicrowaveBusinessException("Informe um tempo entre 1 segundo e 2 minutos.", "time");

        if (timeInSeconds < MicrowaveConstants.MinTimeSeconds || timeInSeconds > MicrowaveConstants.MaxTimeSeconds)
            throw new MicrowaveBusinessException("Tempo inválido. Informe entre 1 segundo e 120 segundos (2 minutos).", "time");

        return timeInSeconds.Value;
    }

    public static int ValidatePower(int? power)
    {
        if (power is null or 0)
            return MicrowaveConstants.DefaultPower;

        if (power < MicrowaveConstants.MinPower || power > MicrowaveConstants.MaxPower)
            throw new MicrowaveBusinessException("Potência inválida. Informe um valor entre 1 e 10.", "power");

        return power.Value;
    }

    public static char ValidateHeatingChar(char heatingChar, IEnumerable<char> existingChars)
    {
        if (heatingChar == MicrowaveConstants.DefaultHeatingChar)
            throw new MicrowaveBusinessException("O caractere de aquecimento não pode ser '.'.", "heatingChar");

        if (heatingChar == '\0' || heatingChar == ' ')
            throw new MicrowaveBusinessException("Informe um caractere de aquecimento válido.", "heatingChar");

        if (existingChars.Contains(heatingChar))
            throw new MicrowaveBusinessException($"O caractere '{heatingChar}' já está em uso por outro programa.", "heatingChar");

        return heatingChar;
    }
}
