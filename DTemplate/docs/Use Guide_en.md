# DTemplate Use Guide

Usage guide for ASP.NET Core services built with TurtlePath.

## Index

- [Stack](#stack)
- [Getting Started](#getting-started)
- [Feature Organization](#feature-organization)
- [Naming Conventions](#naming-conventions)
- [Automations](#automations)
- [Custom Handlers](#custom-handlers)
- [Hooks](#hooks)
- [Mapping And Validation](#mapping-and-validation)
- [Transactions](#transactions)
- [Migration Checklist](#migration-checklist)
- [Template Update Notes](#template-update-notes)

## Stack

- `TurtlePath` for handlers, hooks, requests, responses, and application exceptions.
- `TurtlePath.Domain` for `CId`, `BaseEntity`, and `IEntity<TKey>`.
- `TurtlePath.EntityFrameworkCore` for `BaseDbContext`, `IDbContext`, storage adapters, and EF conventions.
- `TurtlePath.OctoMap` for mapping.
- `TurtlePath.Crabalidator` for validation.
- `TurtlePath.Sieve` for filtering, sorting, and paging.
- `TurtlePath.Analyzers` in Domain and Business to catch unsafe `CId` comparisons and assignments.
- `Pigeon.Messaging` with Azure Service Bus as the default broker.
- `Spider.Pipelines` for messaging boundaries and the transaction boundary.

## Getting Started

The template keeps shared infrastructure in TurtlePath packages and keeps service code organized by feature.

### Registering The Stack

The API composition root registers the business assembly, TurtlePath, adapters, identifier mapping, EF Core, Pigeon, and Spider:

```csharp
services.AddCrabalidator(typeof(Constants).Assembly);

services.AddOctoMap(registration =>
{
    registration.Options.EnableRuntimeImplicitMaps = true;
    registration.Options.DuplicateMapPolicy = DuplicateMapPolicy.Throw;
    registration.AddMaps(typeof(Constants).Assembly);
});

services.AddScoped<IMapperAdapter, OctoMapAdapter>();
services.AddScoped<IValidatorAdapter, CrabalidatorAdapter>();

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

Pigeon uses Azure Service Bus by default:

```csharp
services.AddPigeon(configuration, builder =>
{
    // builder.ScanConsumersFromAssemblies(typeof(Program).Assembly);
    builder.UseAzureServiceBus();
});
```

### Minimal Manual Flow

Entity:

```csharp
using TurtlePath.Domain.Contracts;

public sealed class Customer : BaseEntity
{
    public string Name { get; set; }
}
```

Request and response:

```csharp
using Pelican.Mediator;
using TurtlePath.Models.Responses;

public sealed class CreateCustomerRequest : IRequest<CustomerResponse>
{
    public string Name { get; set; }
}

public sealed class CustomerResponse : BaseResponse
{
    public string Name { get; set; }
}
```

Handler:

```csharp
using TurtlePath.Commands;

public sealed class CreateCustomerCommandHandler
    : CreateCommandHandler<CreateCustomerRequest, CustomerResponse, Customer>
{
    public CreateCustomerCommandHandler(IServiceProvider services) : base(services)
    {
    }
}
```

Controller:

```csharp
[HttpPost]
public Task<CustomerResponse> Create(CreateCustomerRequest request, CancellationToken cancellationToken)
{
    return Mediator.Send(request, cancellationToken);
}
```

Use manual handlers when the flow has custom business behavior that should be explicit in code. Use automations for standard CRUD happy paths.

## Feature Organization

Business code should be organized by feature, not by global technical buckets.

```text
Feature/
  Commands/
  Queries/
  Validators/
  Mappings/
  Hooks/
  Automations/
  Models/
    Requests/
    Responses/
  Services/
```

`Feature` is a placeholder. Replace it with the real business capability when creating code.

Examples:

```text
Customers/
  Commands/
  Queries/
  Validators/
  Mappings/
  Hooks/
  Automations/
  Models/
    Requests/
    Responses/
  Services/

Invoices/
  Commands/
  Queries/
  Validators/
  Mappings/
  Hooks/
  Automations/
  Models/
    Requests/
    Responses/
  Services/

Orders/
  Commands/
  Queries/
  Validators/
  Mappings/
  Hooks/
  Automations/
  Models/
    Requests/
    Responses/
  Services/
```

Do not create a generic `Features` container. Each feature lives directly under the Business project.

Guidelines:

- Put commands that mutate state under `Commands`.
- Put read requests under `Queries`.
- Put request/response DTOs under `Models/Requests` and `Models/Responses`.
- Put Crabalidator validators under `Validators`.
- Put OctoMap maps/profiles under `Mappings`.
- Put TurtlePath hooks under `Hooks`.
- Put TurtlePath automation profiles or attributes under `Automations`.
- Put feature-owned collaborators under `Services`.
- When a feature needs an external integration, put it under `Services` using a service-specific folder such as `Services/SAT`.

Shared business services that are reused by several features should live at the Business root and still be grouped by service:

```text
Services/
  Audit/
```

This keeps a future extraction path clean if the service becomes a shared library.

## Naming Conventions

### Requests

Mutation messages are commands conceptually, but their class names must keep the `Request` suffix:

- `CreateCustomerRequest`
- `UpdateInvoiceRequest`
- `ChangeOrderStatusRequest`

### Responses

Responses represent the output of any handler:

- `CustomerResponse`
- `InvoiceResponse`
- `ChangeOrderStatusResponse`

### Command Handlers

Command handlers express the action and end with `CommandHandler`:

- `CreateCustomerCommandHandler`
- `UpdateInvoiceCommandHandler`
- `ChangeOrderStatusCommandHandler`

Example:

```csharp
public sealed class ChangeOrderStatusCommandHandler
    : CreateCommandHandler<ChangeOrderStatusRequest, ChangeOrderStatusResponse, Order>
{
    public ChangeOrderStatusCommandHandler(IServiceProvider services) : base(services)
    {
    }
}
```

### Queries

Query messages and handlers both use `Query` terminology:

- `GetCustomerByIdQuery`
- `GetCustomerByIdQueryHandler`
- `GetPagedInvoicesQuery`
- `GetPagedInvoicesQueryHandler`

For small query flows, keep the handler nested inside the query:

```csharp
public sealed class GetCustomerByIdQuery : IRequest<CustomerResponse>
{
    public CId Id { get; set; }

    public sealed class GetCustomerByIdQueryHandler
        : QueryByIdHandler<GetCustomerByIdQuery, CustomerResponse, Customer>
    {
        public GetCustomerByIdQueryHandler(IServiceProvider services) : base(services)
        {
        }
    }
}
```

When a flow is generated through automations, omit manual command handlers and query handlers.

### Validators

Validators use the request name plus `Validator`:

- `CreateCustomerRequestValidator`
- `UpdateInvoiceRequestValidator`
- `ChangeOrderStatusRequestValidator`

### Mappings

Mapping profiles use the feature or aggregate name plus `MappingProfile`:

- `CustomerMappingProfile`
- `InvoiceMappingProfile`
- `OrderMappingProfile`

### Automations

Automation profiles use the feature or aggregate name plus `AutomationProfile`:

- `CustomerAutomationProfile`
- `InvoiceAutomationProfile`
- `OrderAutomationProfile`

### Hooks

Hooks should describe the action and the hook point where they run:

- `AssignCustomerNumberBeforeSaveHook`
- `PublishInvoiceCanceledAfterSaveHook`
- `NormalizeOrderStatusBeforeValidationHook`

### Services

Feature-owned services live inside the feature under a service-specific folder.

```text
Customers/
  Services/
    SAT/
      ISatService.cs
      SatService.cs
```

Example service method:

```csharp
public interface ISatService
{
    Task<bool> ValidateCustomerRfc(string rfc, CancellationToken cancellationToken);
}
```

Shared services live at the Business root, also grouped by service:

```text
Services/
  Audit/
    IAuditService.cs
    AuditService.cs
```

### Controllers

Controllers use plural resource names:

- `CustomersController`
- `InvoicesController`
- `OrdersController`

Use RESTful routes for CRUD:

- `[POST] customers` creates a resource.
- `[PUT] customers/{id}` updates a resource.
- `[DELETE] customers/{id}` deletes a resource.
- `[GET] customers` reads the collection, normally paged.
- `[GET] customers/{id}` reads one resource by id.

Additional paged filters should be query filters resolved dynamically with Sieve.

Use `POST` for actions outside CRUD:

- `[POST] customers/{id}/deactivate`
- `[POST] invoices/{id}/cancel`

Use nested routes for subresources:

- `[GET] customers/{id}/orders`
- `[DELETE] orders/{id}/details/{detailId}`

### Hub Consumers

Hub consumers also use the plural resource name:

- `CustomersHubConsumer`
- `InvoicesHubConsumer`

### Entity Configurations

Entity configurations use the entity name plus `Configuration`:

- `CustomerConfiguration`
- `InvoiceConfiguration`
- `OrderConfiguration`

## Automations

Use `TurtlePath.Automations` when the feature follows the standard happy path and does not need a custom handler.

### Fluent Profile

```csharp
using TurtlePath.Automations.Profiles;

public sealed class CustomerAutomationProfile : AutomationProfile
{
    public override void Configure(IAutomationProfileBuilder builder)
    {
        builder.For<Customer>()
            .ToCreate<CreateCustomerRequest, CustomerResponse>()
            .ToUpdate<UpdateCustomerRequest, CustomerResponse>()
            .ToDelete<DeleteCustomerRequest>()
            .ToQueryById<GetCustomerByIdQuery, CustomerResponse>()
            .ToQueryPaged<GetPagedCustomersQuery, CustomerResponse>();
    }
}
```

Register automation profiles from the API composition root when the service starts using automations.

### Attributes

Attributes are useful for very small features where the request itself can declare the automation intent.

```csharp
[AutomateCreate(typeof(Customer), typeof(CustomerResponse))]
public sealed class CreateCustomerRequest : IRequest<CustomerResponse>
{
    public string Name { get; set; }
}
```

### Customization Without Handlers

Prefer hooks when the default flow is still correct but needs a business step. See [Hooks](#hooks) for the complete list of command and query hooks.

Use a custom handler when the control flow changes substantially, the operation touches several aggregates, or the behavior would be hard to understand through hooks alone.

## Custom Handlers

Use custom handlers when the default TurtlePath happy path is not enough.

Recommended handlers use `BaseEntity` and `CId`:

```csharp
public sealed class CreateCustomerCommandHandler
    : CreateCommandHandler<CreateCustomerRequest, CustomerResponse, Customer>
{
    public CreateCustomerCommandHandler(IServiceProvider services) : base(services)
    {
    }
}
```

For legacy or custom key contracts, use the generic handlers:

```csharp
public sealed class CreateLegacyCustomerCommandHandler
    : GenericCreateCommandHandler<CreateLegacyCustomerRequest, LegacyCustomerResponse, LegacyCustomer, int>
{
    public CreateLegacyCustomerCommandHandler(IServiceProvider services) : base(services)
    {
    }
}
```

Handlers still expose virtual methods for focused customization. Prefer overriding a method when the change belongs only to that handler. Prefer a hook when the behavior is reusable across handlers or automations.

### Command Handler Virtual Methods

Create handlers:

- `ValidateRequest`: enables or disables request validation. Defaults to `true`.
- `UseProjectionFromStorage`: maps the response from a fresh storage projection after save. Applies only to handlers that return a response. Defaults to `false`.
- `ValidateAsync(request, cancellationToken)`: validates the request.
- `MapToEntityAsync(request, cancellationToken)`: creates the entity from the request.
- `SaveEntityAsync(request, entity, cancellationToken)`: persists the new entity.
- `MapToResponseAsync(request, entity, cancellationToken)`: builds the response. Applies only to handlers that return a response.

Update handlers:

- `ValidateRequest`: enables or disables request validation. Defaults to `true`.
- `UseProjectionFromStorage`: maps the response from a fresh storage projection after save. Applies only to handlers that return a response. Defaults to `false`.
- `GetEntityAsync(request, cancellationToken)`: loads the entity to update.
- `ValidateAsync(request, entity, cancellationToken)`: validates the request with the loaded entity.
- `MapEntityAsync(request, entity, cancellationToken)`: maps request values onto the entity.
- `UpdateEntityAsync(request, entity, cancellationToken)`: saves the updated entity.
- `MapToResponseAsync(request, entity, cancellationToken)`: builds the response. Applies only to handlers that return a response.

Delete handlers:

- `ValidateRequest`: enables or disables request validation. Defaults to `false`.
- `GetEntityAsync(request, cancellationToken)`: loads the entity to delete.
- `ValidateAsync(request, entity, cancellationToken)`: validates whether the delete is allowed.
- `DeleteEntityAsync(entity, cancellationToken)`: deletes the entity.
- `BuildResponseAsync(request, entity, cancellationToken)`: builds the response. Applies only to handlers that return a response.

Patch handlers:

- `ValidateRequest`: enables or disables request validation. Defaults to `false`.
- `GetEntityAsync(request, cancellationToken)`: loads the entity to patch.
- `ValidateAsync(request, entity, cancellationToken)`: validates whether the patch is allowed.
- `PatchEntityAsync(request, entity, cancellationToken)`: applies the patch action to the entity.
- `UpdateEntityAsync(request, entity, cancellationToken)`: saves the patched entity.
- `BuildResponseAsync(request, entity, cancellationToken)`: builds the response. Applies only to handlers that return a response.

Example: disable validation for an idempotent internal command.

```csharp
public sealed class TouchCustomerCommandHandler
    : UpdateCommandHandler<TouchCustomerRequest, Customer>
{
    public TouchCustomerCommandHandler(IServiceProvider services) : base(services)
    {
    }

    protected override bool ValidateRequest => false;
}
```

Example: add a feature-specific lookup rule.

```csharp
public sealed class CancelInvoiceCommandHandler
    : UpdateCommandHandler<CancelInvoiceRequest, InvoiceResponse, Invoice>
{
    public CancelInvoiceCommandHandler(IServiceProvider services) : base(services)
    {
    }

    protected override async Task<Invoice> GetEntityAsync(CancelInvoiceRequest request, CancellationToken cancellationToken)
    {
        var invoice = await StorageReaderAdapter
            .For<Invoice>()
            .Where(x => x.Id == request.Id && !x.IsCanceled)
            .FirstOrDefaultAsync(cancellationToken);

        return invoice ?? throw new NotFoundException(nameof(Invoice), request.Id.ToString());
    }
}
```

### Query Handler Virtual Methods

Get one and get by id handlers:

- `Handle(query, cancellationToken)`: can be overridden when the whole query flow needs to change.
- `GetFilterExpression(query)`: defines how the entity is located.

Get many handlers:

- `Handle(query, cancellationToken)`: can be overridden when the whole query flow needs to change.
- `GetFilterExpression(query)`: adds typed filters before Sieve string filters.
- `GetSortingExpression(query)`: adds typed sorting before Sieve string sorting.

Get paged handlers:

- `DefaultSorts`: defines fallback Sieve sorting when the request does not specify sorts.
- `GetFiltersExpression(query)`: adds typed filters before Sieve string filters.
- `GetSortingExpression(query)`: adds typed sorting before Sieve string sorting.

Example: force a customer-scoped invoice list while still allowing Sieve filters.

```csharp
public sealed class GetPagedInvoicesQueryHandler
    : GetPagedInfoQueryHandler<GetPagedInvoicesQuery, Invoice, InvoiceResponse>
{
    public GetPagedInvoicesQueryHandler(IServiceProvider services) : base(services)
    {
    }

    protected override string DefaultSorts => "-CreatedAt";

    protected override Expression<Func<Invoice, bool>> GetFiltersExpression(GetPagedInvoicesQuery query)
    {
        return invoice => invoice.CustomerId == query.CustomerId;
    }
}
```

## Hooks

Hooks let a feature customize the standard path without replacing the handler.

Use hooks when the default flow is correct and you only need to add behavior at a known stage. Hooks are discovered through dependency injection, run in registration order, and can implement `IOrderedHook` when several hooks target the same stage.

Command hooks use `CommandHookContext<TRequest, TEntity>` or `CommandHookContext<TRequest, TEntity, TResponse>`. The context exposes `Request`, `Entity`, `Response` when available, and typed key/value storage to share data between hooks in the same handler execution.

Available command hooks:

- `IBeforeValidationHook<TRequest, TEntity>`: normalize request data or load context before validation.
- `IAfterValidationHook<TRequest, TEntity>`: run logic that depends on a valid request.
- `IBeforeGetEntityHook<TRequest, TEntity>`: prepare data before update, delete, or patch loads the entity.
- `IAfterGetEntityHook<TRequest, TEntity>`: inspect or enrich the loaded entity before validation or mutation.
- `IBeforeMapHook<TRequest, TEntity>`: normalize data before create or update mapping.
- `IAfterMapHook<TRequest, TEntity>`: enforce derived entity values after mapping.
- `IBeforePatchHook<TRequest, TEntity>`: check patch preconditions.
- `IAfterPatchHook<TRequest, TEntity>`: enforce derived values after patching.
- `IBeforeSaveHook<TRequest, TEntity>`: stamp audit fields, assign business numbers, or prepare side effects before persistence.
- `IAfterSaveHook<TRequest, TEntity>`: publish messages, write audit records, or trigger async work after persistence.
- `IBeforeDeleteHook<TRequest, TEntity>`: block deletion or prepare related cleanup.
- `IAfterDeleteHook<TRequest, TEntity>`: publish deletion events or write audit records.
- `IBeforeResponseHook<TRequest, TEntity, TResponse>`: enrich data before the response is built.
- `IAfterResponseHook<TRequest, TEntity, TResponse>`: adjust or enrich the response before it returns.

Available query hooks:

- `IBeforeQueryHook<TQuery, TResult>`: apply read context, capture telemetry data, or validate query preconditions.
- `IAfterQueryHook<TQuery, TResult>`: enrich query results, capture metrics, or write read audit records.

Example: assign a business number before saving a customer.

```csharp
public sealed class AssignCustomerNumberBeforeSaveHook
    : IBeforeSaveHook<CreateCustomerRequest, Customer>
{
    private readonly ICustomerNumberService customerNumberService;

    public AssignCustomerNumberBeforeSaveHook(ICustomerNumberService customerNumberService)
    {
        this.customerNumberService = customerNumberService;
    }

    public async ValueTask BeforeSaveAsync(CommandHookContext<CreateCustomerRequest, Customer> context, CancellationToken cancellationToken)
    {
        context.Entity.CustomerNumber = await customerNumberService.NextAsync(cancellationToken);
    }
}
```

Example: publish an integration event after saving.

```csharp
public sealed class PublishCustomerCreatedAfterSaveHook
    : IAfterSaveHook<CreateCustomerRequest, Customer>
{
    private readonly ISpider spider;

    public PublishCustomerCreatedAfterSaveHook(ISpider spider)
    {
        this.spider = spider;
    }

    public async ValueTask AfterSaveAsync(CommandHookContext<CreateCustomerRequest, Customer> context, CancellationToken cancellationToken)
    {
        await spider.Send(new CustomerCreatedEvent(context.Entity.Id), cancellationToken);
    }
}
```

Example: enrich a paged query result after it runs.

```csharp
public sealed class AttachInvoiceSummaryAfterQueryHook
    : IAfterQueryHook<GetPagedInvoicesQuery, PagedResponse<InvoiceResponse>>
{
    public ValueTask AfterQueryAsync(QueryHookContext<GetPagedInvoicesQuery, PagedResponse<InvoiceResponse>> context, CancellationToken cancellationToken)
    {
        foreach (var invoice in context.Result.Results)
            invoice.CanBeCanceled = invoice.Status == InvoiceStatus.Pending;

        return ValueTask.CompletedTask;
    }
}
```

Use hooks for:

- cross-cutting audit and telemetry
- feature-specific enrichment
- request normalization before validation or mapping
- side effects after save or delete
- response enrichment
- business checks that fit naturally at one stage

Avoid hooks when the main business flow becomes hard to understand without opening many files. In that case, use a custom handler.

## Mapping And Validation

The template uses OctoMap and Crabalidator through TurtlePath adapters.

### Mapping

Register maps in the Business assembly. The API composition root scans the assembly:

```csharp
services.AddOctoMap(registration =>
{
    registration.Options.EnableRuntimeImplicitMaps = true;
    registration.Options.DuplicateMapPolicy = DuplicateMapPolicy.Throw;
    registration.AddMaps(typeof(Constants).Assembly);
});
```

Keep maps close to the feature that owns them:

```text
Feature/Mappings/
```

### Validation

Register validators in the Business assembly. The API composition root scans the assembly:

```csharp
services.AddCrabalidator(typeof(Constants).Assembly);
```

Keep validators close to the feature request:

```text
Feature/Validators/
```

TurtlePath handlers and automations call the registered validator adapter before mapping or saving.

## Transactions

The template uses a Spider execution boundary for ambient transactions instead of a Pelican pipeline behavior.

Registration:

```csharp
services.Configure<TransactionBoundaryOptions>(configuration.GetSection("TransactionBoundary"));
services.AddSingleton<ITransactionBoundaryRequestFilter>(provider =>
{
    var filter = new TransactionBoundaryRequestFilter(provider.GetRequiredService<IOptions<TransactionBoundaryOptions>>());
    filter.Discover(typeof(Constants).Assembly);

    return filter;
});

services.AddSpider(builder =>
{
    builder.AddExecutionBoundary<TransactionExecutionBoundary>();
});
```

Configuration:

```json
"TransactionBoundary": {
  "Enabled": true,
  "IncludeQueries": false,
  "IsolationLevel": "ReadCommitted",
  "TimeoutSeconds": 30,
  "ExcludedRequestTypes": []
}
```

By default:

- mutations run inside a `TransactionScope`
- query requests are skipped
- requests marked with `[SkipTransactionBoundary]` are skipped
- request types listed in `ExcludedRequestTypes` are skipped
- request boundary decisions are discovered and cached by request type

`ExcludedRequestTypes` accepts either the full type name or the short type name.

```json
"ExcludedRequestTypes": [
  "RebuildSearchIndexCommand",
  "MyService.Features.Health.Commands.PingExternalDependencyCommand"
]
```

## Migration Checklist

Use this checklist when migrating a service created with the previous template.

- Replace `DTemplate.Domain.Identifier` usages with `TurtlePath.Domain.Identifier`.
- Replace `DTemplate.Domain.Contracts` usages with `TurtlePath.Domain.Contracts`.
- Replace `DTemplate.Business.Core.Commands` usages with `TurtlePath.Commands`.
- Replace `DTemplate.Business.Core.Queries` usages with `TurtlePath.Queries`.
- Replace `DTemplate.Business.Core.Models.Requests` usages with `TurtlePath.Models.Requests`.
- Replace `DTemplate.Business.Core.Models.Responses` usages with `TurtlePath.Models.Responses`.
- Replace `DTemplate.Business.Core.Hooks` usages with `TurtlePath.Hooks`.
- Replace `DTemplate.Business.Core.Exceptions` usages with `TurtlePath.Exceptions` or `TurtlePath.Validation`.
- Replace local `IDbContext` usages with `TurtlePath.EntityFrameworkCore.IDbContext`.
- Make the concrete DbContext inherit from `TurtlePath.EntityFrameworkCore.BaseDbContext`.
- Remove local handler core, local CId implementation, local storage adapters, local mapper adapter, and local validator adapter.
- Remove entity configurations that inherit from the old `BaseEntityConfiguration<TEntity>`.
- Register Business dependencies from the API composition root.
- Register TurtlePath from the API composition root.
- Register OctoMap through `TurtlePath.OctoMap`.
- Register Crabalidator through `TurtlePath.Crabalidator`.
- Register Sieve through `TurtlePath.Sieve`.
- Replace Pelican transaction pipeline behavior registration with the Spider transaction boundary.
- Add `TurtlePath.Analyzers` privately to Domain and Business projects.
- Build and run the composition tests before migrating feature code.

## Template Update Notes

The template was updated to consume the published TurtlePath NuGet packages instead of carrying the extracted handler, identifier, persistence, mapper, and validator infrastructure locally.

Key changes:

- `DTemplate.Domain` keeps service-owned entities and domain code only.
- `DTemplate.Business` keeps service-owned commands, queries, validators, mappings, hooks, automations, and services.
- `DTemplate.Persistence` keeps the concrete EF Core context and service-owned EF configurations only.
- Shared handler and identifier infrastructure now lives in TurtlePath packages.
- Business dependency registration moved to the API composition root.
- The transaction behavior moved from a Pelican pipeline behavior to a Spider execution boundary.
- Pigeon defaults to Azure Service Bus.

Verification:

```powershell
dotnet restore DTemplate.sln --verbosity minimal
dotnet build DTemplate.sln --configuration Release --no-restore --verbosity minimal
dotnet test DTemplate.sln --configuration Release --no-build --verbosity minimal
```
