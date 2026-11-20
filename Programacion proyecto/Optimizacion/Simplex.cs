using System;
using System.Collections.Generic;
using System.Linq;

public static class Simplex
{
    private const double Eps = 1e-9;

    public static ResultadoLp Minimizar(
        double[] c,
        IReadOnlyList<double[]> a,
        IReadOnlyList<Sentido> sentidos,
        IReadOnlyList<double> b)
    {
        int m = a.Count;
        int n = c.Length;
        var filas = new double[m][];
        var rhs = new double[m];
        var s = new Sentido[m];

        for (int i = 0; i < m; i++)
        {
            filas[i] = (double[])a[i].Clone();
            rhs[i] = b[i];
            s[i] = sentidos[i];

            if (rhs[i] < 0)
            {
                for (int j = 0; j < n; j++) filas[i][j] = -filas[i][j];
                rhs[i] = -rhs[i];
                s[i] = s[i] switch
                {
                    Sentido.MenorIgual => Sentido.MayorIgual,
                    Sentido.MayorIgual => Sentido.MenorIgual,
                    _ => Sentido.Igual
                };
            }
        }

        int nHolgura = s.Count(q => q != Sentido.Igual);
        int nArtificial = s.Count(q => q != Sentido.MenorIgual);
        int cols = n + nHolgura + nArtificial;
        int primeraArt = n + nHolgura;

        var T = new double[m + 1, cols + 1];
        var basis = new int[m];
        int colH = n;
        int colA = primeraArt;

        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++) T[i, j] = filas[i][j];
            T[i, cols] = rhs[i];

            switch (s[i])
            {
                case Sentido.MenorIgual:
                    T[i, colH] = 1;
                    basis[i] = colH;
                    colH++;
                    break;
                case Sentido.MayorIgual:
                    T[i, colH] = -1;
                    colH++;
                    T[i, colA] = 1;
                    basis[i] = colA;
                    colA++;
                    break;
                default:
                    T[i, colA] = 1;
                    basis[i] = colA;
                    colA++;
                    break;
            }
        }

        void PrepararFilaZ(double[] costos)
        {
            for (int j = 0; j < cols; j++) T[m, j] = costos[j];
            T[m, cols] = 0.0;

            for (int i = 0; i < m; i++)
            {
                double cb = costos[basis[i]];
                if (cb != 0.0)
                    for (int j = 0; j <= cols; j++) T[m, j] -= cb * T[i, j];
            }
        }

        void Pivotar(int fila, int col)
        {
            double p = T[fila, col];
            for (int j = 0; j <= cols; j++) T[fila, j] /= p;

            for (int i = 0; i <= m; i++)
            {
                if (i == fila) continue;
                double f = T[i, col];
                if (f != 0.0)
                    for (int j = 0; j <= cols; j++) T[i, j] -= f * T[fila, j];
            }
            basis[fila] = col;
        }

        bool Iterar(int limiteColumnas)
        {
            for (int it = 0; it < 20000; it++)
            {
                int entra = -1;
                for (int j = 0; j < limiteColumnas; j++)
                {
                    if (T[m, j] < -Eps)
                    {
                        entra = j;
                        break;
                    }
                }
                if (entra < 0) return true;

                int sale = -1;
                double mejor = 0.0;
                for (int i = 0; i < m; i++)
                {
                    if (T[i, entra] > Eps)
                    {
                        double razon = T[i, cols] / T[i, entra];
                        if (sale < 0 || razon < mejor - 1e-12 || (Math.Abs(razon - mejor) <= 1e-12 && basis[i] < basis[sale]))
                        {
                            sale = i;
                            mejor = razon;
                        }
                    }
                }
                if (sale < 0) return false;
                Pivotar(sale, entra);
            }
            return false;
        }

        if (nArtificial > 0)
        {
            var c1 = new double[cols];
            for (int j = primeraArt; j < cols; j++) c1[j] = 1.0;
            PrepararFilaZ(c1);
            Iterar(cols);

            if (-T[m, cols] > 1e-7) return new ResultadoLp { Factible = false };

            for (int i = 0; i < m; i++)
            {
                if (basis[i] < primeraArt) continue;
                for (int j = 0; j < primeraArt; j++)
                {
                    if (Math.Abs(T[i, j]) > Eps)
                    {
                        Pivotar(i, j);
                        break;
                    }
                }
            }
        }

        var c2 = new double[cols];
        for (int j = 0; j < n; j++) c2[j] = c[j];
        PrepararFilaZ(c2);
        bool acotado = Iterar(primeraArt);

        var x = new double[n];
        for (int i = 0; i < m; i++)
            if (basis[i] < n) x[basis[i]] = T[i, cols];

        double z = 0.0;
        for (int j = 0; j < n; j++) z += c[j] * x[j];

        return new ResultadoLp { Factible = true, Acotado = acotado, Z = z, X = x };
    }
}
