# SGCAE — Sistema de Gestión y Control de Acceso de Empleados

Backend (ASP.NET Core Web API, .NET 10) para administrar empleados, sus credenciales, los puntos de acceso físicos y el registro de entradas y salidas de una empresa de servicios logísticos y de almacenamiento.

Construido con **Clean Architecture**, **DDD** y **CQRS** (mediator propio), persistencia con **Entity Framework Core sobre SQL Server**.

---

## 1. Arquitectura

```
AccessControl.slnx
├── Core/
│   ├── AccessControl.Domain        → Entidades, objetos de valor, reglas e invariantes (no depende de nada)
│   └── AccessControl.Application   → Casos de uso (Commands / Queries), contratos, validaciones, Result, Mediator
├── Infrastructure/
│   └── AccessControl.Persistence   → EF Core, DataContext, configuraciones, repositorios, UnitOfWork, seeders
├── Presenters/
│   └── AccessControl.Api           → Controladores REST, DTOs de entrada, ApiResponse, middleware
├── Tests/
│   └── AccessControl.Tests         → Pruebas unitarias del dominio (MSTest)
└── scripts/ef.cmd                  → Atajo para migraciones
```

Regla de dependencias (hacia adentro):

```
Api ──► Application ──► Domain
 │            ▲
 └──► Persistence ──┘   (Persistence implementa los contratos de Application)
```

- **Domain** no conoce EF Core ni ASP.NET.
- **Application** define `IRepository<T>`, `IEmployeesRepository`, `IAccessPointsRepository`, `IAccessRecordsRepository` e `IUnitOfWork`; **Persistence** los implementa (inversión de dependencias).
- **Api** solo traduce HTTP ⇄ Command/Query y envía al `IMediator`.

## 2. Modelo de dominio (DDD)

| Elemento | Tipo | Descripción |
|---|---|---|
| `Employee` | Entidad / **raíz de agregado** | Identificación única, nombres, apellidos, correo, cargo, área y estado laboral. Es dueño de sus credenciales. |
| `Credential` | Entidad dentro del agregado `Employee` | Código único y estado (`Active`, `Blocked`, `Revoked`). Solo se crea y modifica a través de `Employee`. |
| `AccessPoint` | Entidad / raíz de agregado | Recepción, parqueadero, bodega, área restringida… con indicador `IsEnabled`. |
| `AccessRecord` | Entidad / raíz de agregado | Movimiento de entrada o salida, **autorizado o rechazado**, con motivo de rechazo. |
| `Email` | Objeto de valor | Formato válido, normalizado en minúsculas. |
| `CredentialCode` | Objeto de valor | 6–32 caracteres (letras, números y guiones), normalizado en mayúsculas. |
| `OrganizationalArea` | Objeto de valor (catálogo cerrado) | `ADMINISTRATION`, `OPERATIONS`, `WAREHOUSE`, `LOGISTICS`, `SECURITY`, `HUMAN_RESOURCES`, `TECHNOLOGY`. |
| `EmploymentStatus`, `CredentialStatus`, `AccessPointType`, `MovementType`, `AccessResult`, `RejectionReason` | Enumeraciones del dominio | |

### Reglas e invariantes

| Regla | Dónde se garantiza |
|---|---|
| Identificación y correo únicos | Caso de uso (consulta al repositorio) + índice único en BD |
| Código de credencial único en toda la organización | Caso de uso + índice único en BD |
| No se asignan credenciales a empleados inactivos | `Employee.AssignCredential` |
| Un empleado tiene como máximo **una** credencial no revocada | `Employee.AssignCredential` |
| Una credencial revocada no puede bloquearse ni desbloquearse (la revocación es definitiva) | `Credential.Block / Unblock / Revoke` |
| Solo se desbloquea una credencial bloqueada | `Credential.Unblock` |
| Un empleado inactivo no puede desactivarse de nuevo | `Employee.Deactivate` |
| El acceso se rechaza si: el punto no existe o está deshabilitado, la credencial no existe, está bloqueada o revocada, o el empleado está inactivo | `AccessRecord.Register` |
| No se puede **entrar dos veces sin salir** ni **salir sin haber entrado** (evita registros duplicados) | `AccessRecord.Register` usando el último movimiento autorizado |
| Todo intento de acceso queda registrado, incluso los rechazados | `RegisterAccessUseCase` |

## 3. CQRS

Cada caso de uso es un `IRequest<Result<T>>` con su `IRequestHandler` (UseCase), despachado por `SimpleMediator`, que además ejecuta el validador de FluentValidation correspondiente antes del handler.

| Requerimiento | Commands (modifican) | Queries (consultan) |
|---|---|---|
| RF-01 Empleados | `CreateEmployee`, `UpdateEmployee`, `DeactivateEmployee` | `GetEmployeesList` (paginada), `GetEmployeeById`, `GetAreasList` |
| RF-02 Credenciales | `AssignCredential`, `BlockCredential`, `UnblockCredential`, `RevokeCredential` | `GetCredentialByCode` |
| RF-03 Entradas/salidas | `RegisterAccess` | — |
| RF-04 Historial | — | `GetAccessHistory` (paginada, con filtros) |
| RF-06 Puntos de acceso | `CreateAccessPoint`, `UpdateAccessPoint` | `GetAccessPointsList` (paginada), `GetAccessPointById` |

