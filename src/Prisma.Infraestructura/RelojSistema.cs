using Prisma.Aplicacion.Contratos;

namespace Prisma.Infraestructura;

public class RelojSistema : IRelojSistema
{
    public DateOnly Hoy => DateOnly.FromDateTime(DateTime.Now);
}
