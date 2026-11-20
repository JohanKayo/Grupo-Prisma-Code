using System;
using System.Globalization;

public static class Entrada
{
    public static double LeerDouble(string mensaje, double min = double.MinValue)
    {
        while (true)
        {
            Console.Write(mensaje);
            string input = (Console.ReadLine() ?? "").Replace(',', '.');
            if (double.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out double val) && val >= min)
                return val;
            Console.WriteLine($"Entrada no valida. Ingrese un numero mayor o igual a {min}.");
        }
    }
}
