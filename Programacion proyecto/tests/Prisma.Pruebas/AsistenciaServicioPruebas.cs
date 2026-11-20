using Prisma.Aplicacion.Servicios;
using Prisma.Dominio.Entidades;
using Prisma.Infraestructura.Memoria;

namespace Prisma.Pruebas;

public class AsistenciaServicioPruebas
{
    private readonly NinoRepositorioMemoria _ninos = new();
    private readonly AsistenciaRepositorioMemoria _asistencias = new();
    private readonly RelojFalso _reloj = new() { Hoy = new DateOnly(2026, 11, 20) };
    private readonly AsistenciaServicio _servicio;

    public AsistenciaServicioPruebas()
    {
        _servicio = new AsistenciaServicio(_ninos, _asistencias, _reloj);
    }

    private Guid CrearNino()
    {
        var nino = new Nino("Ana", "Pérez", new DateOnly(2012, 5, 3));
        _ninos.Agregar(nino);
        return nino.Id;
    }

    [Fact]
    public void RegistrarHoy_NinoRegistrado_TieneExito()
    {
        var r = _servicio.RegistrarHoy(CrearNino());

        Assert.True(r.Exito);
        Assert.Equal(1, _servicio.CantidadDeHoy());
    }

    [Fact]
    public void RegistrarHoy_DosVecesElMismoDia_SeRechazaLaSegunda()  // RN2
    {
        var id = CrearNino();
        _servicio.RegistrarHoy(id);

        var r = _servicio.RegistrarHoy(id);

        Assert.False(r.Exito);
        Assert.Equal(1, _servicio.CantidadDeHoy());
    }

    [Fact]
    public void RegistrarHoy_AlDiaSiguiente_SePermite()
    {
        var id = CrearNino();
        _servicio.RegistrarHoy(id);
        _reloj.Hoy = _reloj.Hoy.AddDays(1);

        var r = _servicio.RegistrarHoy(id);

        Assert.True(r.Exito);
    }

    [Fact]
    public void RegistrarHoy_NinoNoRegistrado_SeRechaza()
    {
        var r = _servicio.RegistrarHoy(Guid.NewGuid());

        Assert.False(r.Exito);
    }
}
