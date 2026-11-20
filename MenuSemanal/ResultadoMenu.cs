using System.Collections.Generic;

public sealed class ResultadoMenu
{
    public bool Factible { get; init; }
    public double CostoTotal { get; init; }
    public double[,] UsoKg { get; init; } = new double[0, 0];
    public Dictionary<Grupo, double> CompraKg { get; init; } = new();
    public double CostoCompras { get; init; }
    public double MermaKg { get; init; }
}
