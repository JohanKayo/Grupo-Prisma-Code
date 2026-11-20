using Prisma.Dominio.Entidades;

namespace Prisma.Aplicacion.Contratos;

public interface INinoRepositorio
{
    Nino? ObtenerPorId(Guid id);
    Nino? BuscarIgual(Nino candidato);
    void Agregar(Nino nino);
}
