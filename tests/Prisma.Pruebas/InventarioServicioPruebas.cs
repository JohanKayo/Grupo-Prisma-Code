using Prisma.Aplicacion.Servicios;
using Prisma.Infraestructura.Memoria;

namespace Prisma.Pruebas;

public class InventarioServicioPruebas
{
    private readonly RelojFalso _reloj = new() { Hoy = new DateOnly(2026, 11, 20) };
    private readonly InventarioServicio _servicio;

    public InventarioServicioPruebas()
    {
        _servicio = new InventarioServicio(new DonativoRepositorioMemoria(), _reloj);
    }

    [Fact]
    public void ObtenerVencidos_DevuelveSoloLosVencidos()  // RN7
    {
        _servicio.RegistrarDonativo("Leche", 10, "litros", new DateOnly(2026, 11, 10));
        _servicio.RegistrarDonativo("Arroz", 25, "kg", new DateOnly(2027, 1, 1));
        _servicio.RegistrarDonativo("Sal", 2, "kg", null);

        var vencidos = _servicio.ObtenerVencidos();

        Assert.Single(vencidos);
        Assert.Equal("Leche", vencidos[0].Producto);
    }

    [Fact]
    public void ObtenerProximosAVencer_DetectaLosQueVencenEnLosProximosDias()
    {
        _servicio.RegistrarDonativo("Yogur", 5, "litros", new DateOnly(2026, 11, 24));
        _servicio.RegistrarDonativo("Arroz", 25, "kg", new DateOnly(2027, 1, 1));

        var proximos = _servicio.ObtenerProximosAVencer(7);

        Assert.Single(proximos);
        Assert.Equal("Yogur", proximos[0].Producto);
    }

    [Fact]
    public void RegistrarDonativo_CantidadCero_SeRechaza()  // RN6
    {
        var r = _servicio.RegistrarDonativo("Arroz", 0, "kg", null);

        Assert.False(r.Exito);
    }

    [Fact]
    public void RegistrarDonativo_SinNombreDeProducto_SeRechaza()
    {
        var r = _servicio.RegistrarDonativo(" ", 3, "kg", null);

        Assert.False(r.Exito);
    }
}