Los commands usan `IUnitOfWork.CommitAsync()`; las queries usan `AsNoTracking()` y devuelven DTOs propios.

Manejo de errores: `BaseHandler` convierte excepciones en `Result` (`Validation` / `Business` → 400, `NotFound` → 404, `Unexpected` → 500) y `ApiController` las traduce a `ApiResponse<T>`. El `ExceptionHandlingMiddleware` cubre las validaciones que el mediator lanza antes de llegar al handler.

## 4. Endpoints

Todas las respuestas tienen el formato:

```json
{ "isSuccess": true, "data": { }, "message": null, "errors": null }
```

| Método | Ruta | RF | Descripción |
|---|---|---|---|
| GET | `/api/areas` | 01 | Catálogo de áreas organizacionales |
| GET | `/api/employees?pageNumber&pageSize&status&areaCode` | 01 | Lista paginada de empleados |
| GET | `/api/employees/{id}` | 01 | Detalle del empleado con sus credenciales |
| POST | `/api/employees` | 01 | Registrar empleado |
| PUT | `/api/employees` | 01 | Actualizar empleado |
| PUT | `/api/employees/{id}/deactivate` | 01 | Desactivar empleado |
| POST | `/api/credentials` | 02 | Asignar credencial a un empleado |
| GET | `/api/credentials/{code}` | 02 | Estado de la credencial y si puede usarse (`canBeUsed`) |
| PUT | `/api/credentials/{code}/block` | 02 | Bloquear credencial |
| PUT | `/api/credentials/{code}/unblock` | 02 | Desbloquear credencial |
| PUT | `/api/credentials/{code}/revoke` | 02 | Revocar credencial (definitivo) |
| POST | `/api/accessrecords` | 03 | Registrar entrada/salida (autorizada o rechazada) |
| GET | `/api/accessrecords?employeeId&from&to&accessPointId&result&pageNumber&pageSize` | 04 | Historial paginado y filtrado |
| GET | `/api/accesspoints?isEnabled&type&pageNumber&pageSize` | 06 | Lista paginada de puntos de acceso |
| GET | `/api/accesspoints/{id}` | 06 | Detalle de un punto de acceso |
| POST | `/api/accesspoints` | 06 | Registrar punto de acceso |
| PUT | `/api/accesspoints` | 06 | Actualizar / habilitar / deshabilitar punto de acceso |

Valores numéricos de entrada:

- `status` (empleado): `1` Active, `2` Inactive
- `type` (punto de acceso): `1` Reception, `2` Parking, `3` Warehouse, `4` RestrictedArea, `99` Other
- `movementType`: `1` Entry, `2` Exit
- `result` (historial): `1` Authorized, `2` Rejected

Ejemplo RF-03:

```http
POST /api/accessrecords
{ "credentialCode": "CRED-0001", "accessPointId": "<id>", "movementType": 1 }
```

```json
{
  "isSuccess": true,
  "data": {
    "accessRecordId": "…",
    "isAuthorized": false,
    "result": "Rejected",
    "rejectionReason": "EmployeeAlreadyInside",
    "movementType": "Entry",
    "employeeId": "…",
    "accessPointId": "…",
    "occurredAt": "2026-10-27T13:05:12Z"
  }
}
```

La documentación OpenAPI queda publicada en `/openapi/v1.json` (entorno Development).

## 5. Cómo ejecutar

Requisitos: .NET 10 SDK, SQL Server (Express o instancia local) y la herramienta `dotnet-ef`.

1. Ajustar la cadena de conexión `MyConnection` en `AccessControl.Api/appsettings.json`:

   ```json
   "MyConnection": "Server=.\\SQLEXPRESS;Database=AccessControlDb;Trusted_Connection=True;TrustServerCertificate=True;"
   ```

2. Crear y aplicar la migración (desde la raíz del repositorio):

   ```cmd
   ef.cmd add InitialCreate
   ef.cmd update
   ```

3. Ejecutar la API:

   ```cmd
   dotnet run --project AccessControl.Api --launch-profile http
   ```

   Al iniciar, los seeders cargan datos de prueba:

   | Dato | Estado |
   |---|---|
   | Recepción principal, Parqueadero, Bodega central, Cuarto de servidores | Habilitados |
   | Bodega norte | Deshabilitado |
   | Ana Gómez — `CRED-0001` | Empleada activa, credencial activa |
   | Carlos Pérez — `CRED-0002` | Empleado activo, credencial **bloqueada** |
   | Laura Restrepo — `CRED-0003` | Empleada **inactiva** |

## 6. Evidencias

- **Postman**: importar `docs/SGCAE.postman_collection.json` y ejecutar la colección completa (*Run collection*) sobre una base de datos recién creada. Recorre todos los RF con casos exitosos y de error, y cada request tiene su prueba automática.
- **Pruebas unitarias del dominio**: `dotnet test` ejecuta `AccessControl.Tests` (invariantes de empleado, credenciales y reglas de autorización de acceso).

## 7. Flujo de trabajo Git

Se trabajó con Git Flow: ramas `main` y `develop`, una rama `feature/*` por requerimiento, integradas a `develop` mediante Pull Request y luego a `main`.
