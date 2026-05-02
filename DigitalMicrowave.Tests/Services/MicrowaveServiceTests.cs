using DigitalMicrowave.Business.Exceptions;
using DigitalMicrowave.Business.Models;
using DigitalMicrowave.Business.Repositories;
using DigitalMicrowave.Business.Services;
using Moq;
using Xunit;

namespace DigitalMicrowave.Tests.Services;

public class MicrowaveServiceTests
{
    private readonly Mock<IProgramRepository> _repo = new();
    private MicrowaveService Build() => new(_repo.Object);

    [Fact]
    public void StartHeating_SemEntrada_IniciaCom30sEPotencia10()
    {
        var svc = Build();
        var session = svc.StartHeating(null, null);

        Assert.Equal(HeatingStatus.Running, session.Status);
        Assert.Equal(30, session.TotalTimeInSeconds);
        Assert.Equal(10, session.Power);
    }

    [Fact]
    public void StartHeating_EntradaValida_ConfiguraSessionCorretamente()
    {
        var svc = Build();
        var session = svc.StartHeating(60, 5);

        Assert.Equal(HeatingStatus.Running, session.Status);
        Assert.Equal(60, session.TotalTimeInSeconds);
        Assert.Equal(5, session.Power);
    }

    [Fact]
    public void StartHeating_SemPotencia_UsaPotenciaDefault10()
    {
        var svc = Build();
        var session = svc.StartHeating(30, null);
        Assert.Equal(10, session.Power);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(121)]
    public void StartHeating_TempoInvalido_LancaException(int tempo)
    {
        var svc = Build();
        Assert.Throws<MicrowaveBusinessException>(() => svc.StartHeating(tempo, 5));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(11)]
    public void StartHeating_PotenciaInvalida_LancaException(int potencia)
    {
        var svc = Build();
        Assert.Throws<MicrowaveBusinessException>(() => svc.StartHeating(30, potencia));
    }

    [Fact]
    public void StartHeating_ComAquecimentoEmAndamento_Adiciona30Segundos()
    {
        var svc = Build();
        svc.StartHeating(30, 5);
        var session = svc.StartHeating(null, null);

        Assert.Equal(60, session.TotalTimeInSeconds);
        Assert.Equal(HeatingStatus.Running, session.Status);
    }

    [Fact]
    public void PauseOrCancel_AquecendoPrimeiro_Pausa()
    {
        var svc = Build();
        svc.StartHeating(30, 5);
        var session = svc.PauseOrCancel();

        Assert.Equal(HeatingStatus.Paused, session.Status);
    }

    [Fact]
    public void PauseOrCancel_Pausado_CancelaELimpa()
    {
        var svc = Build();
        svc.StartHeating(30, 5);
        svc.PauseOrCancel();
        var session = svc.PauseOrCancel();

        Assert.Equal(HeatingStatus.Idle, session.Status);
        Assert.Equal(0, session.TotalTimeInSeconds);
    }

    [Fact]
    public void StartHeating_ComPausado_Retoma()
    {
        var svc = Build();
        svc.StartHeating(30, 5);
        svc.PauseOrCancel();
        var session = svc.StartHeating(null, null);

        Assert.Equal(HeatingStatus.Running, session.Status);
    }

    [Fact]
    public void Tick_AdicionaCaracteresPorPotenciaEDecrementaTempo()
    {
        var svc = Build();
        svc.StartHeating(10, 3);
        var session = svc.Tick();

        Assert.Equal("...", session.HeatingString);
        Assert.Equal(9, session.RemainingTimeInSeconds);
    }

    [Fact]
    public void Tick_QuandoTempoZera_FinalizaComMensagem()
    {
        var svc = Build();
        svc.StartHeating(1, 1);
        var session = svc.Tick();

        Assert.Equal(HeatingStatus.Completed, session.Status);
        Assert.Contains("Aquecimento concluído", session.HeatingString);
    }

    [Fact]
    public void FormattedRemainingTime_AcimaDeUmMinuto_RetornaMMSS()
    {
        var svc = Build();
        svc.StartHeating(90, 5);
        var session = svc.GetSession();

        Assert.Equal("1:30", session.FormattedRemainingTime);
    }

    [Fact]
    public void FormattedRemainingTime_AbaixoDeUmMinuto_RetornaSegundos()
    {
        var svc = Build();
        svc.StartHeating(45, 5);
        var session = svc.GetSession();

        Assert.Equal("45", session.FormattedRemainingTime);
    }

    [Fact]
    public void StartHeating_ProgramaPreDefinidoEmAndamento_NaoPermiteAdicionarTempo()
    {
        var program = new HeatingProgram { Id = 1, IsCustom = false, TimeInSeconds = 180, Power = 7, HeatingChar = 'p' };
        _repo.Setup(r => r.GetById(1)).Returns(program);

        var svc = Build();
        svc.StartProgram(1);

        Assert.Throws<MicrowaveBusinessException>(() => svc.StartHeating(null, null));
    }
}
