# DTemplate

Template base para servicios ASP.NET Core de Elysium.

La infraestructura transversal de handlers, identificadores, persistencia, hooks, mapeo, validación y queries vive en los paquetes publicados de TurtlePath. El template solo conserva código propio del servicio: composición, DbContext concreto, perfiles de mapeo, validadores, comandos, queries, consumidores y configuraciones específicas.

## Capas

### DTemplate.Api

- Host ASP.NET Core.
- Registro de Swagger, MVC, health checks, Pigeon, Spider Pipelines, Pelican y TurtlePath.
- Filtro global de excepciones.
- Boundary transaccional de Spider para envolver ejecuciones de pipeline.

### DTemplate.Business

- Assembly principal para comandos, queries, validators, mapping profiles y hooks del servicio.
- Usa los adapters recomendados, registrados desde `DTemplate.Api`:
  - `TurtlePath.OctoMap`
  - `TurtlePath.Crabalidator`
  - `TurtlePath.Sieve`

### DTemplate.Domain

- Entidades y contratos propios del dominio del servicio.
- Usa `TurtlePath.Domain` para:
  - `CId`
  - `BaseEntity`
  - `IEntity<TKey>`

### DTemplate.Persistence

- DbContext concreto del servicio.
- `AppDbContext` hereda de `TurtlePath.EntityFrameworkCore.BaseDbContext`.
- Las convenciones de TurtlePath configuran `BaseEntity`, `CId` y adapters de lectura/escritura.

## Registro Principal

El template registra dependencias de Business y TurtlePath desde `StartupExtensions` en la capa API:

```csharp
services.AddCrabalidator(typeof(Constants).Assembly);

services.AddOctoMap(registration =>
{
    registration.Options.EnableRuntimeImplicitMaps = true;
    registration.Options.DuplicateMapPolicy = DuplicateMapPolicy.Throw;
    registration.AddMaps(typeof(Constants).Assembly);
});

services.AddTurtlePath(typeof(Constants).Assembly)
    .UseOctoMap()
    .UseCrabalidator()
    .UseSieve()
    .UseCId<Ulid, string>(config =>
    {
        config.DefaultFactory = () => CId.From(Ulid.NewUlid());
        config.ConvertToDb = id => id.ToString();
        config.ConvertFromDb = value => CId.From(Ulid.Parse(value));
        config.JsonConverter = value => string.IsNullOrEmpty(value) ? CId.From(Ulid.Empty) : CId.From(Ulid.Parse(value));
        config.NullableJsonConverter = value => string.IsNullOrEmpty(value) ? null : CId.From(Ulid.Parse(value));
        config.ParseFunction = value => CId.From(Ulid.Parse(value));
    })
    .UseEntityFrameworkCore<AppDbContext>();
```

## Pigeon y Spider Pipelines

El template mantiene Pigeon y Spider en la composición de API:

```csharp
services.AddPigeon(configuration, builder =>
{
    //builder
        //.ScanConsumersFromAssemblies(typeof(Program).Assembly)
        //.UseRabbitMq();
});

services.AddSpider(builder =>
{
    builder.AddExecutionBoundary<TransactionExecutionBoundary>();
});
```

Los consumidores Pigeon pueden resolver servicios desde `Context.Services`, incluyendo `Pelican.Mediator.IMediator`, cuando necesiten enviar comandos o queries.

## Migración Desde La Versión Anterior Del Template

Usa el checklist de `docs/TURTLEPATH_TEMPLATE_UPDATE_PLAN.md` para migrar servicios existentes. Los cambios principales son:

- Cambiar namespaces `DTemplate.Business.Core.*` por `TurtlePath.*`.
- Cambiar `DTemplate.Domain.Identifier` por `TurtlePath.Domain.Identifier`.
- Cambiar `DTemplate.Domain.Contracts` por `TurtlePath.Domain.Contracts`.
- Cambiar `DTemplate.Persistence.Abstractions.IDbContext` por `TurtlePath.EntityFrameworkCore.IDbContext`.
- Eliminar `BaseEntityConfiguration<TEntity>` y dejar que `BaseDbContext` aplique las convenciones de TurtlePath.
- Registrar OctoMap y Crabalidator usando los adapters `TurtlePath.OctoMap` y `TurtlePath.Crabalidator`.
- Agregar `TurtlePath.Analyzers` como dependencia privada para detectar comparaciones/asignaciones peligrosas de `CId`.
