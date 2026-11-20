using System.Collections.Generic;

public sealed class DatosMenu
{
    public required int[] Dias { get; init; }
    public required List<LoteMenu> Lotes { get; init; }
    public required Dictionary<Grupo, double> PrecioCompra { get; init; }
    public required Dictionary<Grupo, double> DemandaDiariaKg { get; init; }
}
