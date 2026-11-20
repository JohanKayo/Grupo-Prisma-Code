using System;

public static class ModeloRaciones
{
    // Modelo del vendedor de periodicos: P(D <= Q*) = Cf / (Cf + Cs)
    public static ResultadoRaciones Calcular(double media, double desv, double costoSobrante, double costoFaltante)
    {
        double critica = costoFaltante / (costoFaltante + costoSobrante);
        double z = InversaNormal(critica);
        double q = media + z * desv;
        int redondeado = (int)Math.Round(q, MidpointRounding.AwayFromZero);
        return new ResultadoRaciones(critica, z, q, redondeado);
    }

    private static double CdfNormal(double x) => 0.5 * (1.0 + Erf(x / Math.Sqrt(2.0)));

    // Aproximacion de Abramowitz-Stegun 7.1.26
    private static double Erf(double x)
    {
        double signo = x < 0 ? -1 : 1;
        x = Math.Abs(x);
        double t = 1.0 / (1.0 + 0.3275911 * x);
        double y = 1.0 - (((((1.061405429 * t - 1.453152027) * t) + 1.421413741) * t - 0.284496736) * t + 0.254829592) * t * Math.Exp(-x * x);
        return signo * y;
    }

    private static double InversaNormal(double p)
    {
        double lo = -10, hi = 10;
        for (int i = 0; i < 200; i++)
        {
            double mid = (lo + hi) / 2;
            if (CdfNormal(mid) < p) lo = mid; else hi = mid;
        }
        return (lo + hi) / 2;
    }
}
