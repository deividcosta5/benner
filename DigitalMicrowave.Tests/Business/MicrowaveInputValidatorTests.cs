using DigitalMicrowave.Business.Exceptions;
using DigitalMicrowave.Business.Validators;
using Xunit;

namespace DigitalMicrowave.Tests.Business;

public class MicrowaveInputValidatorTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(60)]
    [InlineData(120)]
    public void ValidateTime_ValoresValidos_Retorna(int segundos)
        => Assert.Equal(segundos, MicrowaveInputValidator.ValidateTime(segundos));

    [Theory]
    [InlineData(0)]
    [InlineData(121)]
    [InlineData(-5)]
    public void ValidateTime_ForaDosLimites_LancaException(int segundos)
        => Assert.Throws<MicrowaveBusinessException>(() => MicrowaveInputValidator.ValidateTime(segundos));

    [Fact]
    public void ValidateTime_Nulo_LancaException()
        => Assert.Throws<MicrowaveBusinessException>(() => MicrowaveInputValidator.ValidateTime(null));

    [Fact]
    public void ValidatePower_Nulo_Retorna10()
        => Assert.Equal(10, MicrowaveInputValidator.ValidatePower(null));

    [Fact]
    public void ValidatePower_Zero_Retorna10()
        => Assert.Equal(10, MicrowaveInputValidator.ValidatePower(0));

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    public void ValidatePower_ValoresValidos_Retorna(int potencia)
        => Assert.Equal(potencia, MicrowaveInputValidator.ValidatePower(potencia));

    [Theory]
    [InlineData(-1)]
    [InlineData(11)]
    public void ValidatePower_ForaDosLimites_LancaException(int potencia)
        => Assert.Throws<MicrowaveBusinessException>(() => MicrowaveInputValidator.ValidatePower(potencia));

    [Fact]
    public void ValidateHeatingChar_Ponto_LancaException()
        => Assert.Throws<MicrowaveBusinessException>(() =>
            MicrowaveInputValidator.ValidateHeatingChar('.', Array.Empty<char>()));

    [Fact]
    public void ValidateHeatingChar_JaUsado_LancaException()
        => Assert.Throws<MicrowaveBusinessException>(() =>
            MicrowaveInputValidator.ValidateHeatingChar('x', new[] { 'x', 'y' }));

    [Fact]
    public void ValidateHeatingChar_NovoCaractere_Retorna()
        => Assert.Equal('z', MicrowaveInputValidator.ValidateHeatingChar('z', new[] { 'x', 'y' }));
}
