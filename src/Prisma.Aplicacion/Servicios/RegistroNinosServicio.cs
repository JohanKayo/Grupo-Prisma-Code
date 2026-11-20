using Prisma.Aplicacion.Contratos;
using Prisma.Dominio;
using Prisma.Dominio.Entidades;

namespace Prisma.Aplicacion.Servicios;

/// <summary>RF1 + RN1: registrar niños sin crear duplicados.</summary>
public class RegistroNinosServicio
{
    private readonly INinoRepositorio _ninos;

    public RegistroNinosServicio(INinoRepositorio ninos)
    {
        _ninos = ninos;
    }

    public Resultado<Nino> Registrar(string nombres, string apellidos, DateOnly fechaNacimiento)
    {
        Nino candidato;
        try
        {
            candidato = new Nino(nombres, apellidos, fechaNacimiento);
        }
        catch (ReglaDeNegocioException ex)
        {
            return Resultado<Nino>.Falla(ex.Message);
        }

        if (_ninos.BuscarIgual(candidato) is not null)
            return Resultado<Nino>.Falla("Este niño ya está registrado. Búsquelo en la lista en vez de crearlo de nuevo.");

        _ninos.Agregar(candidato);
        return Resultado<Nino>.Ok(candidato);
    }
}
