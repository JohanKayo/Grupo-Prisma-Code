using Prisma.Aplicacion.Contratos;
using Prisma.Dominio.Entidades;

namespace Prisma.Infraestructura.Memoria;

public class AsistenciaRepositorioMemoria : IAsistenciaRepositorio
{
    private readonly List<Asistencia> _asistencias = new();

    public bool Existe(Guid ninoId, DateOnly fecha) =>
        _asistencias.Any(a => a.NinoId == ninoId && a.Fecha == fecha);

    public int ContarDelDia(DateOnly fecha) => _asistencias.Count(a => a.Fecha == fecha);

    public void Agregar(Asistencia asistencia) => _asistencias.Add(asistencia);
}
