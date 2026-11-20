# Grupo Prisma – Sistema de gestión para el Comedor Infantil Santa Ana de Cala Cala

Plataforma de acceso restringido para llevar el registro de niños, asistencia, pagos e inventario de donativos
(ODS 2 – Hambre Cero y ODS 12 – Producción y consumo responsables).

## Arquitectura por capas

| Proyecto | Responsabilidad | Depende de |
| --- | --- | --- |
| `Prisma.Dominio` | Entidades y reglas de negocio (RN1–RN8) | nada |
| `Prisma.Aplicacion` | Casos de uso (servicios) e interfaces (contratos) | Dominio |
| `Prisma.Infraestructura` | Implementaciones: memoria hoy, MySQL/Aiven después | Aplicación, Dominio |
| `Prisma.Web` (pendiente) | Interfaz web y control de acceso | Aplicación, Infraestructura |
| `Prisma.Pruebas` | Pruebas unitarias con xUnit | todos |

Regla: las dependencias apuntan hacia adentro. El Dominio no conoce la base de datos ni la web.

## Módulos y requerimientos

| Módulo | Requerimientos | Estado |
| --- | --- | --- |
| Niños | RF1, RN1 | Base lista |
| Asistencia | RF2, RN2, RN5 | Base lista |
| Inventario y donativos | RF4, RF5, RN6, RN7 | Base lista |
| Pagos | RF3, RN3, RN4, RN5 | Pendiente |
| Usuarios y acceso | RN8, 5.6, 5.7 | Pendiente |

## Cómo trabajar

- Rama `main`: solo versiones que funcionan. Rama `develop`: integración. Cada tarea: `feature/<modulo>`.
- Mensajes de commit: `feat:`, `fix:`, `test:`, `docs:`, `refactor:` + descripción corta.
- Todo cambio entra por Pull Request revisado por otro integrante.
- No subir contraseñas ni cadenas de conexión (Aiven). Usar `dotnet user-secrets` o variables de entorno.

## Ejecutar las pruebas

```
dotnet test
```
