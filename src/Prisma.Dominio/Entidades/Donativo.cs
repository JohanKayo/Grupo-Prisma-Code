namespace Prisma.Dominio.Entidades;

/// <summary>RN6: cada donativo registra producto, cantidad y fecha de vencimiento (cuando corresponde).</summary>
public class Donativo
{
    public Guid Id { get; }
    public string Producto { get; }
    public decimal Cantidad { get; }
    public string Unidad { get; }
    public DateOnly FechaIngreso { get; }
    public DateOnly? FechaVencimiento { get; }

    public Donativo(string producto, decimal cantidad, string unidad,
                    DateOnly fechaIngreso, DateOnly? fechaVencimiento)
    {
        if (string.IsNullOrWhiteSpace(producto))
            throw new ReglaDeNegocioException("El nombre del producto es obligatorio.");
        if (cantidad <= 0)
            throw new ReglaDeNegocioException("La cantidad debe ser mayor que cero.");

        Id = Guid.NewGuid();
        Producto = producto.Trim();
        Cantidad = cantidad;
        Unidad = string.IsNullOrWhiteSpace(unidad) ? "unidades" : unidad.Trim();
        FechaIngreso = fechaIngreso;
        FechaVencimiento = fechaVencimiento;
    }

    /// <summary>RN7: los productos vencidos deben poder identificarse.</summary>
    public bool EstaVencido(DateOnly hoy) =>
        FechaVencimiento.HasValue && FechaVencimiento.Value < hoy;
}
