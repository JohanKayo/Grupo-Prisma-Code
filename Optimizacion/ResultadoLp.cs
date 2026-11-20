using System;

public sealed class ResultadoLp
{
    public bool Factible { get; init; }
    public bool Acotado { get; init; } = true;
    public double Z { get; init; }
    public double[] X { get; init; } = Array.Empty<double>();
}
