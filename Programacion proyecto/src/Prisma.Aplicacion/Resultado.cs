namespace Prisma.Aplicacion;

/// <summary>Resultado simple de una operación: éxito con valor, o fallo con un mensaje legible.</summary>
public sealed class Resultado<T>
{
    public bool Exito { get; }
    public T? Valor { get; }
    public string? Error { get; }

    private Resultado(bool exito, T? valor, string? error)
    {
        Exito = exito;
        Valor = valor;
        Error = error;
    }

    public static Resultado<T> Ok(T valor) => new(true, valor, null);
    public static Resultado<T> Falla(string error) => new(false, default, error);
}
