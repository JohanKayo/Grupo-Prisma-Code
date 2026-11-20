using System;
using System.Collections.Generic;
using System.Linq;

public static class PlanificadorMenu
{
    public static DatosMenu DatosEjemplo() => new DatosMenu
    {
        Dias = new[] { 1, 2, 3, 4, 5 },
        Lotes = new List<LoteMenu>
        {
            new("Arroz",     Grupo.Carbohidrato, 120, 4),
            new("Papa",      Grupo.Carbohidrato,  60, 2),
            new("Pollo",     Grupo.Proteina,      50, 1),
            new("Carne",     Grupo.Proteina,      40, 3),
            new("Zanahoria", Grupo.Verdura,       30, 2),
            new("Tomate",    Grupo.Verdura,       25, 0),
            new("Cebolla",   Grupo.Verdura,       40, 4)
        },
        PrecioCompra = new Dictionary<Grupo, double>
        {
            [Grupo.Carbohidrato] = 8,
            [Grupo.Proteina] = 28,
            [Grupo.Verdura] = 6
        },
        DemandaDiariaKg = new Dictionary<Grupo, double>
        {
            [Grupo.Carbohidrato] = 40,
            [Grupo.Proteina] = 25,
            [Grupo.Verdura] = 30
        }
    };

    public static ResultadoMenu Resolver(DatosMenu d)
    {
        int nL = d.Lotes.Count;
        int nD = d.Dias.Length;
        var grupos = Enum.GetValues<Grupo>();
        int nG = grupos.Length;

        var idxX = new int[nL, nD];
        var idxB = new int[nG, nD];
        int n = 0;

        for (int i = 0; i < nL; i++)
            for (int p = 0; p < nD; p++)
                idxX[i, p] = p <= d.Lotes[i].UltimoDiaUso ? n++ : -1;

        for (int g = 0; g < nG; g++)
            for (int p = 0; p < nD; p++)
                idxB[g, p] = n++;

        var costos = new double[n];
        for (int g = 0; g < nG; g++)
            for (int p = 0; p < nD; p++)
                costos[idxB[g, p]] = d.PrecioCompra[grupos[g]];

        var a = new List<double[]>();
        var sentidos = new List<Sentido>();
        var b = new List<double>();

        // Demanda diaria por grupo
        for (int g = 0; g < nG; g++)
        {
            for (int p = 0; p < nD; p++)
            {
                var fila = new double[n];
                for (int i = 0; i < nL; i++)
                    if (d.Lotes[i].Grupo == grupos[g] && idxX[i, p] >= 0)
                        fila[idxX[i, p]] = 1;
                fila[idxB[g, p]] = 1;
                a.Add(fila);
                sentidos.Add(Sentido.MayorIgual);
                b.Add(d.DemandaDiariaKg[grupos[g]]);
            }
        }

        // Limite de stock por lote
        for (int i = 0; i < nL; i++)
        {
            var fila = new double[n];
            for (int p = 0; p < nD; p++)
                if (idxX[i, p] >= 0) fila[idxX[i, p]] = 1;
            a.Add(fila);
            sentidos.Add(Sentido.MenorIgual);
            b.Add(d.Lotes[i].StockKg);
        }

        var lp = Simplex.Minimizar(costos, a, sentidos, b);
        if (!lp.Factible || !lp.Acotado)
            return new ResultadoMenu { Factible = false };

        var uso = new double[nL, nD];
        for (int i = 0; i < nL; i++)
            for (int p = 0; p < nD; p++)
                if (idxX[i, p] >= 0) uso[i, p] = lp.X[idxX[i, p]];

        var compra = new Dictionary<Grupo, double>();
        for (int g = 0; g < nG; g++)
        {
            double total = 0;
            for (int p = 0; p < nD; p++) total += lp.X[idxB[g, p]];
            compra[grupos[g]] = total;
        }

        double costoCompras = compra.Sum(kv => kv.Value * d.PrecioCompra[kv.Key]);

        double merma = 0;
        for (int i = 0; i < nL; i++)
        {
            if (d.Lotes[i].UltimoDiaUso >= nD - 1) continue;
            double usado = 0;
            for (int p = 0; p < nD; p++) usado += uso[i, p];
            merma += Math.Max(0, d.Lotes[i].StockKg - usado);
        }

        return new ResultadoMenu
        {
            Factible = true,
            CostoTotal = lp.Z,
            UsoKg = uso,
            CompraKg = compra,
            CostoCompras = costoCompras,
            MermaKg = merma
        };
    }
}
