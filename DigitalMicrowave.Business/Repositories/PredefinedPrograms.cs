using DigitalMicrowave.Business.Models;

namespace DigitalMicrowave.Business.Repositories;

internal static class PredefinedPrograms
{
    public static IReadOnlyList<HeatingProgram> All { get; } =
    [
        new() { Id = 1, Name = "Pipoca", Food = "Pipoca (de micro-ondas)", TimeInSeconds = 180, Power = 7, HeatingChar = 'p',
            Instructions = "Observar o barulho de estouros do milho, caso houver um intervalo de mais de 10 segundos entre um estouro e outro, interrompa o aquecimento." },

        new() { Id = 2, Name = "Leite", Food = "Leite", TimeInSeconds = 300, Power = 5, HeatingChar = 'l',
            Instructions = "Cuidado com aquecimento de líquidos, o choque térmico aliado ao movimento do recipiente pode causar fervura imediata causando risco de queimaduras." },

        new() { Id = 3, Name = "Carnes de boi", Food = "Carne em pedaço ou fatias", TimeInSeconds = 840, Power = 4, HeatingChar = 'c',
            Instructions = "Interrompa o processo na metade e vire o conteúdo com a parte de baixo para cima para o descongelamento uniforme." },

        new() { Id = 4, Name = "Frango", Food = "Frango (qualquer corte)", TimeInSeconds = 480, Power = 7, HeatingChar = 'f',
            Instructions = "Interrompa o processo na metade e vire o conteúdo com a parte de baixo para cima para o descongelamento uniforme." },

        new() { Id = 5, Name = "Feijão", Food = "Feijão congelado", TimeInSeconds = 480, Power = 9, HeatingChar = 'j',
            Instructions = "Deixe o recipiente destampado e em casos de plástico, cuidado ao retirar o recipiente pois o mesmo pode perder resistência em altas temperaturas." }
    ];
}
