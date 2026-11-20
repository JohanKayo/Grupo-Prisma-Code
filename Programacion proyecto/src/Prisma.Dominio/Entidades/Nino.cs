using System.Globalization;
using System.Text;

namespace Prisma.Dominio.Entidades;

/// <summary>
/// Niño o adolescente que asiste al comedor.
/// Solo guarda los datos mínimos (principio de minimización de la ética de datos).
/// </summary>
public class Nino
{
    public Guid Id { get; }
    public string Nombres { get; }
    public string Apellidos { get; }
    public DateOnly FechaNacimiento { get; }

    // Constructor para registrar un niño nuevo.
    public Nino(string nombres, string apellidos, DateOnly fechaNacimiento)
        : this(Guid.NewGuid(), nombres, apellidos, fechaNacimiento) { }

    // Constructor para reconstruir un niño que ya existe (por ejemplo, leído de MySQL).
    public Nino(Guid id, string nombres, string apellidos, DateOnly fechaNacimiento)
    {
        if (string.IsNullOrWhiteSpace(nombres))
            throw new ReglaDeNegocioException("Los nombres son obligatorios.");
        if (string.IsNullOrWhiteSpace(apellidos))
            throw new ReglaDeNegocioException("Los apellidos son obligatorios.");

        Id = id;
        Nombres = nombres.Trim();
        Apellidos = apellidos.Trim();
        FechaNacimiento = fechaNacimiento;
    }

    /// <summary>
    /// RN1: un niño tiene un registro único. Se considera la misma persona si coinciden
    /// nombres, apellidos y fecha de nacimiento (sin importar mayúsculas ni tildes).
    /// Se usa la fecha de nacimiento para no mezclar a dos niños homónimos.
    /// </summary>
    public bool EsMismaPersonaQue(Nino otro) =>
        Normalizar(Nombres) == Normalizar(otro.Nombres)
        && Normalizar(Apellidos) == Normalizar(otro.Apellidos)
        && FechaNacimiento == otro.FechaNacimiento;

    private static string Normalizar(string texto)
    {
        var descompuesto = texto.Trim().ToUpperInvariant().Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();
        foreach (var c in descompuesto)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }
        return sb.ToString().Normalize(NormalizationForm.FormC);
    }
}
