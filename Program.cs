while (true)
{
    Pantallas.MostrarMenu();
    string opcion = Console.ReadLine()?.Trim() ?? "";

    if (opcion == "0") break;

    switch (opcion)
    {
        case "1": Pantallas.Modelo1MenuSemanal(); break;
        case "2": Pantallas.Modelo2Raciones(); break;
        case "3": Pantallas.Modelo3Cola(); break;
        default: Console.WriteLine("Opcion no valida. Intente de nuevo."); break;
    }
}
