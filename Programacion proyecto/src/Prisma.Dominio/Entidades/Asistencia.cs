namespace Prisma.Dominio.Entidades;

/// <summary>RN2: la asistencia se registra con la fecha del día de atención.</summary>
public record Asistencia(Guid NinoId, DateOnly Fecha);
