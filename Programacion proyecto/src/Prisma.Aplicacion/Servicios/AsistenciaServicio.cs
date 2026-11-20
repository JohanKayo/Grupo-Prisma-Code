using Prisma.Aplicacion.Contratos;
using Prisma.Dominio.Entidades;

namespace Prisma.Aplicacion.Servicios;

/// <summary>RF2 + RN2 + RN5: registrar la asistencia del día (no depende de que el pago esté al día).</summary>
public class AsistenciaServicio
{
    private readonly INinoRepositorio _ninos;
    private readonly IAsistenciaRepositorio _asistencias;
    private readonly IRelojSistema _reloj;

    public AsistenciaServicio(INinoRepositorio ninos, IAsistenciaRepositorio asistencias, IRelojSistema reloj)
    {
        _ninos = ninos;
        _asistencias = asistencias;
        _reloj = reloj;
    }

    public Resultado<Asistencia> RegistrarHoy(Guid ninoId)
    {
        if (_ninos.ObtenerPorId(ninoId) is null)
            return Resultado<Asistencia>.Falla("El niño no está registrado.");

        var hoy = _reloj.Hoy;
        if (_asistencias.Existe(ninoId, hoy))
            return Resultado<Asistencia>.Falla("La asistencia de hoy ya fue registrada.");

        var asistencia = new Asistencia(ninoId, hoy);
        _asistencias.Agregar(asistencia);
        return Resultado<Asistencia>.Ok(asistencia);
    }

    /// <summary>Cuántos niños comieron hoy (sirve para planificar raciones, ODS 2).</summary>
    public int CantidadDeHoy() => _asistencias.ContarDelDia(_reloj.Hoy);
}
