using System;
using System.Linq;

public static class Pantallas
{
    public static void MostrarMenu()
    {
        Console.WriteLine("\n==========================================");
        Console.WriteLine(" SELECCIONE EL MODELO A CALCULAR:");
        Console.WriteLine(" 1. Menu semanal de costo minimo (Datos de ejemplo)");
        Console.WriteLine(" 2. Raciones a preparar (Ingresar datos)");
        Console.WriteLine(" 3. Fila de ingreso M/M/1 (Ingresar datos)");
        Console.WriteLine(" 0. Salir");
        Console.WriteLine("==========================================");
        Console.Write("Opcion: ");
    }

    public static void Modelo1MenuSemanal()
    {
        string[] nombresDias = { "Lun", "Mar", "Mie", "Jue", "Vie" };
        Console.WriteLine("\n=== MODELO 1: menu semanal de costo minimo con vencimientos ===");
        var datos = PlanificadorMenu.DatosEjemplo();
        var menu = PlanificadorMenu.Resolver(datos);

        if (!menu.Factible)
        {
            Console.WriteLine("El modelo no tiene solucion con estos datos (revisar restricciones).");
            return;
        }

        Console.WriteLine($"Costo total (funcion objetivo): {menu.CostoTotal:F1} Bs");
        Console.WriteLine();
        Console.WriteLine("Uso de cada producto por dia (kg):");
        Console.WriteLine($"{"Producto",-14}" + string.Concat(nombresDias.Select(d => $"{d,7}")) + $"{"Usado",8}{"Sobra",8}");

        for (int i = 0; i < datos.Lotes.Count; i++)
        {
            double usado = 0;
            string fila = $"{datos.Lotes[i].Nombre,-14}";
            for (int p = 0; p < datos.Dias.Length; p++)
            {
                fila += $"{menu.UsoKg[i, p],7:F1}";
                usado += menu.UsoKg[i, p];
            }
            fila += $"{usado,8:F1}{datos.Lotes[i].StockKg - usado,8:F1}";
            Console.WriteLine(fila);
        }

        Console.WriteLine();
        Console.WriteLine("Compras necesarias:");
        foreach (var (grupo, kg) in menu.CompraKg)
            Console.WriteLine($" {grupo,-14}: {kg,6:F1} kg -> {kg * datos.PrecioCompra[grupo],7:F1} Bs");

        Console.WriteLine($"Compras totales: {menu.CostoCompras:F1} Bs | Merma (se vence sin usar): {menu.MermaKg:F1} kg");
    }

    public static void Modelo2Raciones()
    {
        Console.WriteLine("\n=== MODELO 2: raciones a preparar ===");
        double media = Entrada.LeerDouble("Ingrese demanda media (ej. 220): ", min: 0.01);
        double desv = Entrada.LeerDouble("Ingrese desviacion estandar (ej. 25): ", min: 0.01);
        double cSob = Entrada.LeerDouble("Ingrese costo por sobrante en Bs (ej. 9): ", min: 0.01);
        double cFal = Entrada.LeerDouble("Ingrese costo por faltante en Bs (ej. 15): ", min: 0.01);

        var raciones = ModeloRaciones.Calcular(media, desv, cSob, cFal);
        Console.WriteLine($"\nProporcion critica: {raciones.ProporcionCritica:F3}");
        Console.WriteLine($"z = {raciones.Z:F2} -> Q* = {raciones.RacionesOptimas:F1} -> preparar {raciones.RacionesRedondeadas} raciones");
    }

    public static void Modelo3Cola()
    {
        Console.WriteLine("\n=== MODELO 3: fila de ingreso (M/M/1) ===");
        double llegadas = Entrada.LeerDouble("Ingrese llegadas por minuto (ej. 2): ", min: 0.01);
        double segundos = Entrada.LeerDouble("Ingrese segundos de atencion por nino (ej. 25): ", min: 0.01);

        var res = ColaIngreso.Calcular(llegadas, segundos);
        if (!res.Estable)
            Console.WriteLine("\nLa fila colapsa (llegan mas ninos de los que se pueden atender).");
        else
            Console.WriteLine($"\nOcupacion: {res.Rho:P0}, espera: {res.EsperaMinutos * 60:F0} s, niños en fila: {res.NinosEnFila:F1}");
    }
}
