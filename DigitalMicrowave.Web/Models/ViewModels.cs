using System.ComponentModel.DataAnnotations;
using DigitalMicrowave.Business.Models;

namespace DigitalMicrowave.Web.Models;

public class MicrowaveViewModel
{
    public HeatingSession Session { get; set; } = new();
    public IList<HeatingProgram> Programs { get; set; } = new List<HeatingProgram>();
}

public class CreateProgramViewModel
{
    [Required(ErrorMessage = "O nome do programa é obrigatório.")]
    [MaxLength(100)]
    public string? Name { get; set; }

    [Required(ErrorMessage = "O alimento é obrigatório.")]
    [MaxLength(200)]
    public string? Food { get; set; }

    [Required(ErrorMessage = "O tempo é obrigatório.")]
    [Range(1, 120, ErrorMessage = "O tempo deve ser entre 1 e 120 segundos.")]
    public int TimeInSeconds { get; set; }

    [Required(ErrorMessage = "A potência é obrigatória.")]
    [Range(1, 10, ErrorMessage = "A potência deve ser entre 1 e 10.")]
    public int Power { get; set; }

    [Required(ErrorMessage = "O caractere de aquecimento é obrigatório.")]
    public char HeatingChar { get; set; }

    [MaxLength(500)]
    public string? Instructions { get; set; }
}
