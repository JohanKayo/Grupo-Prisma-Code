using Prisma.Dominio.Entidades;

namespace Prisma.Aplicacion.Contratos;

public interface IAsistenciaRepositorio
{
    bool Existe(Guid ninoId, DateOnly fecha);
    int ContarDelDia(DateOnly fecha);
    void Agregar(Asistencia asistencia);
}
