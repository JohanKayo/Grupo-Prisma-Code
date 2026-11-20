using Prisma.Aplicacion.Contratos;
using Prisma.Dominio.Entidades;

namespace Prisma.Infraestructura.Memoria;

public class DonativoRepositorioMemoria : IDonativoRepositorio
{
    private readonly List<Donativo> _donativos = new();

    public IReadOnlyList<Donativo> ObtenerTodos() => _donativos.ToList();

    public void Agregar(Donativo donativo) => _donativos.Add(donativo);
}
