namespace Prisma.Aplicacion.Contratos;

/// <summary>Da la fecha de hoy. Existe para poder probar reglas que dependen de la fecha.</summary>
public interface IRelojSistema
{
    DateOnly Hoy { get; }
}
