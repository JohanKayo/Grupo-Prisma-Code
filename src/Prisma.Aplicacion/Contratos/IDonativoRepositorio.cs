using Prisma.Dominio.Entidades;

namespace Prisma.Aplicacion.Contratos;

public interface IDonativoRepositorio
{
    IReadOnlyList<Donativo> ObtenerTodos();
    void Agregar(Donativo donativo);
}
