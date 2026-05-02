namespace DigitalMicrowave.Business.Exceptions;

public class MicrowaveBusinessException : Exception
{
    public string Field { get; }

    public MicrowaveBusinessException(string message, string field = "") : base(message)
    {
        Field = field;
    }
}
