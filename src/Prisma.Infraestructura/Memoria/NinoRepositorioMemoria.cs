using Prisma.Aplicacion.Contratos;
using Prisma.Dominio.Entidades;

namespace Prisma.Infraestructura.Memoria;

/// <summary>Versión en memoria, para pruebas y para avanzar antes de tener la BD lista.</summary>
public class NinoRepositorioMemoria : INinoRepositorio
{
    private readonly List<Nino> _ninos = new();

    public Nino? ObtenerPorId(Guid id) => _ninos.FirstOrDefault(n => n.Id == id);

    public Nino? BuscarIgual(Nino candidato) => _ninos.FirstOrDefault(n => n.EsMismaPersonaQue(candidato));

    public void Agregar(Nino nino) => _ninos.Add(nino);
}
