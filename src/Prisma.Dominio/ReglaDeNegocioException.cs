namespace Prisma.Dominio;

/// <summary>Se lanza cuando se intenta crear algo que rompe una regla de negocio (RN1–RN8).</summary>
public class ReglaDeNegocioException : Exception
{
    public ReglaDeNegocioException(string mensaje) : base(mensaje) { }
}
