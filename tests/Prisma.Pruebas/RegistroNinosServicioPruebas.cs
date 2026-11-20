using Prisma.Aplicacion.Servicios;
using Prisma.Infraestructura.Memoria;

namespace Prisma.Pruebas;

public class RegistroNinosServicioPruebas
{
    private readonly RegistroNinosServicio _servicio = new(new NinoRepositorioMemoria());

    [Fact]
    public void Registrar_NinoNuevo_TieneExito()
    {
        var r = _servicio.Registrar("Ana", "Pérez", new DateOnly(2012, 5, 3));

        Assert.True(r.Exito);
        Assert.NotNull(r.Valor);
    }

    [Fact]
    public void Registrar_MismaPersonaConOtraEscritura_SeRechaza()  // RN1
    {
        _servicio.Registrar("Ana", "Pérez", new DateOnly(2012, 5, 3));

        var r = _servicio.Registrar("  ana ", "PEREZ", new DateOnly(2012, 5, 3));

        Assert.False(r.Exito);
    }

    [Fact]
    public void Registrar_HomonimoConOtraFechaDeNacimiento_SePermite()
    {
        _servicio.Registrar("Ana", "Pérez", new DateOnly(2012, 5, 3));

        var r = _servicio.Registrar("Ana", "Pérez", new DateOnly(2013, 8, 21));

        Assert.True(r.Exito);
    }

    [Fact]
    public void Registrar_SinNombres_SeRechaza()
    {
        var r = _servicio.Registrar("   ", "Pérez", new DateOnly(2012, 5, 3));

        Assert.False(r.Exito);
    }
}
