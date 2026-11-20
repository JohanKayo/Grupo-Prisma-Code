using Prisma.Aplicacion.Contratos;

namespace Prisma.Pruebas;

/// <summary>Reloj controlable para probar fechas sin depender del día real.</summary>
public class RelojFalso : IRelojSistema
{
    public DateOnly Hoy { get; set; }
}
