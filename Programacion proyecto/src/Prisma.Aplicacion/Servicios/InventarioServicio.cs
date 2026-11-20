using Prisma.Aplicacion.Contratos;
using Prisma.Dominio;
using Prisma.Dominio.Entidades;

namespace Prisma.Aplicacion.Servicios;

/// <summary>RF4 + RF5 + RN6 + RN7: registrar donativos y revisar vencimientos.</summary>
public class InventarioServicio
{
    private readonly IDonativoRepositorio _donativos;
    private readonly IRelojSistema _reloj;

    public InventarioServicio(IDonativoRepositorio donativos, IRelojSistema reloj)
    {
        _donativos = donativos;
        _reloj = reloj;
    }

    public Resultado<Donativo> RegistrarDonativo(string producto, decimal cantidad, string unidad,
                                                 DateOnly? fechaVencimiento)
    {
        try
        {
            var donativo = new Donativo(producto, cantidad, unidad, _reloj.Hoy, fechaVencimiento);
            _donativos.Agregar(donativo);
            return Resultado<Donativo>.Ok(donativo);
        }
        catch (ReglaDeNegocioException ex)
        {
            return Resultado<Donativo>.Falla(ex.Message);
        }
    }

    public IReadOnlyList<Donativo> ObtenerVencidos()
    {
        var hoy = _reloj.Hoy;
        return _donativos.ObtenerTodos().Where(d => d.EstaVencido(hoy)).ToList();
    }

    public IReadOnlyList<Donativo> ObtenerProximosAVencer(int dias)
    {
        var hoy = _reloj.Hoy;
        var limite = hoy.AddDays(dias);
        return _donativos.ObtenerTodos()
            .Where(d => d.FechaVencimiento.HasValue
                        && !d.EstaVencido(hoy)
                        && d.FechaVencimiento.Value <= limite)
            .ToList();
    }
}
