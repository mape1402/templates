# TurtlePath Template Update Plan

## Goal

Update the service template to consume the published TurtlePath NuGet packages instead of carrying the extracted handler, identifier, persistence, mapper, and validator infrastructure locally.

## Execution Plan

1. Replace duplicated template infrastructure with TurtlePath packages.
   - Use `TurtlePath` as the handler/application package.
   - Use `TurtlePath.Domain` for `CId`, `BaseEntity`, and `IEntity`.
   - Use `TurtlePath.EntityFrameworkCore` for `BaseDbContext`, `IDbContext`, storage adapters, and CId EF conventions.
   - Use `TurtlePath.OctoMap` and `TurtlePath.Crabalidator` as the recommended Elysium adapters.
   - Use `TurtlePath.Sieve` for query criteria support.
   - Use `TurtlePath.Analyzers` privately to catch unsafe cross-entity `CId` comparisons and assignments.

2. Simplify the template layers.
   - Keep `DTemplate.Domain` for service-owned entities and domain code only.
   - Keep `DTemplate.Business` for service-owned commands, queries, validators, mappings, and hooks.
   - Keep `DTemplate.Persistence` for the concrete EF Core context and service-owned EF configurations only.
   - Remove the local copied handler core, local CId implementation, local storage adapters, local mapper adapter, local validator adapter, and local base EF entity configuration.

3. Update dependency registration.
   - Register Pelican from the business assembly.
   - Register Business dependencies from the API composition root.
   - Register TurtlePath once through the chainable builder.
   - Configure the default `CId` profile as `Ulid` in C# and `string` in the database.
   - Register EF Core through `UseEntityFrameworkCore<AppDbContext>()`.
   - Register OctoMap and Crabalidator through TurtlePath adapters.
   - Register Sieve through TurtlePath.

4. Update MVC, Swagger, Pigeon, and Spider.
   - Use TurtlePath JSON converters for `CId`.
   - Update Swagger schema filters to target `TurtlePath.Domain.Identifier.CId`.
   - Keep Pigeon consumer registration in the API composition root.
   - Keep Spider pipeline registration in the API composition root.
   - Keep Pigeon and Spider registration in the API composition root.
   - Register the transaction boundary through Spider instead of a Pelican transaction pipeline behavior.

5. Verify.
   - Restore the solution.
   - Build the solution.
   - Run tests.

## Service Migration Checklist

- Replace `DTemplate.Domain.Identifier` usages with `TurtlePath.Domain.Identifier`.
- Replace `DTemplate.Domain.Contracts` usages with `TurtlePath.Domain.Contracts`.
- Replace `DTemplate.Business.Core.Commands` usages with `TurtlePath.Commands`.
- Replace `DTemplate.Business.Core.Queries` usages with `TurtlePath.Queries`.
- Replace `DTemplate.Business.Core.Models.Requests` usages with `TurtlePath.Models.Requests`.
- Replace `DTemplate.Business.Core.Models.Responses` usages with `TurtlePath.Models.Responses`.
- Replace `DTemplate.Business.Core.Hooks` usages with `TurtlePath.Hooks`.
- Replace `DTemplate.Business.Core.Exceptions` usages with `TurtlePath.Exceptions` or `TurtlePath.Validation`.
- Replace local `IDbContext` usages with `TurtlePath.EntityFrameworkCore.IDbContext`.
- Make the concrete `AppDbContext` inherit from `TurtlePath.EntityFrameworkCore.BaseDbContext`.
- Remove entity configurations that inherit from the old `BaseEntityConfiguration<TEntity>`; use normal `IEntityTypeConfiguration<TEntity>` and let the TurtlePath DbContext conventions configure `BaseEntity` ids.
- Register TurtlePath from the API composition root using `services.AddTurtlePath(...).UseOctoMap().UseCrabalidator().UseSieve().UseCId(...).UseEntityFrameworkCore<AppDbContext>()`.
- Remove service-owned `AddBusiness()` dependency registration extensions; keep Business as behavior/model code, not composition code.
- Replace Pelican transaction pipeline behavior registration with `services.AddSpider(builder => builder.AddExecutionBoundary<TransactionExecutionBoundary>())`.
- Keep service-specific validators in the business assembly so Crabalidator can discover them.
- Keep service-specific OctoMap profiles in the business assembly so OctoMap can discover them.
- Keep Pigeon consumers in the API assembly or explicitly scan their assemblies in `AddPigeon`.
- Keep Spider registered before consumers need `ISpider`.
- Resolve `Pelican.Mediator.IMediator` from `Context.Services` in Pigeon consumers when they need to trigger commands or queries.
- Add `TurtlePath.Analyzers` as a private analyzer dependency to catch `CId` mistakes during migration.
- Build and test the service before removing compatibility namespaces or aliases.
