namespace DigitalMicrowave.Business.Models;

public class HeatingProgram
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Food { get; set; } = string.Empty;
    public int TimeInSeconds { get; set; }
    public int Power { get; set; }
    public char HeatingChar { get; set; }
    public string? Instructions { get; set; }
    public bool IsCustom { get; set; }
    public bool IsPreDefined => !IsCustom;
}
