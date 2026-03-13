# Documentación de la solución DTemplate

## Tabla de contenido
- [DTemplate.Api](#dtemplateapi)
  - [Visión general](#dtemplateapi-visión-general)
  - [Componentes](#dtemplateapi-componentes)
    - **Controllers**
      - [BaseController](#dtemplateapi-basecontroller)
    - **Filters**
      - [GlobalExceptionFilter](#dtemplateapi-globalexceptionfilter)
    - **Messaging**
      - [BaseHubConsumer](#dtemplateapi-basehubconsumer)
    - **Extensions**
      - [StartupExtensions](#dtemplateapi-startupextensions)
      - [PersistenceExtensions](#dtemplateapi-persistenceextensions)
      - [MvcExtensions](#dtemplateapi-mvcextensions)
      - [SwaggerExtensions](#dtemplateapi-swaggerextensions)
      - [HealthCheckExtensions](#dtemplateapi-healthcheckextensions)
      - [MediatorExtensions](#dtemplateapi-mediatorextensions)
    - **Routing**
      - [RoutePrefixConvention](#dtemplateapi-routeprefixconvention)
    - **Swagger**
      - [SwaggerConstants](#dtemplateapi-swaggerconstants)
      - [CIdSchemaFilter](#dtemplateapi-cidschemafilter)
      - [RemoveVersionParametersFilter](#dtemplateapi-removeversionparametersfilter)
      - [SetVersionInPathsFilter](#dtemplateapi-setversioninpathsfilter)
- [DTemplate.Business](#dtemplatebusiness)
  - [Visión general](#dtemplatebusiness-visión-general)
  - [Componentes](#dtemplatebusiness-componentes)
    - **Base**
      - [Constants](#dtemplatebusiness-constants)
      - [MappingProfile](#dtemplatebusiness-mappingprofile)
      - [BaseRequest](#dtemplatebusiness-baserequest)
      - [BaseResponse](#dtemplatebusiness-baseresponse)
      - [PagedResponse](#dtemplatebusiness-pagedresponse)
    - **Criteria & Settings**
      - [GetOneCriteria](#dtemplatebusiness-getonecriteria)
      - [GetManyCriteria](#dtemplatebusiness-getmanycriteria)
      - [PagedSettings](#dtemplatebusiness-pagedsettings)
    - **Results**
      - [BatchResult](#dtemplatebusiness-batchresult)
    - **Adapters**
      - [IStorageReaderAdapter](#dtemplatebusiness-istoragereaderadapter)
      - [IStorageWriterAdapter](#dtemplatebusiness-istoragewriteradapter)
      - [IMapperAdapter](#dtemplatebusiness-imapperadapter)
      - [IValidatorAdapter](#dtemplatebusiness-ivalidadapter)
      - [ValidatorAdapter](#dtemplatebusiness-validatoradapter)
      - [MapperAdapter](#dtemplatebusiness-mapperadapter)
      - [StorageReaderAdapter](#dtemplatebusiness-storagereaderadapter)
      - [StorageWriterAdapter](#dtemplatebusiness-storagewriteradapter)
    - **Handlers**
      - [BaseCommandHandler](#dtemplatebusiness-basecommandhandler)
      - [NoReturnCommandHandler](#dtemplatebusiness-noreturncommandhandler)
      - [CreateCommandHandler (con respuesta)](#dtemplatebusiness-createcommandhandler-con-respuesta)
      - [CreateCommandHandler (sin respuesta)](#dtemplatebusiness-createcommandhandler-sin-respuesta)
      - [DeleteCommandHandler (con respuesta)](#dtemplatebusiness-deletecommandhandler-con-respuesta)
      - [DeleteCommandHandler (sin respuesta)](#dtemplatebusiness-deletecommandhandler-sin-respuesta)
      - [UpdateCommandHandler (con respuesta)](#dtemplatebusiness-updatecommandhandler-con-respuesta)
      - [UpdateCommandHandler (sin respuesta)](#dtemplatebusiness-updatecommandhandler-sin-respuesta)
      - [PatchCommandHandler (con respuesta)](#dtemplatebusiness-patchcommandhandler-con-respuesta)
      - [PatchCommandHandler (sin respuesta)](#dtemplatebusiness-patchcommandhandler-sin-respuesta)
    - **Queries**
      - [GetOneQuery](#dtemplatebusiness-getonequery)
      - [GetOneQueryHandler](#dtemplatebusiness-getonequeryhandler)
      - [GetByIdQuery](#dtemplatebusiness-getbyidquery)
      - [GetByIdQueryHandler](#dtemplatebusiness-getbyidqueryhandler)
      - [GetManyQuery](#dtemplatebusiness-getmanyquery)
      - [GetManyQueryHandler](#dtemplatebusiness-getmanyqueryhandler)
      - [GetPagedInfoQuery](#dtemplatebusiness-getpagedinfoquery)
      - [GetPagedInfoQueryHandler](#dtemplatebusiness-getpagedinfoqueryhandler)
    - **Pipeline**
      - [TransactionPipelineBehavior](#dtemplatebusiness-transactionpipelinebehavior)
    - **Extensions**
      - [ServicesExtensions](#dtemplatebusiness-servicesextensions)
    - **Exceptions**
      - [HttpException](#dtemplatebusiness-httpexception)
      - [BadRequestException](#dtemplatebusiness-badrequestexception)
      - [NotFoundException](#dtemplatebusiness-notfoundexception)
      - [UnauthorizedException](#dtemplatebusiness-unauthorizedexception)
      - [ForbiddenException](#dtemplatebusiness-forbiddenexception)
- [DTemplate.Domain](#dtemplatedomain)
  - [Visión general](#dtemplatedomain-visión-general)
  - [Componentes](#dtemplatedomain-componentes)
    - **Entities**
      - [IEntity](#dtemplatedomain-ientity)
      - [IEntity<TKey>](#dtemplatedomain-ientitytkey)
      - [BaseEntity](#dtemplatedomain-baseentity)
    - **CId**
      - [CId](#dtemplatedomain-cid)
      - [CIdConfiguration](#dtemplatedomain-cidconfiguration)
      - [CIdMetadata](#dtemplatedomain-cidmetadata)
      - [CIdTypeConverter](#dtemplatedomain-cidtypeconverter)
      - [CIdJsonConverter](#dtemplatedomain-cidjsonconverter)
      - [CIdNulleableJsonConverter](#dtemplatedomain-cidnulleablejsonconverter)
      - [CIdDbValueGenerator](#dtemplatedomain-ciddbvaluegenerator)
    - **Extensions**
      - [ServiceExtensions](#dtemplatedomain-serviceextensions)
- [DTemplate.Persistence](#dtemplatepersistence)
  - [Visión general](#dtemplatepersistence-visión-general)
  - [Componentes](#dtemplatepersistence-componentes)
    - **Context**
      - [IDbContext](#dtemplatepersistence-idbcontext)
      - [AppDbContext](#dtemplatepersistence-appdbcontext)
    - **Configurations**
      - [BaseEntityConfiguration](#dtemplatepersistence-baseentityconfiguration)
- [DTemplate.Tests](#dtemplatetests)
  - [Visión general](#dtemplatetests-visión-general)

---

# DTemplate.Api

## DTemplate.Api visión general
Proyecto de API ASP.NET Core que configura el host, expone controladores, aplica filtros de excepciones, y ofrece extensiones de configuración para servicios, Swagger, salud y mediación. Integra `Pelican.Mediator`, `Serilog`, y componentes de infraestructura compartidos.

## DTemplate.Api componentes

### DTemplate.Api BaseController
**Descripción:** Controlador base que proporciona acceso al mediador desde los servicios de la solicitud HTTP.

**Índice de métodos:** No aplica.

**Casos de uso:**
- Derivar controladores que necesiten enviar comandos/consultas mediante `IMediator`.

**Propiedades**
| Nombre | Tipo | Descripción |
| --- | --- | --- |
| `Mediator` | `IMediator` | Obtiene el mediador desde `HttpContext.RequestServices`. Lanza excepción si no está disponible. |

**Métodos**
No define métodos públicos adicionales.

**Ejemplo de uso**
```csharp
public class OrdersController : BaseController
{
    [HttpPost]
    public Task<IActionResult> Create(CreateOrder command)
        => Mediator.Send(command).ContinueWith(_ => Ok());
}
```

---

### DTemplate.Api GlobalExceptionFilter
**Descripción:** Filtro global que captura excepciones, determina un código HTTP y devuelve un `ProblemDetails` consistente.

**Índice de métodos**
- [`GlobalExceptionFilter`](#dtemplate-api-globalexceptionfilter-ctor)
- [`OnException`](#dtemplate-api-globalexceptionfilter-onexception)

**Casos de uso:**
- Centralizar el manejo de errores y la forma de respuesta en la API.

**Propiedades**
| Nombre | Tipo | Descripción |
| --- | --- | --- |
| `_logger` | [`ILogger<GlobalExceptionFilter>`](https://learn.microsoft.com/dotnet/api/microsoft.extensions.logging.ilogger-1) | Registrador interno para eventos de error. |
| `_options` | [`ApiBehaviorOptions`](https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.mvc.apibehavioroptions) | Opciones de comportamiento de API para mapear códigos de error. |

**Métodos**
#### <a id="dtemplate-api-globalexceptionfilter-ctor"></a>`GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger, IOptions<ApiBehaviorOptions> options)`
- **Descripción:** Inicializa el filtro con logger y opciones de API.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
| `logger` | [`ILogger<GlobalExceptionFilter>`](https://learn.microsoft.com/dotnet/api/microsoft.extensions.logging.ilogger-1) | Registrador de eventos. |
| `options` | [`IOptions<ApiBehaviorOptions>`](https://learn.microsoft.com/dotnet/api/microsoft.extensions.options.ioptions-1) | Opciones de API. |
- **Devuelve:** No aplica.
- **Excepciones:** `ArgumentNullException` si `logger` es nulo.
- **Ejemplo:**
```csharp
services.AddControllers(o => o.Filters.Add<GlobalExceptionFilter>());
```

#### <a id="dtemplate-api-globalexceptionfilter-onexception"></a>`OnException(ExceptionContext context)`
- **Descripción:** Procesa la excepción y genera un `ProblemDetails` con estado apropiado.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
| `context` | [`ExceptionContext`](https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.mvc.filters.exceptioncontext) | Contexto de excepción de ASP.NET Core. |
- **Devuelve:** No aplica; modifica `context.Result` y `Response.StatusCode`.
- **Excepciones:** No lanza explícitamente; registra el error.
- **Ejemplo:**
```csharp
// Ejecutado automáticamente por el pipeline MVC cuando ocurre una excepción.
```

---

### DTemplate.Api BaseHubConsumer
**Descripción:** Consumidor base de `Pigeon` que expone una instancia de `ISpider` para orquestación.

**Índice de métodos:** No aplica.

**Casos de uso:**
- Crear consumidores de mensajería con acceso a pipelines de `Spider`.

**Propiedades**
| Nombre | Tipo | Descripción |
| --- | --- | --- |
| `Spider` | `ISpider` | Resuelve `ISpider` desde los servicios del contexto. |

**Métodos**
No define métodos públicos adicionales.

**Ejemplo de uso**
```csharp
public class OrderConsumer : BaseHubConsumer
{
    public Task HandleAsync(MyMessage message) => Spider.DefaultSend(message.Command);
}
```

---

### DTemplate.Api StartupExtensions
**Descripción:** Extensiones de configuración de servicios y middleware para el host de la API.

**Índice de métodos**
- [`AddDefaults`](#dtemplate-api-startupextensions-adddefaults)
- [`UseDefaults`](#dtemplate-api-startupextensions-usedefaults)

**Métodos**
#### <a id="dtemplate-api-startupextensions-adddefaults"></a>`AddDefaults(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)`
- **Descripción:** Registra servicios base (Swagger, persistencia, mediación, errores, mapeos, salud).
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
| `services` | [`IServiceCollection`](https://learn.microsoft.com/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection) | Contenedor de servicios. |
| `configuration` | [`IConfiguration`](https://learn.microsoft.com/dotnet/api/microsoft.extensions.configuration.iconfiguration) | Configuración de aplicación. |
| `environment` | [`IWebHostEnvironment`](https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.hosting.iwebhostenvironment) | Entorno de hosting. |
- **Devuelve:** `IServiceCollection` con registros agregados.
- **Excepciones:** Puede propagar excepciones de configuración si faltan dependencias.
- **Ejemplo:**
```csharp
builder.Services.AddDefaults(builder.Configuration, builder.Environment);
```

#### <a id="dtemplate-api-startupextensions-usedefaults"></a>`UseDefaults(IApplicationBuilder app, IWebHostEnvironment environment)`
- **Descripción:** Configura middleware por defecto (Swagger en desarrollo, HTTPS, routing, autorización, endpoints).
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
| `app` | [`IApplicationBuilder`](https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.builder.iapplicationbuilder) | Builder del pipeline HTTP. |
| `environment` | [`IWebHostEnvironment`](https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.hosting.iwebhostenvironment) | Entorno de hosting. |
- **Devuelve:** `IApplicationBuilder` configurado.
- **Excepciones:** No lanza explícitamente.
- **Ejemplo:**
```csharp
app.UseDefaults(app.Environment);
```

**Casos de uso:**
- Inicializar la API con configuraciones predefinidas.

---

### DTemplate.Api PersistenceExtensions
**Descripción:** Extensiones para registrar la capa de persistencia.

**Índice de métodos**
- [`AddPersistence`](#dtemplate-api-persistenceextensions-addpersistence)

**Métodos**
#### <a id="dtemplate-api-persistenceextensions-addpersistence"></a>`AddPersistence(IServiceCollection services, string connectionString)`
- **Descripción:** Registra `AppDbContext` y `IDbContext` usando SQL Server (o PostgreSQL si se habilita).
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
| `services` | [`IServiceCollection`](https://learn.microsoft.com/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection) | Contenedor de servicios. |
  | `connectionString` | `string` | Cadena de conexión de la base de datos. |
- **Devuelve:** No aplica.
- **Excepciones:** Propaga errores de configuración de EF Core.
- **Ejemplo:**
```csharp
services.AddPersistence(configuration.GetConnectionString("Default"));
```

---

### DTemplate.Api MvcExtensions
**Descripción:** Configura MVC y opciones JSON internas del proyecto.

**Índice de métodos**
- [`AddMvcDefaults`](#dtemplate-api-mvcextensions-addmvcdefaults)

**Métodos**
#### <a id="dtemplate-api-mvcextensions-addmvcdefaults"></a>`AddMvcDefaults(IServiceCollection services)`
- **Descripción:** Configura routing, filtros globales, convención de rutas y serialización JSON con convertidores de [`CId`](#dtemplatedomain-cid) y enums.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
| `services` | [`IServiceCollection`](https://learn.microsoft.com/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection) | Contenedor de servicios. |
- **Devuelve:** No aplica.
- **Excepciones:** No lanza explícitamente.
- **Ejemplo:**
```csharp
services.AddMvcDefaults();
```

---

### DTemplate.Api RoutePrefixConvention
**Descripción:** Convención MVC que aplica un prefijo de ruta global a los controladores.

**Índice de métodos**
- [`RoutePrefixConvention`](#dtemplate-api-routeprefixconvention-ctor)
- [`Apply`](#dtemplate-api-routeprefixconvention-apply)

**Propiedades**
| Nombre | Tipo | Descripción |
| --- | --- | --- |
| `RoutePrefix` | [`AttributeRouteModel`](https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.mvc.applicationmodels.attributeroutemodel) | Modelo de ruta que actúa como prefijo. |

**Métodos**
#### <a id="dtemplate-api-routeprefixconvention-ctor"></a>`RoutePrefixConvention(IRouteTemplateProvider route)`
- **Descripción:** Inicializa el prefijo con un proveedor de plantilla de ruta.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
| `route` | [`IRouteTemplateProvider`](https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.mvc.routing.iroutetemplateprovider) | Proveedor del prefijo. |
- **Devuelve:** No aplica.
- **Excepciones:** No lanza explícitamente.
- **Ejemplo:**
```csharp
opts.Conventions.Add(new RoutePrefixConvention(new RouteAttribute("api/")));
```

#### <a id="dtemplate-api-routeprefixconvention-apply"></a>`Apply(ApplicationModel application)`
- **Descripción:** Inserta el prefijo en cada selector de controlador.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
| `application` | [`ApplicationModel`](https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.mvc.applicationmodels.applicationmodel) | Modelo de aplicación MVC. |
- **Devuelve:** No aplica.
- **Excepciones:** No lanza explícitamente.
- **Ejemplo:**
```csharp
// Ejecutado automáticamente por MVC al construir el modelo.
```

---

### DTemplate.Api SwaggerExtensions
**Descripción:** Configura Swagger y Swagger UI para la API.

**Índice de métodos**
- [`AddSwaggerDefaults`](#dtemplate-api-swaggerextensions-addswaggerdefaults)
- [`UseSwaggerDefaults`](#dtemplate-api-swaggerextensions-useswaggerdefaults)

**Métodos**
#### <a id="dtemplate-api-swaggerextensions-addswaggerdefaults"></a>`AddSwaggerDefaults(IServiceCollection services)`
- **Descripción:** Registra Swagger, configura documento, filtros y carga XML de comentarios.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
| `services` | [`IServiceCollection`](https://learn.microsoft.com/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection) | Contenedor de servicios. |
- **Devuelve:** No aplica.
- **Excepciones:** Puede propagar excepciones al cargar XML.
- **Ejemplo:**
```csharp
services.AddSwaggerDefaults();
```

#### <a id="dtemplate-api-swaggerextensions-useswaggerdefaults"></a>`UseSwaggerDefaults(IApplicationBuilder app, IWebHostEnvironment env)`
- **Descripción:** Configura endpoints de Swagger y Swagger UI con opciones de UI y entorno.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
| `app` | [`IApplicationBuilder`](https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.builder.iapplicationbuilder) | Builder de la aplicación. |
| `env` | [`IWebHostEnvironment`](https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.hosting.iwebhostenvironment) | Entorno de hosting. |
- **Devuelve:** No aplica.
- **Ejemplo:**
```csharp
app.UseSwaggerDefaults(app.Environment);
```

---

### DTemplate.Api HealthCheckExtensions
**Descripción:** Extensiones para configurar y exponer endpoints de salud.

**Índice de métodos**
- [`AddHealthChecks`](#dtemplate-api-healthcheckextensions-addhealthchecks)
- [`MapHealthCheckEndPoints`](#dtemplate-api-healthcheckextensions-maphealthcheckendpoints)

**Métodos**
#### <a id="dtemplate-api-healthcheckextensions-addhealthchecks"></a>`AddHealthChecks(IServiceCollection services, string connectionString)`
- **Descripción:** Registra checks de salud básicos (autochequeo “Self”).
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
| `services` | [`IServiceCollection`](https://learn.microsoft.com/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection) | Contenedor de servicios. |
  | `connectionString` | `string` | Cadena de conexión; actualmente no se usa. |
- **Devuelve:** No aplica.
- **Ejemplo:**
```csharp
services.AddHealthChecks(connectionString);
```

#### <a id="dtemplate-api-healthcheckextensions-maphealthcheckendpoints"></a>`MapHealthCheckEndPoints(IEndpointRouteBuilder endpoints)`
- **Descripción:** Mapea endpoints `/health`, `/health/live` y `/health/ready`.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
| `endpoints` | [`IEndpointRouteBuilder`](https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.routing.iendpointroutebuilder) | Enrutador de endpoints. |
- **Devuelve:** No aplica.
- **Ejemplo:**
```csharp
endpoints.MapHealthCheckEndPoints();
```

---

### DTemplate.Api MediatorExtensions
**Descripción:** Extensiones para integrar `IMediator` con los bridges de `Spider`.

**Índice de métodos**
- [`AsMediator`](#dtemplate-api-mediatorextensions-asmediator)
- [`Send<TRequest>`](#dtemplate-api-mediatorextensions-send)
- [`Send<TRequest, TResponse>`](#dtemplate-api-mediatorextensions-send-response)
- [`DefaultForwading<TRequest>`](#dtemplate-api-mediatorextensions-defaultforwarding)
- [`DefaultForwading<TRequest, TResponse>`](#dtemplate-api-mediatorextensions-defaultforwarding-response)
- [`DefaultSend<TRequest>`](#dtemplate-api-mediatorextensions-defaultsend)
- [`DefaultSend<TResponse>`](#dtemplate-api-mediatorextensions-defaultsend-response)

**Métodos**
#### <a id="dtemplate-api-mediatorextensions-asmediator"></a>`AsMediator(ISpider spider)`
- **Descripción:** Inicializa un bridge hacia `IMediator`.
- **Parámetros:**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `spider` | `ISpider` | Instancia de spider. |
- **Devuelve:** `IServiceBridge<IMediator>`.
- **Ejemplo:**
```csharp
var mediatorBridge = spider.AsMediator();
```

#### <a id="dtemplate-api-mediatorextensions-send"></a>`Send<TRequest>(IServiceBridge<IMediator, TRequest> bridge, TRequest request, CancellationToken cancellationToken = default)`
- **Descripción:** Envía un request sin respuesta mediante el bridge.
- **Parámetros:**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `bridge` | `IServiceBridge<IMediator, TRequest>` | Bridge configurado. |
  | `request` | `TRequest` | Request a enviar. |
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación. |
- **Devuelve:** `Task`.
- **Ejemplo:**
```csharp
await bridge.Send(new Ping());
```

#### <a id="dtemplate-api-mediatorextensions-send-response"></a>`Send<TRequest, TResponse>(IServiceBridge<IMediator, TRequest, TResponse> bridge, TRequest request, CancellationToken cancellationToken = default)`
- **Descripción:** Envía un request con respuesta mediante el bridge.
- **Parámetros:**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `bridge` | `IServiceBridge<IMediator, TRequest, TResponse>` | Bridge configurado. |
  | `request` | `TRequest` | Request a enviar. |
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación. |
- **Devuelve:** `Task<TResponse>`.
- **Ejemplo:**
```csharp
var response = await bridge.Send<FindOrder, OrderDto>(query);
```

#### <a id="dtemplate-api-mediatorextensions-defaultforwarding"></a>`DefaultForwading<TRequest>(IServiceBridge<IMediator> bridge)`
- **Descripción:** Configura el bridge con pipeline de forwarding por defecto.
- **Parámetros:**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `bridge` | `IServiceBridge<IMediator>` | Bridge base. |
- **Devuelve:** `IServiceBridge<IMediator, TRequest>`.
- **Ejemplo:**
```csharp
var forwarding = bridge.DefaultForwading<MyCommand>();
```

#### <a id="dtemplate-api-mediatorextensions-defaultforwarding-response"></a>`DefaultForwading<TRequest, TResponse>(IServiceBridge<IMediator> bridge)`
- **Descripción:** Configura forwarding para requests con respuesta.
- **Parámetros:**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `bridge` | `IServiceBridge<IMediator>` | Bridge base. |
- **Devuelve:** `IServiceBridge<IMediator, TRequest, TResponse>`.
- **Ejemplo:**
```csharp
var forwarding = bridge.DefaultForwading<GetOrder, OrderDto>();
```

#### <a id="dtemplate-api-mediatorextensions-defaultsend"></a>`DefaultSend<TRequest>(ISpider spider, TRequest request, CancellationToken cancellationToken = default)`
- **Descripción:** Envía un request usando forwarding por defecto.
- **Parámetros:**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `spider` | `ISpider` | Spider. |
  | `request` | `TRequest` | Request. |
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación. |
- **Devuelve:** `Task`.
- **Ejemplo:**
```csharp
await spider.DefaultSend(new Ping());
```

#### <a id="dtemplate-api-mediatorextensions-defaultsend-response"></a>`DefaultSend<TResponse>(ISpider spider, IRequest<TResponse> request, CancellationToken cancellationToken = default)`
- **Descripción:** Envía un request con respuesta usando forwarding por defecto.
- **Parámetros:**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `spider` | `ISpider` | Spider. |
  | `request` | `IRequest<TResponse>` | Request. |
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación. |
- **Devuelve:** `Task<TResponse>`.
- **Ejemplo:**
```csharp
var response = await spider.DefaultSend(new GetOrder());
```

---

### DTemplate.Api SwaggerConstants
**Descripción:** Constantes internas para configuración de Swagger.

**Índice de métodos:** No aplica.

**Propiedades**
| Nombre | Tipo | Descripción |
| --- | --- | --- |
| `Docs.ApiName` | `string` | Nombre del API con versión. |
| `Docs.ApiVersion` | `string` | Versión del API. |

**Métodos**
No aplica.

---

### DTemplate.Api CIdSchemaFilter
**Descripción:** Ajusta el esquema de [`CId`](#dtemplatedomain-cid) en Swagger como string.

**Índice de métodos**
- [`Apply`](#dtemplate-api-cidschemafilter-apply)

**Métodos**
#### <a id="dtemplate-api-cidschemafilter-apply"></a>`Apply(OpenApiSchema schema, SchemaFilterContext context)`
- **Descripción:** Cambia tipo, formato y ejemplo cuando el tipo es [`CId`](#dtemplatedomain-cid).
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `schema` | `OpenApiSchema` | Esquema a modificar. |
  | `context` | `SchemaFilterContext` | Contexto del filtro. |
- **Devuelve:** No aplica.
- **Ejemplo:**
```csharp
services.AddSwaggerGen(o => o.SchemaFilter<CIdSchemaFilter>());
```

---

### DTemplate.Api RemoveVersionParametersFilter
**Descripción:** Elimina el parámetro `version` de operaciones Swagger.

**Índice de métodos**
- [`Apply`](#dtemplate-api-removeversionparametersfilter-apply)

**Métodos**
#### <a id="dtemplate-api-removeversionparametersfilter-apply"></a>`Apply(OpenApiOperation operation, OperationFilterContext context)`
- **Descripción:** Busca y remueve el parámetro `version` del listado.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `operation` | `OpenApiOperation` | Operación Swagger. |
  | `context` | `OperationFilterContext` | Contexto del filtro. |
- **Devuelve:** No aplica.
- **Ejemplo:**
```csharp
options.OperationFilter<RemoveVersionParametersFilter>();
```

---

### DTemplate.Api SetVersionInPathsFilter
**Descripción:** Reemplaza `v{version}` en rutas Swagger por la versión real.

**Índice de métodos**
- [`Apply`](#dtemplate-api-setversioninpathsfilter-apply)

**Métodos**
#### <a id="dtemplate-api-setversioninpathsfilter-apply"></a>`Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)`
- **Descripción:** Reescribe las rutas del documento con la versión concreta.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `swaggerDoc` | `OpenApiDocument` | Documento Swagger. |
  | `context` | `DocumentFilterContext` | Contexto del filtro. |
- **Devuelve:** No aplica.
- **Ejemplo:**
```csharp
options.DocumentFilter<SetVersionInPathsFilter>();
```

---

# DTemplate.Business

## DTemplate.Business visión general
Biblioteca de dominio de negocio que implementa patrones CQRS con `Pelican.Mediator`, adaptadores de almacenamiento, validación, mapeo y comportamientos transaccionales. Contiene contratos, respuestas paginadas, excepciones y extensiones para DI.

## DTemplate.Business componentes

### DTemplate.Business Constants
**Descripción:** Marcador de ensamblado para registros de DI y validadores.

**Índice de métodos:** No aplica.

**Casos de uso:**
- Referenciar ensamblado desde `AddPelican` o `AddValidatorsFromAssembly`.

**Propiedades y métodos:** No aplica.

---

### DTemplate.Business MappingProfile
**Descripción:** Perfil base de AutoMapper.

**Índice de métodos**
- [`MappingProfile`](#dtemplate-business-mappingprofile-ctor)
- [`OpportunityMapping`](#dtemplate-business-mappingprofile-opportunitymapping)

**Métodos**
#### <a id="dtemplate-business-mappingprofile-ctor"></a>`MappingProfile()`
- **Descripción:** Inicializa el perfil y registra los mapeos internos.
- **Parámetros:** No aplica.
- **Devuelve:** No aplica.
- **Excepciones:** No lanza explícitamente.
- **Ejemplo:**
```csharp
services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());
```

#### <a id="dtemplate-business-mappingprofile-opportunitymapping"></a>`OpportunityMapping()`
- **Descripción:** Punto de extensión interno para agregar configuraciones de mapeo (actualmente vacío).
- **Parámetros:** No aplica.
- **Devuelve:** No aplica.
- **Excepciones:** No aplica.
- **Ejemplo:**
```csharp
// Se implementa en el perfil cuando se agreguen mapeos.
```

---

### DTemplate.Business BaseRequest
**Descripción:** Request base con identificador [`CId`](#dtemplatedomain-cid).

**Índice de métodos:** No aplica.

**Propiedades**
| Nombre | Tipo | Descripción |
| --- | --- | --- |
| `Id` | [`CId`](#dtemplatedomain-cid) | Identificador del recurso. |

---

### DTemplate.Business BaseResponse
**Descripción:** Response base con identificador [`CId`](#dtemplatedomain-cid).

**Índice de métodos:** No aplica.

**Propiedades**
| Nombre | Tipo | Descripción |
| --- | --- | --- |
| `Id` | [`CId`](#dtemplatedomain-cid) | Identificador del recurso. |

---

### DTemplate.Business PagedResponse
**Descripción:** Respuesta paginada con metadatos y resultados.

**Índice de métodos:** No aplica.

**Propiedades**
| Nombre | Tipo | Descripción |
| --- | --- | --- |
| `CurrentPage` | `int` | Número de página actual. |
| `PageSize` | `int` | Tamaño de página. |
| `PageCount` | `int` | Total de páginas. |
| `RowCount` | `long` | Total de filas. |
| `Results` | [`IEnumerable<TResponse>`](https://learn.microsoft.com/dotnet/api/system.collections.generic.ienumerable-1) | Resultados de la página. |

---

### DTemplate.Business GetOneCriteria
**Descripción:** Criterios para obtener una única entidad.

**Índice de métodos:** No aplica.

**Propiedades**
| Nombre | Tipo | Descripción |
| --- | --- | --- |
| `FiltersExpression` | [`Expression<Func<TEntity, bool>>`](https://learn.microsoft.com/dotnet/api/system.linq.expressions.expression-1) | Filtro por expresión. |
| `Filters` | `string` | Filtro en cadena. |
| `UseTracking` | `bool` | Indica si usa tracking. |

---

### DTemplate.Business GetManyCriteria
**Descripción:** Criterios para obtener múltiples entidades con filtros, orden, y paginado.

**Índice de métodos**
- [`UseFiltersExpression`](#dtemplate-business-getmanycriteria-usefiltersexpression)
- [`UseFilters`](#dtemplate-business-getmanycriteria-usefilters)
- [`UseSortingExpression`](#dtemplate-business-getmanycriteria-usesortingexpression)
- [`UseSorts`](#dtemplate-business-getmanycriteria-usesorts)
- [`UsePaging`](#dtemplate-business-getmanycriteria-usepaging)

**Propiedades**
| Nombre | Tipo | Descripción |
| --- | --- | --- |
| `FiltersExpression` | [`Expression<Func<TEntity, bool>>`](https://learn.microsoft.com/dotnet/api/system.linq.expressions.expression-1) | Filtro por expresión. |
| `SortingExpression` | [`Expression<Func<TEntity, object>>`](https://learn.microsoft.com/dotnet/api/system.linq.expressions.expression-1) | Ordenamiento por expresión. |
| `AscendentSort` | `bool` | Indica orden ascendente. |
| `Filters` | `string` | Filtro en cadena. |
| `Sorts` | `string` | Ordenamiento en cadena. |
| `UseTracking` | `bool` | Indica si usa tracking. |
| `PageSize` | `int?` | Tamaño de página. |
| `PageNumber` | `int?` | Número de página. |

**Métodos**
#### <a id="dtemplate-business-getmanycriteria-usefiltersexpression"></a>`UseFiltersExpression()`
- **Descripción:** Indica si existe filtro por expresión.
- **Parámetros:** No aplica.
- **Devuelve:** `bool` indicando si `FiltersExpression` está definido.
- **Excepciones:** No aplica.
- **Ejemplo:**
```csharp
if (criteria.UseFiltersExpression()) { /* aplicar filtros */ }
```

#### <a id="dtemplate-business-getmanycriteria-usefilters"></a>`UseFilters()`
- **Descripción:** Indica si existe filtro en cadena.
- **Parámetros:** No aplica.
- **Devuelve:** `bool` indicando si `Filters` tiene valor.
- **Excepciones:** No aplica.

#### <a id="dtemplate-business-getmanycriteria-usesortingexpression"></a>`UseSortingExpression()`
- **Descripción:** Indica si existe orden por expresión.
- **Parámetros:** No aplica.
- **Devuelve:** `bool` indicando si `SortingExpression` está definido.
- **Excepciones:** No aplica.

#### <a id="dtemplate-business-getmanycriteria-usesorts"></a>`UseSorts()`
- **Descripción:** Indica si existe orden en cadena.
- **Parámetros:** No aplica.
- **Devuelve:** `bool` indicando si `Sorts` tiene valor.
- **Excepciones:** No aplica.

#### <a id="dtemplate-business-getmanycriteria-usepaging"></a>`UsePaging()`
- **Descripción:** Indica si se aplicará paginado.
- **Parámetros:** No aplica.
- **Devuelve:** `bool` indicando si `PageSize` y `PageNumber` son válidos.
- **Excepciones:** No aplica.

---

### DTemplate.Business BatchResult
**Descripción:** Resultado de lote paginado.

**Índice de métodos**
- [`AsEnumerable`](#dtemplate-business-batchresult-asenumerable)

**Propiedades**
| Nombre | Tipo | Descripción |
| --- | --- | --- |
| `PageSize` | `int` | Tamaño de página. |
| `PageNumber` | `int` | Página actual. |
| `RowCount` | `long` | Total de filas. |
| `PageCount` | `int` | Total de páginas. |
| `Results` | [`IEnumerable<T>`](https://learn.microsoft.com/dotnet/api/system.collections.generic.ienumerable-1) | Resultados. |

**Métodos**
#### <a id="dtemplate-business-batchresult-asenumerable"></a>`AsEnumerable()`
- **Descripción:** Devuelve resultados o un enumerable vacío cuando `Results` es nulo.
- **Parámetros:** No aplica.
- **Devuelve:** `IEnumerable<T>` con los elementos disponibles.
- **Excepciones:** No aplica.
- **Ejemplo:**
```csharp
var items = batch.AsEnumerable();
```

---

### DTemplate.Business IStorageReaderAdapter
**Descripción:** Contrato para lectura de entidades con filtros, orden y paginado.

**Índice de métodos**
- [`GetOneAsync<TEntity, TExpected>`](#dtemplate-business-istoragereaderadapter-getoneasync)
- [`GetManyAsync<TEntity, TExpected>`](#dtemplate-business-istoragereaderadapter-getmanyasync)

**Métodos**
#### <a id="dtemplate-business-istoragereaderadapter-getoneasync"></a>`GetOneAsync<TEntity, TExpected>(GetOneCriteria<TEntity> criteria, CancellationToken cancellationToken = default)`
- **Descripción:** Obtiene una entidad según criterios.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `criteria` | [`GetOneCriteria<TEntity>`](#dtemplatebusiness-getonecriteria) | Criterios de selección. |
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación. |
- **Devuelve:** `Task<TExpected>` con la entidad o proyección encontrada (puede ser nulo si no existe).
- **Excepciones:** Puede lanzar `ArgumentException` según implementación.
- **Ejemplo:**
```csharp
var entity = await reader.GetOneAsync<Entity, Dto>(criteria);
```

#### <a id="dtemplate-business-istoragereaderadapter-getmanyasync"></a>`GetManyAsync<TEntity, TExpected>(GetManyCriteria<TEntity> criteria, CancellationToken cancellationToken = default)`
- **Descripción:** Obtiene múltiples entidades según criterios.
- **Parámetros:**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `criteria` | [`GetManyCriteria<TEntity>`](#dtemplatebusiness-getmanycriteria) | Criterios de selección. |
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación. |
- **Devuelve:** `Task<`[`BatchResult<TExpected>`](#dtemplatebusiness-batchresult)`>` con resultados y metadatos de paginación.
- **Ejemplo:**
```csharp
var batch = await reader.GetManyAsync<Entity, Dto>(criteria);
```

---

### DTemplate.Business IStorageWriterAdapter
**Descripción:** Contrato para operaciones de escritura en almacenamiento.

**Índice de métodos**
- [`SaveAsync<TEntity>`](#dtemplate-business-istoragewriteradapter-saveasync)
- [`UpdateAsync<TEntity>`](#dtemplate-business-istoragewriteradapter-updateasync)
- [`DeleteAsync<TEntity>`](#dtemplate-business-istoragewriteradapter-deleteasync)

**Métodos**
#### <a id="dtemplate-business-istoragewriteradapter-saveasync"></a>`SaveAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)`
- **Descripción:** Guarda una entidad nueva en el almacenamiento.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `entity` | `TEntity` | Entidad a guardar. |
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación. |
- **Devuelve:** `Task` que completa cuando la entidad es persistida.
- **Excepciones:** Puede propagar excepciones del proveedor de datos.

#### <a id="dtemplate-business-istoragewriteradapter-updateasync"></a>`UpdateAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)`
- **Descripción:** Persiste cambios en una entidad.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `entity` | `TEntity` | Entidad a actualizar. |
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación. |
- **Devuelve:** `Task` que completa al guardar cambios.
- **Excepciones:** Puede propagar excepciones del proveedor de datos.

#### <a id="dtemplate-business-istoragewriteradapter-deleteasync"></a>`DeleteAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)`
- **Descripción:** Elimina una entidad.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `entity` | `TEntity` | Entidad a eliminar. |
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación. |
- **Devuelve:** `Task` que completa al eliminar.
- **Excepciones:** Puede propagar excepciones del proveedor de datos.

---

### DTemplate.Business IMapperAdapter
**Descripción:** Contrato de mapeo entre tipos.

**Índice de métodos**
- [`MapAsync<TSource, TDestination>`](#dtemplate-business-imapperadapter-mapasync)
- [`UpdateMapAsync<TSource, TDestination>`](#dtemplate-business-imapperadapter-updatemapasync)

**Métodos**
#### <a id="dtemplate-business-imapperadapter-mapasync"></a>`MapAsync<TSource, TDestination>(TSource source, CancellationToken cancellationToken = default)`
- **Descripción:** Mapea el objeto de origen a un nuevo objeto de destino.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `source` | `TSource` | Instancia de origen. |
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación. |
- **Devuelve:** `ValueTask<TDestination>` con el resultado del mapeo.
- **Excepciones:** Puede propagar excepciones de configuración de mapeo.

#### <a id="dtemplate-business-imapperadapter-updatemapasync"></a>`UpdateMapAsync<TSource, TDestination>(TSource source, TDestination destination, CancellationToken cancellationToken = default)`
- **Descripción:** Aplica valores del origen sobre un destino existente.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `source` | `TSource` | Objeto origen. |
  | `destination` | `TDestination` | Objeto destino a actualizar. |
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación. |
- **Devuelve:** `ValueTask` que completa al finalizar el mapeo.
- **Excepciones:** Puede propagar excepciones de configuración de mapeo.

---

### DTemplate.Business IValidatorAdapter
**Descripción:** Contrato de validación de modelos.

**Índice de métodos**
- [`ValidateAsync<TModel>`](#dtemplate-business-ivalidadapter-validateasync)

**Métodos**
#### <a id="dtemplate-business-ivalidadapter-validateasync"></a>`ValidateAsync<TModel>(TModel model, CancellationToken cancellationToken = default)`
- **Descripción:** Valida el modelo especificado.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `model` | `TModel` | Modelo a validar. |
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación. |
- **Devuelve:** `ValueTask` que completa cuando la validación termina.
- **Excepciones:** Puede lanzar excepciones de validación según la implementación.

---

### DTemplate.Business ValidatorAdapter
**Descripción:** Implementación de `IValidatorAdapter` con FluentValidation.

**Índice de métodos**
- [`ValidatorAdapter`](#dtemplate-business-validatoradapter-ctor)
- [`ValidateAsync<TModel>`](#dtemplate-business-validatoradapter-validateasync)

**Métodos**
#### <a id="dtemplate-business-validatoradapter-ctor"></a>`ValidatorAdapter(IServiceProvider serviceProvider)`
- **Descripción:** Inicializa el adaptador con un proveedor de servicios.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
| `serviceProvider` | [`IServiceProvider`](https://learn.microsoft.com/dotnet/api/system.iserviceprovider) | Contenedor de servicios para resolver validadores. |
- **Devuelve:** No aplica.
- **Excepciones:** `ArgumentNullException` si `serviceProvider` es nulo.

#### <a id="dtemplate-business-validatoradapter-validateasync"></a>`ValidateAsync<TModel>(TModel model, CancellationToken cancellationToken = default)`
- **Descripción:** Resuelve un `IValidator<TModel>` y valida el modelo.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `model` | `TModel` | Modelo a validar. |
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación. |
- **Devuelve:** `ValueTask` que completa al finalizar la validación.
- **Excepciones:** `InvalidOperationException` si no hay validador registrado.

---

### DTemplate.Business MapperAdapter
**Descripción:** Implementación de `IMapperAdapter` usando AutoMapper.

**Índice de métodos**
- [`MapperAdapter`](#dtemplate-business-mapperadapter-ctor)
- [`MapAsync<TSource, TDestination>`](#dtemplate-business-mapperadapter-mapasync)
- [`UpdateMapAsync<TSource, TDestination>`](#dtemplate-business-mapperadapter-updatemapasync)

**Métodos**
#### <a id="dtemplate-business-mapperadapter-ctor"></a>`MapperAdapter(IMapper mapper)`
- **Descripción:** Inicializa el adaptador con una instancia de AutoMapper.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `mapper` | `IMapper` | Motor de mapeo. |
- **Devuelve:** No aplica.
- **Excepciones:** `ArgumentNullException` si `mapper` es nulo.

#### <a id="dtemplate-business-mapperadapter-mapasync"></a>`MapAsync<TSource, TDestination>(TSource source, CancellationToken cancellationToken = default)`
- **Descripción:** Ejecuta el mapeo y devuelve el destino.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `source` | `TSource` | Instancia de origen. |
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación. |
- **Devuelve:** `ValueTask<TDestination>` con el objeto mapeado.
- **Excepciones:** Puede propagar excepciones de mapeo.

#### <a id="dtemplate-business-mapperadapter-updatemapasync"></a>`UpdateMapAsync<TSource, TDestination>(TSource source, TDestination destination, CancellationToken cancellationToken = default)`
- **Descripción:** Aplica el mapeo al objeto destino existente.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `source` | `TSource` | Origen de datos. |
  | `destination` | `TDestination` | Destino a actualizar. |
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación. |
- **Devuelve:** `ValueTask` que completa al finalizar.
- **Excepciones:** Puede propagar excepciones de mapeo.

---

### DTemplate.Business StorageReaderAdapter
**Descripción:** Lee entidades desde [`IDbContext`](#dtemplatepersistence-idbcontext) con filtros, orden y paginado.

**Índice de métodos**
- [`StorageReaderAdapter`](#dtemplate-business-storagereaderadapter-ctor)
- [`GetOneAsync<TEntity, TExpected>`](#dtemplate-business-storagereaderadapter-getoneasync)
- [`GetManyAsync<TEntity, TExpected>`](#dtemplate-business-storagereaderadapter-getmanyasync)
- [`ApplyExpressions<TEntity>`](#dtemplate-business-storagereaderadapter-applyexpressions)
- [`ApplySieve<TEntity>`](#dtemplate-business-storagereaderadapter-applysieve)
- [`ApplyPaging<TEntity>`](#dtemplate-business-storagereaderadapter-applypaging)

**Métodos**
#### <a id="dtemplate-business-storagereaderadapter-ctor"></a>`StorageReaderAdapter(ISieveProcessor sieveProcessor, IDbContext dbContext, IMapper mapper)`
- **Descripción:** Inicializa el adaptador con dependencias de filtrado, acceso a datos y mapeo.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `sieveProcessor` | `ISieveProcessor` | Motor Sieve para filtros/orden. |
  | `dbContext` | `IDbContext` | Contexto de datos. |
  | `mapper` | `IMapper` | Motor de mapeo. |
- **Devuelve:** No aplica.
- **Excepciones:** `ArgumentNullException` si alguna dependencia es nula.

#### <a id="dtemplate-business-storagereaderadapter-getoneasync"></a>`GetOneAsync<TEntity, TExpected>(GetOneCriteria<TEntity> criteria, CancellationToken cancellationToken = default)`
- **Descripción:** Aplica filtros y devuelve la entidad o una proyección del tipo esperado.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `criteria` | [`GetOneCriteria<TEntity>`](#dtemplatebusiness-getonecriteria) | Criterios de consulta. |
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación. |
- **Devuelve:** `Task<TExpected>` con la entidad o proyección (puede ser nulo).
- **Excepciones:** `ArgumentNullException` si `criteria` es nulo; `ArgumentException` si `FiltersExpression` es nulo.

#### <a id="dtemplate-business-storagereaderadapter-getmanyasync"></a>`GetManyAsync<TEntity, TExpected>(GetManyCriteria<TEntity> criteria, CancellationToken cancellationToken = default)`
- **Descripción:** Aplica filtros, orden y paginado, devolviendo un lote con metadatos.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `criteria` | [`GetManyCriteria<TEntity>`](#dtemplatebusiness-getmanycriteria) | Criterios de consulta. |
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación. |
- **Devuelve:** `Task<BatchResult<TExpected>>` con resultados y paginación.
- **Excepciones:** Puede propagar excepciones de consulta o mapeo.

#### <a id="dtemplate-business-storagereaderadapter-applyexpressions"></a>`ApplyExpressions<TEntity>(IQueryable<TEntity> source, GetManyCriteria<TEntity> criteria)`
- **Descripción:** Aplica filtros y orden por expresiones LINQ.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
| `source` | [`IQueryable<TEntity>`](https://learn.microsoft.com/dotnet/api/system.linq.iqueryable-1) | Fuente de datos. |
  | `criteria` | [`GetManyCriteria<TEntity>`](#dtemplatebusiness-getmanycriteria) | Criterios con expresiones. |
- **Devuelve:** `IQueryable<TEntity>` con filtros/orden aplicados.
- **Excepciones:** No aplica.

#### <a id="dtemplate-business-storagereaderadapter-applysieve"></a>`ApplySieve<TEntity>(IQueryable<TEntity> source, GetManyCriteria<TEntity> criteria)`
- **Descripción:** Aplica filtros/orden usando Sieve si están definidos.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
| `source` | [`IQueryable<TEntity>`](https://learn.microsoft.com/dotnet/api/system.linq.iqueryable-1) | Fuente de datos. |
  | `criteria` | [`GetManyCriteria<TEntity>`](#dtemplatebusiness-getmanycriteria) | Criterios con filtros/orden. |
- **Devuelve:** `IQueryable<TEntity>` con filtros/orden Sieve aplicados.
- **Excepciones:** Puede propagar excepciones de Sieve.

#### <a id="dtemplate-business-storagereaderadapter-applypaging"></a>`ApplyPaging<TEntity>(IQueryable<TEntity> source, GetManyCriteria<TEntity> criteria)`
- **Descripción:** Aplica paginado y devuelve consulta y metadatos de paginación.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
| `source` | [`IQueryable<TEntity>`](https://learn.microsoft.com/dotnet/api/system.linq.iqueryable-1) | Fuente de datos. |
  | `criteria` | [`GetManyCriteria<TEntity>`](#dtemplatebusiness-getmanycriteria) | Criterios con paginación. |
- **Devuelve:** Tupla con query, `rowCount`, `pageCount`, `pageNumber`, `pageSize`.
- **Excepciones:** No aplica.

**Ejemplo**
```csharp
var criteria = new GetManyCriteria<Order> { PageSize = 20, PageNumber = 1 };
var batch = await reader.GetManyAsync<Order, OrderDto>(criteria);
```

---

### DTemplate.Business StorageWriterAdapter
**Descripción:** Implementación de escritura usando [`IDbContext`](#dtemplatepersistence-idbcontext).

**Índice de métodos**
- [`StorageWriterAdapter`](#dtemplate-business-storagewriteradapter-ctor)
- [`SaveAsync<TEntity>`](#dtemplate-business-storagewriteradapter-saveasync)
- [`UpdateAsync<TEntity>`](#dtemplate-business-storagewriteradapter-updateasync)
- [`DeleteAsync<TEntity>`](#dtemplate-business-storagewriteradapter-deleteasync)

**Métodos**
#### <a id="dtemplate-business-storagewriteradapter-ctor"></a>`StorageWriterAdapter(IDbContext dbContext)`
- **Descripción:** Inicializa el adaptador con el contexto de datos.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `dbContext` | `IDbContext` | Contexto para persistir datos. |
- **Devuelve:** No aplica.
- **Excepciones:** `ArgumentNullException` si `dbContext` es nulo.

#### <a id="dtemplate-business-storagewriteradapter-saveasync"></a>`SaveAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)`
- **Descripción:** Agrega la entidad al contexto y guarda cambios.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `entity` | `TEntity` | Entidad a guardar. |
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación. |
- **Devuelve:** `Task` que completa tras persistir.
- **Excepciones:** Puede propagar excepciones de guardado.

#### <a id="dtemplate-business-storagewriteradapter-updateasync"></a>`UpdateAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)`
- **Descripción:** Persiste cambios en la entidad usando el contexto.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `entity` | `TEntity` | Entidad a actualizar. |
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación. |
- **Devuelve:** `Task` que completa al guardar.
- **Excepciones:** Puede propagar excepciones de guardado.

#### <a id="dtemplate-business-storagewriteradapter-deleteasync"></a>`DeleteAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)`
- **Descripción:** Elimina la entidad y guarda cambios.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `entity` | `TEntity` | Entidad a eliminar. |
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación. |
- **Devuelve:** `Task` que completa tras eliminar.
- **Excepciones:** Puede propagar excepciones de guardado.

---

### DTemplate.Business BaseCommandHandler
**Descripción:** Manejador base de comandos con respuesta.

**Índice de métodos**
- [`Handle`](#dtemplate-business-basecommandhandler-handle)

**Métodos**
#### <a id="dtemplate-business-basecommandhandler-handle"></a>`Handle(TRequest request, CancellationToken cancellationToken = default)`
- **Descripción:** Maneja el comando y devuelve la respuesta.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TRequest` | Comando a procesar. |
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación. |
- **Devuelve:** `Task<TResponse>` con la respuesta generada.
- **Excepciones:** Depende de la implementación concreta.

---

### DTemplate.Business NoReturnCommandHandler
**Descripción:** Manejador base de comandos sin respuesta.

**Índice de métodos**
- [`Handle`](#dtemplate-business-noreturncommandhandler-handle)

**Métodos**
#### <a id="dtemplate-business-noreturncommandhandler-handle"></a>`Handle(TRequest request, CancellationToken cancellationToken = default)`
- **Descripción:** Maneja el comando sin respuesta explícita.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TRequest` | Comando a procesar. |
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación. |
- **Devuelve:** `Task` que completa cuando finaliza el manejo.
- **Excepciones:** Depende de la implementación concreta.

---

### DTemplate.Business CreateCommandHandler (con respuesta)
**Descripción:** Maneja creación con validación, mapeo, guardado y respuesta.

**Índice de métodos**
- [`CreateCommandHandler`](#dtemplate-business-createcommandhandler-response-ctor)
- [`Handle`](#dtemplate-business-createcommandhandler-response-handle)
- [`ValidateAsync`](#dtemplate-business-createcommandhandler-response-validateasync)
- [`MapToEntityAsync`](#dtemplate-business-createcommandhandler-response-maptoentityasync)
- [`SaveEntityAsync`](#dtemplate-business-createcommandhandler-response-saveentityasync)
- [`MapToResponseAsync`](#dtemplate-business-createcommandhandler-response-maptresponse)

**Propiedades**
| Nombre | Tipo | Descripción |
| --- | --- | --- |
| `Services` | [`IServiceProvider`](https://learn.microsoft.com/dotnet/api/system.iserviceprovider) | Proveedor de servicios. |
| `StorageWriterAdapter` | [`IStorageWriterAdapter`](#dtemplatebusiness-istoragewriteradapter) | Escritura en almacenamiento. |
| `StorageReaderAdapter` | [`IStorageReaderAdapter`](#dtemplatebusiness-istoragereaderadapter) | Lectura en almacenamiento. |
| `ValidatorAdapter` | [`IValidatorAdapter`](#dtemplatebusiness-ivalidadapter) | Validación de requests. |
| `MapperAdapter` | [`IMapperAdapter`](#dtemplatebusiness-imapperadapter) | Mapeo de modelos. |
| `ValidateRequest` | `bool` | Indica si valida request. |
| `UseProjectionFromStorage` | `bool` | Indica si usa proyección en lectura. |

**Métodos**
#### <a id="dtemplate-business-createcommandhandler-response-ctor"></a>`CreateCommandHandler(IServiceProvider serviceProvider)`
- **Descripción:** Inicializa dependencias del handler.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
| `serviceProvider` | [`IServiceProvider`](https://learn.microsoft.com/dotnet/api/system.iserviceprovider) | Proveedor de servicios.
- **Devuelve:** No aplica.
- **Excepciones:** `ArgumentNullException` si es nulo.

#### <a id="dtemplate-business-createcommandhandler-response-handle"></a>`Handle(TRequest request, CancellationToken cancellationToken = default)`
- **Descripción:** Orquesta validación, mapeo, guardado y respuesta.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TRequest` | Request de creación.
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación.
- **Devuelve:** `Task<TResponse>` con la respuesta creada.
- **Excepciones:** Puede propagar errores de validación o persistencia.

#### <a id="dtemplate-business-createcommandhandler-response-validateasync"></a>`ValidateAsync(TRequest request, CancellationToken cancellationToken)`
- **Descripción:** Valida el request cuando `ValidateRequest` es verdadero.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TRequest` | Request a validar.
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación.
- **Devuelve:** `ValueTask` que completa al validar.
- **Excepciones:** Puede lanzar excepciones de validación.

#### <a id="dtemplate-business-createcommandhandler-response-maptoentityasync"></a>`MapToEntityAsync(TRequest request, CancellationToken cancellationToken)`
- **Descripción:** Mapea el request a entidad.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TRequest` | Request origen.
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación.
- **Devuelve:** `ValueTask<TEntity>` con la entidad mapeada.
- **Excepciones:** Puede propagar errores de mapeo.

#### <a id="dtemplate-business-createcommandhandler-response-saveentityasync"></a>`SaveEntityAsync(TRequest request, TEntity entity, CancellationToken cancellationToken)`
- **Descripción:** Persiste la entidad.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TRequest` | Request asociado.
  | `entity` | `TEntity` | Entidad a guardar.
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación.
- **Devuelve:** `Task` al completar el guardado.
- **Excepciones:** Puede propagar errores de persistencia.

#### <a id="dtemplate-business-createcommandhandler-response-maptresponse"></a>`MapToResponseAsync(TRequest request, TEntity entity, CancellationToken cancellationToken)`
- **Descripción:** Mapea la entidad a respuesta o consulta proyección.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TRequest` | Request asociado.
  | `entity` | `TEntity` | Entidad persistida.
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación.
- **Devuelve:** `ValueTask<TResponse>` con la respuesta.
- **Excepciones:** Puede propagar errores de mapeo o consulta.

**Ejemplo**
```csharp
public class CreateOrderHandler : CreateCommandHandler<CreateOrder, OrderResponse, Order>
{
    public CreateOrderHandler(IServiceProvider sp) : base(sp) { }
}
```

---

### DTemplate.Business CreateCommandHandler (sin respuesta)
**Descripción:** Maneja creación sin respuesta explícita.

**Índice de métodos**
- [`CreateCommandHandler`](#dtemplate-business-createcommandhandler-noreturn-ctor)
- [`Handle`](#dtemplate-business-createcommandhandler-noreturn-handle)
- [`ValidateAsync`](#dtemplate-business-createcommandhandler-noreturn-validateasync)
- [`MapToEntityAsync`](#dtemplate-business-createcommandhandler-noreturn-maptoentityasync)
- [`SaveEntityAsync`](#dtemplate-business-createcommandhandler-noreturn-saveentityasync)

**Propiedades**
| Nombre | Tipo | Descripción |
| --- | --- | --- |
| `Services` | [`IServiceProvider`](https://learn.microsoft.com/dotnet/api/system.iserviceprovider) | Proveedor de servicios. |
| `StorageWriterAdapter` | [`IStorageWriterAdapter`](#dtemplatebusiness-istoragewriteradapter) | Escritura en almacenamiento. |
| `ValidatorAdapter` | [`IValidatorAdapter`](#dtemplatebusiness-ivalidadapter) | Validación de requests. |
| `MapperAdapter` | [`IMapperAdapter`](#dtemplatebusiness-imapperadapter) | Mapeo de modelos. |
| `ValidateRequest` | `bool` | Indica si valida request. |

**Métodos:**
#### <a id="dtemplate-business-createcommandhandler-noreturn-ctor"></a>`CreateCommandHandler(IServiceProvider serviceProvider)`
- **Descripción:** Inicializa dependencias del handler.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
| `serviceProvider` | [`IServiceProvider`](https://learn.microsoft.com/dotnet/api/system.iserviceprovider) | Proveedor de servicios.
- **Devuelve:** No aplica.
- **Excepciones:** `ArgumentNullException` si es nulo.

#### <a id="dtemplate-business-createcommandhandler-noreturn-handle"></a>`Handle(TRequest request, CancellationToken cancellationToken = default)`
- **Descripción:** Orquesta validación, mapeo y persistencia.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TRequest` | Request de creación.
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación.
- **Devuelve:** `Task` al completar.
- **Excepciones:** Puede propagar errores de validación o persistencia.

#### <a id="dtemplate-business-createcommandhandler-noreturn-validateasync"></a>`ValidateAsync(TRequest request, CancellationToken cancellationToken)`
- **Descripción:** Valida el request cuando `ValidateRequest` es verdadero.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TRequest` | Request a validar.
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación.
- **Devuelve:** `ValueTask`.
- **Excepciones:** Puede lanzar excepciones de validación.

#### <a id="dtemplate-business-createcommandhandler-noreturn-maptoentityasync"></a>`MapToEntityAsync(TRequest request, CancellationToken cancellationToken)`
- **Descripción:** Mapea el request a entidad.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TRequest` | Request origen.
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación.
- **Devuelve:** `ValueTask<TEntity>` con la entidad mapeada.
- **Excepciones:** Puede propagar errores de mapeo.

#### <a id="dtemplate-business-createcommandhandler-noreturn-saveentityasync"></a>`SaveEntityAsync(TRequest request, TEntity entity, CancellationToken cancellationToken)`
- **Descripción:** Persiste la entidad.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TRequest` | Request asociado.
  | `entity` | `TEntity` | Entidad a guardar.
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación.
- **Devuelve:** `Task` al completar.
- **Excepciones:** Puede propagar errores de persistencia.

---

### DTemplate.Business DeleteCommandHandler (con respuesta)
**Descripción:** Maneja eliminación con respuesta.

**Índice de métodos**
- [`DeleteCommandHandler`](#dtemplate-business-deletecommandhandler-response-ctor)
- [`Handle`](#dtemplate-business-deletecommandhandler-response-handle)
- [`GetEntityAsync`](#dtemplate-business-deletecommandhandler-response-getentityasync)
- [`ValidateAsync`](#dtemplate-business-deletecommandhandler-response-validateasync)
- [`DeleteEntityAsync`](#dtemplate-business-deletecommandhandler-response-deleteentityasync)
- [`BuildResponseAsync`](#dtemplate-business-deletecommandhandler-response-buildresponseasync)

**Propiedades**
| Nombre | Tipo | Descripción |
| --- | --- | --- |
| `Services` | [`IServiceProvider`](https://learn.microsoft.com/dotnet/api/system.iserviceprovider) | Proveedor de servicios. |
| `StorageWriterAdapter` | [`IStorageWriterAdapter`](#dtemplatebusiness-istoragewriteradapter) | Escritura en almacenamiento. |
| `StorageReaderAdapter` | [`IStorageReaderAdapter`](#dtemplatebusiness-istoragereaderadapter) | Lectura en almacenamiento. |
| `ValidatorAdapter` | [`IValidatorAdapter`](#dtemplatebusiness-ivalidadapter) | Validación de requests. |
| `MapperAdapter` | [`IMapperAdapter`](#dtemplatebusiness-imapperadapter) | Mapeo de modelos. |
| `ValidateRequest` | `bool` | Indica si valida request. |

**Métodos:**
#### <a id="dtemplate-business-deletecommandhandler-response-ctor"></a>`DeleteCommandHandler(IServiceProvider serviceProvider)`
- **Descripción:** Inicializa dependencias del handler.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
| `serviceProvider` | [`IServiceProvider`](https://learn.microsoft.com/dotnet/api/system.iserviceprovider) | Proveedor de servicios.
- **Devuelve:** No aplica.
- **Excepciones:** `ArgumentNullException` si es nulo.

#### <a id="dtemplate-business-deletecommandhandler-response-handle"></a>`Handle(TRequest request, CancellationToken cancellationToken = default)`
- **Descripción:** Obtiene, valida, elimina y construye la respuesta.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TRequest` | Request de eliminación.
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación.
- **Devuelve:** `Task<TResponse>` con la respuesta.
- **Excepciones:** Puede propagar `NotFoundException` o errores de persistencia.

#### <a id="dtemplate-business-deletecommandhandler-response-getentityasync"></a>`GetEntityAsync(TRequest request, CancellationToken cancellationToken)`
- **Descripción:** Recupera la entidad a eliminar.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TRequest` | Request con el Id.
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación.
- **Devuelve:** `Task<TEntity>` con la entidad.
- **Excepciones:** `NotFoundException` si no existe.

#### <a id="dtemplate-business-deletecommandhandler-response-validateasync"></a>`ValidateAsync(TRequest request, TEntity entity, CancellationToken cancellationToken)`
- **Descripción:** Valida request/entidad cuando `ValidateRequest` es verdadero.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TRequest` | Request a validar.
  | `entity` | `TEntity` | Entidad a validar.
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación.
- **Devuelve:** `ValueTask`.
- **Excepciones:** Puede lanzar excepciones de validación.

#### <a id="dtemplate-business-deletecommandhandler-response-deleteentityasync"></a>`DeleteEntityAsync(TEntity entity, CancellationToken cancellationToken)`
- **Descripción:** Elimina la entidad en almacenamiento.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `entity` | `TEntity` | Entidad a eliminar.
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación.
- **Devuelve:** `Task` al completar.
- **Excepciones:** Puede propagar errores de persistencia.

#### <a id="dtemplate-business-deletecommandhandler-response-buildresponseasync"></a>`BuildResponseAsync(TRequest request, TEntity entity, CancellationToken cancellationToken)`
- **Descripción:** Construye la respuesta final tras la eliminación.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TRequest` | Request asociado.
  | `entity` | `TEntity` | Entidad eliminada.
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación.
- **Devuelve:** `ValueTask<TResponse>` con la respuesta.
- **Excepciones:** Depende de la implementación concreta.

---

### DTemplate.Business DeleteCommandHandler (sin respuesta)
**Descripción:** Maneja eliminación sin respuesta.

**Índice de métodos**
- [`DeleteCommandHandler`](#dtemplate-business-deletecommandhandler-noreturn-ctor)
- [`Handle`](#dtemplate-business-deletecommandhandler-noreturn-handle)
- [`GetEntityAsync`](#dtemplate-business-deletecommandhandler-noreturn-getentityasync)
- [`ValidateAsync`](#dtemplate-business-deletecommandhandler-noreturn-validateasync)
- [`DeleteEntityAsync`](#dtemplate-business-deletecommandhandler-noreturn-deleteentityasync)

**Propiedades**
| Nombre | Tipo | Descripción |
| --- | --- | --- |
| `Services` | [`IServiceProvider`](https://learn.microsoft.com/dotnet/api/system.iserviceprovider) | Proveedor de servicios. |
| `StorageWriterAdapter` | [`IStorageWriterAdapter`](#dtemplatebusiness-istoragewriteradapter) | Escritura en almacenamiento. |
| `StorageReaderAdapter` | [`IStorageReaderAdapter`](#dtemplatebusiness-istoragereaderadapter) | Lectura en almacenamiento. |
| `ValidatorAdapter` | [`IValidatorAdapter`](#dtemplatebusiness-ivalidadapter) | Validación de requests. |
| `MapperAdapter` | [`IMapperAdapter`](#dtemplatebusiness-imapperadapter) | Mapeo de modelos. |
| `ValidateRequest` | `bool` | Indica si valida request. |

**Métodos:**
#### <a id="dtemplate-business-deletecommandhandler-noreturn-ctor"></a>`DeleteCommandHandler(IServiceProvider serviceProvider)`
- **Descripción:** Inicializa dependencias del handler.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
| `serviceProvider` | [`IServiceProvider`](https://learn.microsoft.com/dotnet/api/system.iserviceprovider) | Proveedor de servicios.
- **Devuelve:** No aplica.
- **Excepciones:** `ArgumentNullException` si es nulo.

#### <a id="dtemplate-business-deletecommandhandler-noreturn-handle"></a>`Handle(TRequest request, CancellationToken cancellationToken = default)`
- **Descripción:** Obtiene, valida y elimina la entidad.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TRequest` | Request de eliminación.
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación.
- **Devuelve:** `Task` al completar.
- **Excepciones:** Puede propagar `NotFoundException` o errores de persistencia.

#### <a id="dtemplate-business-deletecommandhandler-noreturn-getentityasync"></a>`GetEntityAsync(TRequest request, CancellationToken cancellationToken)`
- **Descripción:** Recupera la entidad a eliminar.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TRequest` | Request con el Id.
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación.
- **Devuelve:** `Task<TEntity>` con la entidad.
- **Excepciones:** `NotFoundException` si no existe.

#### <a id="dtemplate-business-deletecommandhandler-noreturn-validateasync"></a>`ValidateAsync(TRequest request, TEntity entity, CancellationToken cancellationToken)`
- **Descripción:** Valida request/entidad cuando `ValidateRequest` es verdadero.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TRequest` | Request a validar.
  | `entity` | `TEntity` | Entidad a validar.
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación.
- **Devuelve:** `ValueTask`.
- **Excepciones:** Puede lanzar excepciones de validación.

#### <a id="dtemplate-business-deletecommandhandler-noreturn-deleteentityasync"></a>`DeleteEntityAsync(TEntity entity, CancellationToken cancellationToken)`
- **Descripción:** Elimina la entidad en almacenamiento.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `entity` | `TEntity` | Entidad a eliminar.
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación.
- **Devuelve:** `Task` al completar.
- **Excepciones:** Puede propagar errores de persistencia.

---

### DTemplate.Business UpdateCommandHandler (con respuesta)
**Descripción:** Maneja actualización con respuesta.

**Índice de métodos**
- [`UpdateCommandHandler`](#dtemplate-business-updatecommandhandler-response-ctor)
- [`Handle`](#dtemplate-business-updatecommandhandler-response-handle)
- [`GetEntityAsync`](#dtemplate-business-updatecommandhandler-response-getentityasync)
- [`ValidateAsync`](#dtemplate-business-updatecommandhandler-response-validateasync)
- [`MapEntityAsync`](#dtemplate-business-updatecommandhandler-response-mapentityasync)
- [`UpdateEntityAsync`](#dtemplate-business-updatecommandhandler-response-updateentityasync)
- [`MapToResponseAsync`](#dtemplate-business-updatecommandhandler-response-maptresponse)

**Propiedades**
| Nombre | Tipo | Descripción |
| --- | --- | --- |
| `Services` | [`IServiceProvider`](https://learn.microsoft.com/dotnet/api/system.iserviceprovider) | Proveedor de servicios. |
| `StorageWriterAdapter` | [`IStorageWriterAdapter`](#dtemplatebusiness-istoragewriteradapter) | Escritura en almacenamiento. |
| `StorageReaderAdapter` | [`IStorageReaderAdapter`](#dtemplatebusiness-istoragereaderadapter) | Lectura en almacenamiento. |
| `ValidatorAdapter` | [`IValidatorAdapter`](#dtemplatebusiness-ivalidadapter) | Validación de requests. |
| `MapperAdapter` | [`IMapperAdapter`](#dtemplatebusiness-imapperadapter) | Mapeo de modelos. |
| `ValidateRequest` | `bool` | Indica si valida request. |
| `UseProjectionFromStorage` | `bool` | Indica si usa proyección en lectura. |

**Métodos:**
#### <a id="dtemplate-business-updatecommandhandler-response-ctor"></a>`UpdateCommandHandler(IServiceProvider serviceProvider)`
- **Descripción:** Inicializa dependencias del handler.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `serviceProvider` | [`IServiceProvider`](https://learn.microsoft.com/dotnet/api/system.iserviceprovider) | Proveedor de servicios.
- **Devuelve:** No aplica.
- **Excepciones:** `ArgumentNullException` si es nulo.

#### <a id="dtemplate-business-updatecommandhandler-response-handle"></a>`Handle(TRequest request, CancellationToken cancellationToken = default)`
- **Descripción:** Obtiene, valida, actualiza y responde.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TRequest` | Request de actualización.
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación.
- **Devuelve:** `Task<TResponse>` con la respuesta.
- **Excepciones:** Puede propagar `NotFoundException` o errores de persistencia.

#### <a id="dtemplate-business-updatecommandhandler-response-getentityasync"></a>`GetEntityAsync(TRequest request, CancellationToken cancellationToken)`
- **Descripción:** Recupera la entidad a actualizar.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TRequest` | Request con el Id.
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación.
- **Devuelve:** `Task<TEntity>` con la entidad.
- **Excepciones:** `NotFoundException` si no existe.

#### <a id="dtemplate-business-updatecommandhandler-response-validateasync"></a>`ValidateAsync(TRequest request, TEntity entity, CancellationToken cancellationToken)`
- **Descripción:** Valida request/entidad cuando `ValidateRequest` es verdadero.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TRequest` | Request a validar.
  | `entity` | `TEntity` | Entidad a validar.
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación.
- **Devuelve:** `ValueTask`.
- **Excepciones:** Puede lanzar excepciones de validación.

#### <a id="dtemplate-business-updatecommandhandler-response-mapentityasync"></a>`MapEntityAsync(TRequest request, TEntity entity, CancellationToken cancellationToken)`
- **Descripción:** Aplica el mapeo del request sobre la entidad.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TRequest` | Request con datos actualizados.
  | `entity` | `TEntity` | Entidad a modificar.
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación.
- **Devuelve:** `ValueTask`.
- **Excepciones:** Puede propagar errores de mapeo.

#### <a id="dtemplate-business-updatecommandhandler-response-updateentityasync"></a>`UpdateEntityAsync(TRequest request, TEntity entity, CancellationToken cancellationToken)`
- **Descripción:** Persiste la entidad actualizada.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TRequest` | Request asociado.
  | `entity` | `TEntity` | Entidad actualizada.
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación.
- **Devuelve:** `Task`.
- **Excepciones:** Puede propagar errores de persistencia.

#### <a id="dtemplate-business-updatecommandhandler-response-maptresponse"></a>`MapToResponseAsync(TRequest request, TEntity entity, CancellationToken cancellationToken)`
- **Descripción:** Mapea la entidad a respuesta o consulta proyección.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TRequest` | Request asociado.
  | `entity` | `TEntity` | Entidad actualizada.
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación.
- **Devuelve:** `ValueTask<TResponse>`.
- **Excepciones:** Puede propagar errores de mapeo o consulta.

---

### DTemplate.Business UpdateCommandHandler (sin respuesta)
**Descripción:** Maneja actualización sin respuesta.

**Índice de métodos**
- [`UpdateCommandHandler`](#dtemplate-business-updatecommandhandler-noreturn-ctor)
- [`Handle`](#dtemplate-business-updatecommandhandler-noreturn-handle)
- [`GetEntityAsync`](#dtemplate-business-updatecommandhandler-noreturn-getentityasync)
- [`ValidateAsync`](#dtemplate-business-updatecommandhandler-noreturn-validateasync)
- [`MapEntityAsync`](#dtemplate-business-updatecommandhandler-noreturn-mapentityasync)
- [`UpdateEntityAsync`](#dtemplate-business-updatecommandhandler-noreturn-updateentityasync)

**Métodos:**
#### <a id="dtemplate-business-updatecommandhandler-noreturn-ctor"></a>`UpdateCommandHandler(IServiceProvider serviceProvider)`
- **Descripción:** Inicializa dependencias del handler.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `serviceProvider` | [`IServiceProvider`](https://learn.microsoft.com/dotnet/api/system.iserviceprovider) | Proveedor de servicios.
- **Devuelve:** No aplica.
- **Excepciones:** `ArgumentNullException` si es nulo.

#### <a id="dtemplate-business-updatecommandhandler-noreturn-handle"></a>`Handle(TRequest request, CancellationToken cancellationToken = default)`
- **Descripción:** Obtiene, valida y actualiza la entidad.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TRequest` | Request de actualización.
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación.
- **Devuelve:** `Task`.
- **Excepciones:** Puede propagar `NotFoundException` o errores de persistencia.

#### <a id="dtemplate-business-updatecommandhandler-noreturn-getentityasync"></a>`GetEntityAsync(TRequest request, CancellationToken cancellationToken)`
- **Descripción:** Recupera la entidad a actualizar.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TRequest` | Request con el Id.
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación.
- **Devuelve:** `Task<TEntity>` con la entidad.
- **Excepciones:** `NotFoundException` si no existe.

#### <a id="dtemplate-business-updatecommandhandler-noreturn-validateasync"></a>`ValidateAsync(TRequest request, TEntity entity, CancellationToken cancellationToken)`
- **Descripción:** Valida request/entidad cuando `ValidateRequest` es verdadero.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TRequest` | Request a validar.
  | `entity` | `TEntity` | Entidad a validar.
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación.
- **Devuelve:** `ValueTask`.
- **Excepciones:** Puede lanzar excepciones de validación.

#### <a id="dtemplate-business-updatecommandhandler-noreturn-mapentityasync"></a>`MapEntityAsync(TRequest request, TEntity entity, CancellationToken cancellationToken)`
- **Descripción:** Aplica el mapeo del request sobre la entidad.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TRequest` | Request con datos actualizados.
  | `entity` | `TEntity` | Entidad a modificar.
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación.
- **Devuelve:** `ValueTask`.
- **Excepciones:** Puede propagar errores de mapeo.

#### <a id="dtemplate-business-updatecommandhandler-noreturn-updateentityasync"></a>`UpdateEntityAsync(TRequest request, TEntity entity, CancellationToken cancellationToken)`
- **Descripción:** Persiste la entidad actualizada.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TRequest` | Request asociado.
  | `entity` | `TEntity` | Entidad actualizada.
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación.
- **Devuelve:** `Task`.
- **Excepciones:** Puede propagar errores de persistencia.

---

### DTemplate.Business PatchCommandHandler (con respuesta)
**Descripción:** Maneja actualización parcial con respuesta.

**Índice de métodos**
- [`PatchCommandHandler`](#dtemplate-business-patchcommandhandler-response-ctor)
- [`Handle`](#dtemplate-business-patchcommandhandler-response-handle)
- [`GetEntityAsync`](#dtemplate-business-patchcommandhandler-response-getentityasync)
- [`ValidateAsync`](#dtemplate-business-patchcommandhandler-response-validateasync)
- [`PatchEntityAsync`](#dtemplate-business-patchcommandhandler-response-patchentityasync)
- [`UpdateEntityAsync`](#dtemplate-business-patchcommandhandler-response-updateentityasync)
- [`BuildResponseAsync`](#dtemplate-business-patchcommandhandler-response-buildresponseasync)

**Métodos:**
#### <a id="dtemplate-business-patchcommandhandler-response-ctor"></a>`PatchCommandHandler(IServiceProvider serviceProvider)`
- **Descripción:** Inicializa dependencias del handler.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
| `serviceProvider` | [`IServiceProvider`](https://learn.microsoft.com/dotnet/api/system.iserviceprovider) | Proveedor de servicios. |
- **Devuelve:** No aplica.
- **Excepciones:** `ArgumentNullException` si es nulo.

#### <a id="dtemplate-business-patchcommandhandler-response-handle"></a>`Handle(TRequest request, CancellationToken cancellationToken = default)`
- **Descripción:** Obtiene, valida, aplica el patch, actualiza y devuelve respuesta.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TRequest` | Request de patch.
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación.
- **Devuelve:** `Task<TResponse>` con la respuesta.
- **Excepciones:** Puede propagar `NotFoundException` o errores de persistencia.

#### <a id="dtemplate-business-patchcommandhandler-response-getentityasync"></a>`GetEntityAsync(TRequest request, CancellationToken cancellationToken)`
- **Descripción:** Recupera la entidad a modificar.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TRequest` | Request con el Id.
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación.
- **Devuelve:** `Task<TEntity>` con la entidad.
- **Excepciones:** `NotFoundException` si no existe.

#### <a id="dtemplate-business-patchcommandhandler-response-validateasync"></a>`ValidateAsync(TRequest request, TEntity entity, CancellationToken cancellationToken)`
- **Descripción:** Valida request/entidad cuando `ValidateRequest` es verdadero.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TRequest` | Request a validar.
  | `entity` | `TEntity` | Entidad a validar.
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación.
- **Devuelve:** `ValueTask`.
- **Excepciones:** Puede lanzar excepciones de validación.

#### <a id="dtemplate-business-patchcommandhandler-response-patchentityasync"></a>`PatchEntityAsync(TRequest request, TEntity entity, CancellationToken cancellationToken)`
- **Descripción:** Aplica los cambios parciales sobre la entidad.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TRequest` | Request con cambios parciales.
  | `entity` | `TEntity` | Entidad a modificar.
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación.
- **Devuelve:** `ValueTask`.
- **Excepciones:** Depende de la implementación concreta.

#### <a id="dtemplate-business-patchcommandhandler-response-updateentityasync"></a>`UpdateEntityAsync(TRequest request, TEntity entity, CancellationToken cancellationToken)`
- **Descripción:** Persiste la entidad actualizada.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TRequest` | Request asociado.
  | `entity` | `TEntity` | Entidad actualizada.
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación.
- **Devuelve:** `Task`.
- **Excepciones:** Puede propagar errores de persistencia.

#### <a id="dtemplate-business-patchcommandhandler-response-buildresponseasync"></a>`BuildResponseAsync(TRequest request, TEntity entity, CancellationToken cancellationToken)`
- **Descripción:** Construye la respuesta final tras el patch.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TRequest` | Request asociado.
  | `entity` | `TEntity` | Entidad modificada.
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación.
- **Devuelve:** `ValueTask<TResponse>` con la respuesta.
- **Excepciones:** Depende de la implementación concreta.

---

### DTemplate.Business PatchCommandHandler (sin respuesta)
**Descripción:** Maneja actualización parcial sin respuesta.

**Índice de métodos**
- [`PatchCommandHandler`](#dtemplate-business-patchcommandhandler-noreturn-ctor)
- [`Handle`](#dtemplate-business-patchcommandhandler-noreturn-handle)
- [`GetEntityAsync`](#dtemplate-business-patchcommandhandler-noreturn-getentityasync)
- [`ValidateAsync`](#dtemplate-business-patchcommandhandler-noreturn-validateasync)
- [`PatchEntityAsync`](#dtemplate-business-patchcommandhandler-noreturn-patchentityasync)
- [`UpdateEntityAsync`](#dtemplate-business-patchcommandhandler-noreturn-updateentityasync)

**Métodos:**
#### <a id="dtemplate-business-patchcommandhandler-noreturn-ctor"></a>`PatchCommandHandler(IServiceProvider serviceProvider)`
- **Descripción:** Inicializa dependencias del handler.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
| `serviceProvider` | [`IServiceProvider`](https://learn.microsoft.com/dotnet/api/system.iserviceprovider) | Proveedor de servicios. |
- **Devuelve:** No aplica.
- **Excepciones:** `ArgumentNullException` si es nulo.

#### <a id="dtemplate-business-patchcommandhandler-noreturn-handle"></a>`Handle(TRequest request, CancellationToken cancellationToken = default)`
- **Descripción:** Obtiene, valida, aplica patch y actualiza la entidad.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TRequest` | Request de patch.
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación.
- **Devuelve:** `Task`.
- **Excepciones:** Puede propagar `NotFoundException` o errores de persistencia.

#### <a id="dtemplate-business-patchcommandhandler-noreturn-getentityasync"></a>`GetEntityAsync(TRequest request, CancellationToken cancellationToken)`
- **Descripción:** Recupera la entidad a modificar.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TRequest` | Request con el Id.
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación.
- **Devuelve:** `Task<TEntity>` con la entidad.
- **Excepciones:** `NotFoundException` si no existe.

#### <a id="dtemplate-business-patchcommandhandler-noreturn-validateasync"></a>`ValidateAsync(TRequest request, TEntity entity, CancellationToken cancellationToken)`
- **Descripción:** Valida request/entidad cuando `ValidateRequest` es verdadero.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TRequest` | Request a validar.
  | `entity` | `TEntity` | Entidad a validar.
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación.
- **Devuelve:** `ValueTask`.
- **Excepciones:** Puede lanzar excepciones de validación.

#### <a id="dtemplate-business-patchcommandhandler-noreturn-patchentityasync"></a>`PatchEntityAsync(TRequest request, TEntity entity, CancellationToken cancellationToken)`
- **Descripción:** Aplica los cambios parciales sobre la entidad.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TRequest` | Request con cambios parciales.
  | `entity` | `TEntity` | Entidad a modificar.
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación.
- **Devuelve:** `ValueTask`.
- **Excepciones:** Depende de la implementación concreta.

#### <a id="dtemplate-business-patchcommandhandler-noreturn-updateentityasync"></a>`UpdateEntityAsync(TRequest request, TEntity entity, CancellationToken cancellationToken)`
- **Descripción:** Persiste la entidad actualizada.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TRequest` | Request asociado.
  | `entity` | `TEntity` | Entidad actualizada.
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación.
- **Devuelve:** `Task`.
- **Excepciones:** Puede propagar errores de persistencia.

---

### DTemplate.Business GetOneQuery
**Descripción:** Query base para obtener una entidad por valor.

**Índice de métodos:** No aplica.

**Propiedades**
| Nombre | Tipo | Descripción |
| --- | --- | --- |
| `Value` | `TValue` | Valor de búsqueda. |

---

### DTemplate.Business GetOneQueryHandler
**Descripción:** Manejador base para `GetOneQuery`.

**Índice de métodos**
- [`GetOneQueryHandler`](#dtemplate-business-getonequeryhandler-ctor)
- [`Handle`](#dtemplate-business-getonequeryhandler-handle)
- [`GetFilterExpression`](#dtemplate-business-getonequeryhandler-getfilterexpression)

**Propiedades**
| Nombre | Tipo | Descripción |
| --- | --- | --- |
| `Services` | [`IServiceProvider`](https://learn.microsoft.com/dotnet/api/system.iserviceprovider) | Proveedor de servicios. |
| `StorageReaderAdapter` | [`IStorageReaderAdapter`](#dtemplatebusiness-istoragereaderadapter) | Lectura en almacenamiento. |

**Métodos:**
#### <a id="dtemplate-business-getonequeryhandler-ctor"></a>`GetOneQueryHandler(IServiceProvider serviceProvider)`
- **Descripción:** Inicializa dependencias del handler.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
| `serviceProvider` | [`IServiceProvider`](https://learn.microsoft.com/dotnet/api/system.iserviceprovider) | Proveedor de servicios. |
- **Devuelve:** No aplica.
- **Excepciones:** `ArgumentNullException` si es nulo.

#### <a id="dtemplate-business-getonequeryhandler-handle"></a>`Handle(TQuery request, CancellationToken cancellationToken = default)`
- **Descripción:** Ejecuta la consulta y devuelve la respuesta.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TQuery` | Query a procesar. |
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación. |
- **Devuelve:** `Task<TResponse>` con el resultado.
- **Excepciones:** Puede propagar errores de consulta o mapeo.

#### <a id="dtemplate-business-getonequeryhandler-getfilterexpression"></a>`GetFilterExpression(TQuery request)`
- **Descripción:** Construye la expresión de filtro para la consulta.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TQuery` | Query con criterios.
- **Devuelve:** `Expression<Func<TEntity, bool>>` con el filtro.
- **Excepciones:** Depende de la implementación concreta.

---

### DTemplate.Business GetByIdQuery
**Descripción:** Query para obtener una entidad por [`CId`](#dtemplatedomain-cid).

**Índice de métodos**
- [`GetByIdQuery`](#dtemplate-business-getbyidquery-ctor)

**Métodos**
#### <a id="dtemplate-business-getbyidquery-ctor"></a>`GetByIdQuery(CId id)`
- **Descripción:** Inicializa la query con el identificador.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `id` | [`CId`](#dtemplatedomain-cid) | Identificador del recurso. |
- **Devuelve:** No aplica.
- **Excepciones:** No aplica.

---

### DTemplate.Business GetByIdQueryHandler
**Descripción:** Manejador base para obtener entidades por [`CId`](#dtemplatedomain-cid).

**Índice de métodos**
- [`GetByIdQueryHandler`](#dtemplate-business-getbyidqueryhandler-ctor)
- [`GetFilterExpression`](#dtemplate-business-getbyidqueryhandler-getfilterexpression)

**Métodos**
#### <a id="dtemplate-business-getbyidqueryhandler-ctor"></a>`GetByIdQueryHandler(IServiceProvider serviceProvider)`
- **Descripción:** Inicializa dependencias del handler.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
| `serviceProvider` | [`IServiceProvider`](https://learn.microsoft.com/dotnet/api/system.iserviceprovider) | Proveedor de servicios. |
- **Devuelve:** No aplica.
- **Excepciones:** `ArgumentNullException` si es nulo.

#### <a id="dtemplate-business-getbyidqueryhandler-getfilterexpression"></a>`GetFilterExpression(TQuery request)`
- **Descripción:** Genera filtro por identificador.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TQuery` | Query con el Id. |
- **Devuelve:** `Expression<Func<TEntity, bool>>`.
- **Excepciones:** Depende de la implementación concreta.

---

### DTemplate.Business GetManyQuery
**Descripción:** Query base para obtener múltiples entidades.

**Índice de métodos:** No aplica.

**Propiedades**
| Nombre | Tipo | Descripción |
| --- | --- | --- |
| `Filters` | `string` | Filtros en cadena. |
| `Sorts` | `string` | Orden en cadena. |

---

### DTemplate.Business GetManyQueryHandler
**Descripción:** Manejador base para queries múltiples.

**Índice de métodos**
- [`GetManyQueryHandler`](#dtemplate-business-getmanyqueryhandler-ctor)
- [`Handle`](#dtemplate-business-getmanyqueryhandler-handle)
- [`GetFilterExpression`](#dtemplate-business-getmanyqueryhandler-getfilterexpression)
- [`GetSortingExpression`](#dtemplate-business-getmanyqueryhandler-getsortingexpression)

**Métodos**
#### <a id="dtemplate-business-getmanyqueryhandler-ctor"></a>`GetManyQueryHandler(IServiceProvider serviceProvider)`
- **Descripción:** Inicializa dependencias del handler.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
| `serviceProvider` | [`IServiceProvider`](https://learn.microsoft.com/dotnet/api/system.iserviceprovider) | Proveedor de servicios. |
- **Devuelve:** No aplica.
- **Excepciones:** `ArgumentNullException` si es nulo.

#### <a id="dtemplate-business-getmanyqueryhandler-handle"></a>`Handle(TQuery request, CancellationToken cancellationToken = default)`
- **Descripción:** Ejecuta la consulta y devuelve un lote de resultados.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TQuery` | Query a procesar. |
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación. |
- **Devuelve:** `Task<BatchResult<TResponse>>` con resultados.
- **Excepciones:** Puede propagar errores de consulta o mapeo.

#### <a id="dtemplate-business-getmanyqueryhandler-getfilterexpression"></a>`GetFilterExpression(TQuery query)`
- **Descripción:** Construye la expresión de filtro para la consulta.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `query` | `TQuery` | Query con criterios.
- **Devuelve:** `Expression<Func<TEntity, bool>>`.
- **Excepciones:** Depende de la implementación concreta.

#### <a id="dtemplate-business-getmanyqueryhandler-getsortingexpression"></a>`GetSortingExpression(TQuery query)`
- **Descripción:** Construye la expresión de ordenamiento.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `query` | `TQuery` | Query con orden.
- **Devuelve:** `Expression<Func<TEntity, object>>`.
- **Excepciones:** Depende de la implementación concreta.

---

### DTemplate.Business PagedSettings
**Descripción:** Configuración de paginado.

**Índice de métodos:** No aplica.

**Propiedades**
| Nombre | Tipo | Descripción |
| --- | --- | --- |
| `Filters` | `string` | Filtros en cadena. |
| `Sorts` | `string` | Orden en cadena. |
| `PageSize` | `int?` | Tamaño de página. |
| `PageNumber` | `int?` | Número de página. |

---

### DTemplate.Business GetPagedInfoQuery
**Descripción:** Query para resultados paginados.

**Índice de métodos**
- [`GetPagedInfoQuery`](#dtemplate-business-getpagedinfoquery-ctor)

**Propiedades**
| Nombre | Tipo | Descripción |
| --- | --- | --- |
| `PagedSettings` | [`PagedSettings`](#dtemplatebusiness-pagedsettings) | Configuración de paginado. |

**Métodos**
#### <a id="dtemplate-business-getpagedinfoquery-ctor"></a>`GetPagedInfoQuery(PagedSettings pagedSettings)`
- **Descripción:** Inicializa la query con la configuración de paginado.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `pagedSettings` | [`PagedSettings`](#dtemplatebusiness-pagedsettings) | Configuración de paginado. |
- **Devuelve:** No aplica.
- **Excepciones:** `ArgumentNullException` si es nulo.

---

### DTemplate.Business GetPagedInfoQueryHandler
**Descripción:** Manejador base para queries paginados.

**Índice de métodos**
- [`GetPagedInfoQueryHandler`](#dtemplate-business-getpagedinfoqueryhandler-ctor)
- [`Handle`](#dtemplate-business-getpagedinfoqueryhandler-handle)
- [`GetFiltersExpression`](#dtemplate-business-getpagedinfoqueryhandler-getfiltersexpression)
- [`GetSortingExpression`](#dtemplate-business-getpagedinfoqueryhandler-getsortingexpression)

**Propiedades**
| Nombre | Tipo | Descripción |
| --- | --- | --- |
| `ServiceProvider` | [`IServiceProvider`](https://learn.microsoft.com/dotnet/api/system.iserviceprovider) | Proveedor de servicios. |
| `StorageReaderAdapter` | [`IStorageReaderAdapter`](#dtemplatebusiness-istoragereaderadapter) | Lectura en almacenamiento. |
| `DefaultPageSize` | `int` | Tamaño de página por defecto. |
| `DefaultPageNumber` | `int` | Número de página por defecto. |
| `DefaultSorts` | `string` | Ordenamiento por defecto. |

**Métodos**
#### <a id="dtemplate-business-getpagedinfoqueryhandler-ctor"></a>`GetPagedInfoQueryHandler(IServiceProvider serviceProvider)`
- **Descripción:** Inicializa dependencias del handler.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
| `serviceProvider` | [`IServiceProvider`](https://learn.microsoft.com/dotnet/api/system.iserviceprovider) | Proveedor de servicios. |
- **Devuelve:** No aplica.
- **Excepciones:** `ArgumentNullException` si es nulo.

#### <a id="dtemplate-business-getpagedinfoqueryhandler-handle"></a>`Handle(TQuery request, CancellationToken cancellationToken = default)`
- **Descripción:** Ejecuta la consulta paginada y devuelve respuesta.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TQuery` | Query a procesar. |
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación. |
- **Devuelve:** `Task<`[`PagedResponse<TResponse>`](#dtemplatebusiness-pagedresponse)`>` con resultados paginados.
- **Excepciones:** Puede propagar errores de consulta o mapeo.

#### <a id="dtemplate-business-getpagedinfoqueryhandler-getfiltersexpression"></a>`GetFiltersExpression(TQuery request)`
- **Descripción:** Construye la expresión de filtros para la consulta.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TQuery` | Query con criterios.
- **Devuelve:** `Expression<Func<TEntity, bool>>`.
- **Excepciones:** Depende de la implementación concreta.

#### <a id="dtemplate-business-getpagedinfoqueryhandler-getsortingexpression"></a>`GetSortingExpression(TQuery request)`
- **Descripción:** Construye la expresión de ordenamiento.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TQuery` | Query con orden.
- **Devuelve:** `Expression<Func<TEntity, object>>`.
- **Excepciones:** Depende de la implementación concreta.

---

### DTemplate.Business TransactionPipelineBehavior
**Descripción:** Envuelve la ejecución de requests en una transacción.

**Índice de métodos**
- [`Handle`](#dtemplate-business-transactionpipelinebehavior-handle)

**Métodos**
#### <a id="dtemplate-business-transactionpipelinebehavior-handle"></a>`Handle(TRequest request, Handler<TResponse> next, CancellationToken cancellationToken = default)`
- **Descripción:** Inicia una transacción, ejecuta el handler siguiente y confirma o revierte según el resultado.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `request` | `TRequest` | Request a procesar. |
  | `next` | `Handler<TResponse>` | Delegado al siguiente handler. |
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación. |
- **Devuelve:** `Task<TResponse>` con la respuesta del pipeline.
- **Excepciones:** Puede propagar excepciones durante la transacción o el handler.

---

### DTemplate.Business ServicesExtensions
**Descripción:** Extensiones de DI para registrar servicios de negocio.

**Índice de métodos**
- [`AddBusiness`](#dtemplate-business-servicesextensions-addbusiness)

**Métodos**
#### <a id="dtemplate-business-servicesextensions-addbusiness"></a>`AddBusiness(IServiceCollection services)`
- **Descripción:** Registra adaptadores, comportamientos y servicios de negocio.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
| `services` | [`IServiceCollection`](https://learn.microsoft.com/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection) | Contenedor de servicios. |
- **Devuelve:** `IServiceCollection` con registros agregados.
- **Excepciones:** Puede propagar errores de registro.

---

### DTemplate.Business HttpException
**Descripción:** Excepción base con código HTTP asociado.

**Índice de métodos**
- [`HttpException(HttpStatusCode statusCode)`](#dtemplate-business-httpexception-ctor)
- [`HttpException(HttpStatusCode statusCode, string message)`](#dtemplate-business-httpexception-ctor-message)

**Propiedades**
| Nombre | Tipo | Descripción |
| --- | --- | --- |
| `StatusCode` | [`HttpStatusCode`](https://learn.microsoft.com/dotnet/api/system.net.httpstatuscode) | Código HTTP. |

**Métodos**
#### <a id="dtemplate-business-httpexception-ctor"></a>`HttpException(HttpStatusCode statusCode)`
- **Descripción:** Inicializa la excepción con el código HTTP.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
| `statusCode` | [`HttpStatusCode`](https://learn.microsoft.com/dotnet/api/system.net.httpstatuscode) | Código HTTP asociado. |
- **Devuelve:** No aplica.
- **Excepciones:** No aplica.

#### <a id="dtemplate-business-httpexception-ctor-message"></a>`HttpException(HttpStatusCode statusCode, string message)`
- **Descripción:** Inicializa la excepción con código HTTP y mensaje.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
| `statusCode` | [`HttpStatusCode`](https://learn.microsoft.com/dotnet/api/system.net.httpstatuscode) | Código HTTP asociado. |
  | `message` | `string` | Mensaje de error. |
- **Devuelve:** No aplica.
- **Excepciones:** No aplica.

---

### DTemplate.Business BadRequestException
**Descripción:** Excepción para solicitudes inválidas (400).

**Índice de métodos**
- [`BadRequestException()`](#dtemplate-business-badrequestexception-ctor)
- [`BadRequestException(string message)`](#dtemplate-business-badrequestexception-ctor-message)

**Métodos**
#### <a id="dtemplate-business-badrequestexception-ctor"></a>`BadRequestException()`
- **Descripción:** Crea una excepción 400 sin mensaje personalizado.
- **Parámetros:** No aplica.
- **Devuelve:** No aplica.
- **Excepciones:** No aplica.

#### <a id="dtemplate-business-badrequestexception-ctor-message"></a>`BadRequestException(string message)`
- **Descripción:** Crea una excepción 400 con mensaje.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `message` | `string` | Mensaje de error. |
- **Devuelve:** No aplica.
- **Excepciones:** No aplica.

---

### DTemplate.Business NotFoundException
**Descripción:** Excepción para recursos no encontrados (404).

**Índice de métodos**
- [`NotFoundException(string resource, string key)`](#dtemplate-business-notfoundexception-ctor)
- [`NotFoundException(string message)`](#dtemplate-business-notfoundexception-ctor-message)

**Métodos**
#### <a id="dtemplate-business-notfoundexception-ctor"></a>`NotFoundException(string resource, string key)`
- **Descripción:** Crea una excepción 404 indicando recurso y clave.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `resource` | `string` | Nombre del recurso. |
  | `key` | `string` | Identificador buscado. |
- **Devuelve:** No aplica.
- **Excepciones:** No aplica.

#### <a id="dtemplate-business-notfoundexception-ctor-message"></a>`NotFoundException(string message)`
- **Descripción:** Crea una excepción 404 con mensaje.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `message` | `string` | Mensaje de error. |
- **Devuelve:** No aplica.
- **Excepciones:** No aplica.

---

### DTemplate.Business UnauthorizedException
**Descripción:** Excepción para solicitudes no autorizadas (401).

**Índice de métodos**
- [`UnauthorizedException(string user)`](#dtemplate-business-unauthorizedexception-ctor)
- [`UnauthorizedException(string user, string message)`](#dtemplate-business-unauthorizedexception-ctor-message)

**Métodos**
#### <a id="dtemplate-business-unauthorizedexception-ctor"></a>`UnauthorizedException(string user)`
- **Descripción:** Crea una excepción 401 para el usuario indicado.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `user` | `string` | Usuario implicado. |
- **Devuelve:** No aplica.
- **Excepciones:** No aplica.

#### <a id="dtemplate-business-unauthorizedexception-ctor-message"></a>`UnauthorizedException(string user, string message)`
- **Descripción:** Crea una excepción 401 con mensaje personalizado.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `user` | `string` | Usuario implicado. |
  | `message` | `string` | Mensaje de error. |
- **Devuelve:** No aplica.
- **Excepciones:** No aplica.

---

### DTemplate.Business ForbiddenException
**Descripción:** Excepción para accesos prohibidos (403).

**Índice de métodos**
- [`ForbiddenException(string resource, string user)`](#dtemplate-business-forbiddenexception-ctor)
- [`ForbiddenException(string message)`](#dtemplate-business-forbiddenexception-ctor-message)

**Métodos**
#### <a id="dtemplate-business-forbiddenexception-ctor"></a>`ForbiddenException(string resource, string user)`
- **Descripción:** Crea una excepción 403 para recurso y usuario.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `resource` | `string` | Recurso protegido. |
  | `user` | `string` | Usuario implicado. |
- **Devuelve:** No aplica.
- **Excepciones:** No aplica.

#### <a id="dtemplate-business-forbiddenexception-ctor-message"></a>`ForbiddenException(string message)`
- **Descripción:** Crea una excepción 403 con mensaje.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `message` | `string` | Mensaje de error. |
- **Devuelve:** No aplica.
- **Excepciones:** No aplica.

---

# DTemplate.Domain

## DTemplate.Domain visión general
Proyecto con entidades base y un identificador fuerte [`CId`](#dtemplatedomain-cid) configurable para persistencia, JSON y conversión.

## DTemplate.Domain componentes

### DTemplate.Domain IEntity
**Descripción:** Interfaz marcador de entidad.

**Índice de métodos:** No aplica.

---

### DTemplate.Domain IEntity<TKey>
**Descripción:** Interfaz de entidad con identificador tipado.

**Índice de métodos:** No aplica.

**Propiedades**
| Nombre | Tipo | Descripción |
| --- | --- | --- |
| `Id` | `TKey` | Identificador de entidad. |

---

### DTemplate.Domain BaseEntity
**Descripción:** Implementa [`IEntity`](#dtemplatedomain-ientity) con identificador [`CId`](#dtemplatedomain-cid).

**Índice de métodos:** No aplica.

**Propiedades**
| Nombre | Tipo | Descripción |
| --- | --- | --- |
| `Id` | [`CId`](#dtemplatedomain-cid) | Identificador de entidad. |

---

### DTemplate.Domain CId
**Descripción:** Struct de identificador fuerte con validación de tipo.

**Índice de métodos**
- [`CId()`](#dtemplate-domain-cid-ctor)
- [`CId(object value)`](#dtemplate-domain-cid-ctor-value)
- [`New()`](#dtemplate-domain-cid-new)
- [`Parse(string value)`](#dtemplate-domain-cid-parse)
- [`Cast<T>()`](#dtemplate-domain-cid-cast)
- [`ToString()`](#dtemplate-domain-cid-tostring)
- [`Equals(object obj)`](#dtemplate-domain-cid-equals)
- [`Equals(CId other)`](#dtemplate-domain-cid-equals-cid)
- [`GetHashCode()`](#dtemplate-domain-cid-gethashcode)
- [`operator ==` / `operator !=`](#dtemplate-domain-cid-operators)

**Propiedades**
| Nombre | Tipo | Descripción |
| --- | --- | --- |
| `Value` | `object` | Valor subyacente. |
| `Empty` | `CId` | Identificador vacío. |

**Métodos**
#### <a id="dtemplate-domain-cid-ctor"></a>`CId()`
- **Descripción:** Crea un identificador vacío.
- **Parámetros:** No aplica.
- **Devuelve:** No aplica.
- **Excepciones:** No aplica.

#### <a id="dtemplate-domain-cid-ctor-value"></a>`CId(object value)`
- **Descripción:** Crea un identificador a partir de un valor válido.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `value` | `object` | Valor compatible con la configuración. |
- **Devuelve:** No aplica.
- **Excepciones:** `InvalidOperationException` si el tipo no es permitido.

#### <a id="dtemplate-domain-cid-new"></a>`New()`
- **Descripción:** Crea un nuevo identificador usando la fábrica configurada.
- **Parámetros:** No aplica.
- **Devuelve:** `CId` con valor generado.
- **Excepciones:** `InvalidOperationException` si no hay configuración.

#### <a id="dtemplate-domain-cid-parse"></a>`Parse(string value)`
- **Descripción:** Convierte una cadena en `CId` usando el parse configurado.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `value` | `string` | Valor a convertir. |
- **Devuelve:** `CId` parseado.
- **Excepciones:** `InvalidOperationException` si no hay parse configurado.

#### <a id="dtemplate-domain-cid-cast"></a>`Cast<T>()`
- **Descripción:** Convierte el valor interno al tipo solicitado.
- **Parámetros:** No aplica.
- **Devuelve:** `T` con el valor convertido.
- **Excepciones:** `InvalidCastException` si el valor no es compatible.

#### <a id="dtemplate-domain-cid-tostring"></a>`ToString()`
- **Descripción:** Convierte el identificador a cadena.
- **Parámetros:** No aplica.
- **Devuelve:** `string` con el valor o vacío.
- **Excepciones:** No aplica.

#### <a id="dtemplate-domain-cid-equals"></a>`Equals(object obj)`
- **Descripción:** Compara el identificador con otro objeto.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `obj` | `object` | Objeto a comparar. |
- **Devuelve:** `bool` indicando igualdad.
- **Excepciones:** No aplica.

#### <a id="dtemplate-domain-cid-equals-cid"></a>`Equals(CId other)`
- **Descripción:** Compara con otro `CId`.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `other` | `CId` | Identificador a comparar. |
- **Devuelve:** `bool`.
- **Excepciones:** No aplica.

#### <a id="dtemplate-domain-cid-gethashcode"></a>`GetHashCode()`
- **Descripción:** Devuelve el hash del identificador.
- **Parámetros:** No aplica.
- **Devuelve:** `int`.
- **Excepciones:** No aplica.

#### <a id="dtemplate-domain-cid-operators"></a>`operator ==` / `operator !=`
- **Descripción:** Compara dos identificadores.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `left` | `CId` | Operando izquierdo. |
  | `right` | `CId` | Operando derecho. |
- **Devuelve:** `bool` indicando igualdad.
- **Excepciones:** No aplica.

---

### DTemplate.Domain CIdConfiguration
**Descripción:** Configuración de conversiones de [`CId`](#dtemplatedomain-cid).

**Índice de métodos**
- [`ValidateAndThrow()`](#dtemplate-domain-cidconfiguration-validateandthrow)

**Propiedades**
| Nombre | Tipo | Descripción |
| --- | --- | --- |
| `DefaultFactory` | [`Func<CId>`](https://learn.microsoft.com/dotnet/api/system.func-1) | Crea nuevos `CId`. |
| `ConvertToDb` | [`Expression<Func<CId, TDbType>>`](https://learn.microsoft.com/dotnet/api/system.linq.expressions.expression-1) | Convierte a tipo de base. |
| `ConvertFromDb` | [`Expression<Func<TDbType, CId>>`](https://learn.microsoft.com/dotnet/api/system.linq.expressions.expression-1) | Convierte desde base. |
| `ConvertToDbNullable` | [`Expression<Func<CId?, TDbType?>>`](https://learn.microsoft.com/dotnet/api/system.linq.expressions.expression-1) | Conversión nullable a base. |
| `ConvertFromDbNullable` | [`Expression<Func<TDbType?, CId?>>`](https://learn.microsoft.com/dotnet/api/system.linq.expressions.expression-1) | Conversión nullable desde base. |
| `JsonConverter` | [`Func<string, CId>`](https://learn.microsoft.com/dotnet/api/system.func-2) | Conversión desde JSON. |
| `NulleableJsonConverter` | [`Func<string, CId?>`](https://learn.microsoft.com/dotnet/api/system.func-2) | Conversión nullable desde JSON. |
| `ParseFunction` | [`Func<string, CId>`](https://learn.microsoft.com/dotnet/api/system.func-2) | Parseo desde cadena. |

**Métodos**
#### <a id="dtemplate-domain-cidconfiguration-validateandthrow"></a>`ValidateAndThrow()`
- **Descripción:** Valida la configuración y lanza excepción si falta algún componente.
- **Parámetros:** No aplica.
- **Devuelve:** No aplica.
- **Excepciones:** `InvalidOperationException` si la configuración es inválida.

---

### DTemplate.Domain CIdMetadata
**Descripción:** Metadatos globales para [`CId`](#dtemplatedomain-cid).

**Índice de métodos:** No aplica.

**Propiedades**
| Nombre | Tipo | Descripción |
| --- | --- | --- |
| `AllowedType` | [`Type`](https://learn.microsoft.com/dotnet/api/system.type) | Tipo permitido para el valor. |
| `DefaultFactory` | [`Func<CId>`](https://learn.microsoft.com/dotnet/api/system.func-1) | Fábrica por defecto. |
| `DbConverter` | [`ValueConverter`](https://learn.microsoft.com/dotnet/api/microsoft.entityframeworkcore.storage.valueconverter) | Conversión para DB. |
| `DbNulleableConverter` | [`ValueConverter`](https://learn.microsoft.com/dotnet/api/microsoft.entityframeworkcore.storage.valueconverter) | Conversión nullable para DB. |
| `JsonConverter` | [`Func<string, CId>`](https://learn.microsoft.com/dotnet/api/system.func-2) | Conversión desde JSON. |
| `NulleableJsonConverter` | [`Func<string, CId?>`](https://learn.microsoft.com/dotnet/api/system.func-2) | Conversión nullable desde JSON. |
| `ParseFunction` | [`Func<string, CId>`](https://learn.microsoft.com/dotnet/api/system.func-2) | Parseo desde cadena. |

---

### DTemplate.Domain CIdTypeConverter
**Descripción:** Convertidor de tipo para [`CId`](#dtemplatedomain-cid).

**Índice de métodos**
- [`CanConvertFrom`](#dtemplate-domain-cidtypeconverter-canconvertfrom)
- [`ConvertFrom`](#dtemplate-domain-cidtypeconverter-convertfrom)

**Métodos**
#### <a id="dtemplate-domain-cidtypeconverter-canconvertfrom"></a>`CanConvertFrom(ITypeDescriptorContext context, Type sourceType)`
- **Descripción:** Indica si el convertidor puede convertir desde el tipo dado.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
| `context` | [`ITypeDescriptorContext`](https://learn.microsoft.com/dotnet/api/system.componentmodel.itypedescriptorcontext) | Contexto de conversión. |
| `sourceType` | [`Type`](https://learn.microsoft.com/dotnet/api/system.type) | Tipo de origen. |
- **Devuelve:** `bool`.
- **Excepciones:** No aplica.

#### <a id="dtemplate-domain-cidtypeconverter-convertfrom"></a>`ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)`
- **Descripción:** Convierte un valor de origen a `CId`.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
| `context` | [`ITypeDescriptorContext`](https://learn.microsoft.com/dotnet/api/system.componentmodel.itypedescriptorcontext) | Contexto de conversión. |
| `culture` | [`CultureInfo`](https://learn.microsoft.com/dotnet/api/system.globalization.cultureinfo) | Cultura para la conversión. |
  | `value` | `object` | Valor a convertir. |
- **Devuelve:** `object` con el `CId` resultante.
- **Excepciones:** `NotSupportedException` si no puede convertir.

---

### DTemplate.Domain CIdJsonConverter
**Descripción:** Convertidor JSON para [`CId`](#dtemplatedomain-cid).

**Índice de métodos**
- [`Read`](#dtemplate-domain-cidjsonconverter-read)
- [`Write`](#dtemplate-domain-cidjsonconverter-write)

**Métodos**
#### <a id="dtemplate-domain-cidjsonconverter-read"></a>`Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)`
- **Descripción:** Lee un valor JSON y lo convierte a `CId`.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
| `reader` | [`Utf8JsonReader`](https://learn.microsoft.com/dotnet/api/system.text.json.utf8jsonreader) | Lector JSON. |
| `typeToConvert` | [`Type`](https://learn.microsoft.com/dotnet/api/system.type) | Tipo destino. |
| `options` | [`JsonSerializerOptions`](https://learn.microsoft.com/dotnet/api/system.text.json.jsonserializeroptions) | Opciones de serialización. |
- **Devuelve:** `CId` resultante.
- **Excepciones:** `JsonException` si el valor es inválido.

#### <a id="dtemplate-domain-cidjsonconverter-write"></a>`Write(Utf8JsonWriter writer, CId value, JsonSerializerOptions options)`
- **Descripción:** Escribe el `CId` como JSON.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
| `writer` | [`Utf8JsonWriter`](https://learn.microsoft.com/dotnet/api/system.text.json.utf8jsonwriter) | Escritor JSON. |
  | `value` | `CId` | Valor a serializar. |
| `options` | [`JsonSerializerOptions`](https://learn.microsoft.com/dotnet/api/system.text.json.jsonserializeroptions) | Opciones de serialización. |
- **Devuelve:** No aplica.
- **Excepciones:** `JsonException` si falla la escritura.

---

### DTemplate.Domain CIdNulleableJsonConverter
**Descripción:** Convertidor JSON para [`CId?`](#dtemplatedomain-cid).

**Índice de métodos**
- [`Read`](#dtemplate-domain-cidnulleablejsonconverter-read)
- [`Write`](#dtemplate-domain-cidnulleablejsonconverter-write)

**Métodos**
#### <a id="dtemplate-domain-cidnulleablejsonconverter-read"></a>`Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)`
- **Descripción:** Lee un valor JSON y lo convierte a `CId?`.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
| `reader` | [`Utf8JsonReader`](https://learn.microsoft.com/dotnet/api/system.text.json.utf8jsonreader) | Lector JSON. |
| `typeToConvert` | [`Type`](https://learn.microsoft.com/dotnet/api/system.type) | Tipo destino. |
| `options` | [`JsonSerializerOptions`](https://learn.microsoft.com/dotnet/api/system.text.json.jsonserializeroptions) | Opciones de serialización. |
- **Devuelve:** `CId?` resultante.
- **Excepciones:** `JsonException` si el valor es inválido.

#### <a id="dtemplate-domain-cidnulleablejsonconverter-write"></a>`Write(Utf8JsonWriter writer, CId? value, JsonSerializerOptions options)`
- **Descripción:** Escribe el `CId?` como JSON.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
| `writer` | [`Utf8JsonWriter`](https://learn.microsoft.com/dotnet/api/system.text.json.utf8jsonwriter) | Escritor JSON. |
  | `value` | `CId?` | Valor a serializar. |
| `options` | [`JsonSerializerOptions`](https://learn.microsoft.com/dotnet/api/system.text.json.jsonserializeroptions) | Opciones de serialización. |
- **Devuelve:** No aplica.
- **Excepciones:** `JsonException` si falla la escritura.

---

### DTemplate.Domain CIdDbValueGenerator
**Descripción:** Genera [`CId`](#dtemplatedomain-cid) al persistir entidades.

**Índice de métodos**
- [`NextValue`](#dtemplate-domain-ciddbvaluegenerator-nextvalue)

**Propiedades**
| Nombre | Tipo | Descripción |
| --- | --- | --- |
| `GeneratesTemporaryValues` | `bool` | Indica si los valores son temporales. |

**Métodos**
#### <a id="dtemplate-domain-ciddbvaluegenerator-nextvalue"></a>`NextValue(EntityEntry entry)`
- **Descripción:** Genera el siguiente valor `CId` para la entidad.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
| `entry` | [`EntityEntry`](https://learn.microsoft.com/dotnet/api/microsoft.entityframeworkcore.changetracking.entityentry) | Entrada de entidad en EF Core. |
- **Devuelve:** `object` con el nuevo identificador.
- **Excepciones:** Puede lanzar excepciones si no hay configuración.

---

### DTemplate.Domain ServiceExtensions
**Descripción:** Extensiones de DI para configurar [`CId`](#dtemplatedomain-cid) y [`CIdConfiguration`](#dtemplatedomain-cidconfiguration).

**Índice de métodos**
- [`UseCId<TTargetType, TDbType>`](#dtemplate-domain-serviceextensions-usecid)

**Métodos**
#### <a id="dtemplate-domain-serviceextensions-usecid"></a>`UseCId<TTargetType, TDbType>(IServiceCollection services, Action<CIdConfiguration<TTargetType, TDbType>> setup)`
- **Descripción:** Configura el identificador fuerte y registra convertidores en DI.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
| `services` | [`IServiceCollection`](https://learn.microsoft.com/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection) | Contenedor de servicios. |
| `setup` | [`Action<CIdConfiguration<TTargetType, TDbType>>`](https://learn.microsoft.com/dotnet/api/system.action-1) | Configuración del identificador. |
- **Devuelve:** `IServiceCollection`.
- **Excepciones:** `ArgumentNullException` si algún parámetro es nulo.

---

# DTemplate.Persistence

## DTemplate.Persistence visión general
Proyecto de persistencia con [`AppDbContext`](#dtemplatepersistence-appdbcontext) y configuración base de entidades, expone [`IDbContext`](#dtemplatepersistence-idbcontext) para desacoplar la infraestructura.

## DTemplate.Persistence componentes

### DTemplate.Persistence IDbContext
**Descripción:** Abstracción de `DbContext` con métodos equivalentes a EF Core.

**Índice de métodos**
- [`Add<TEntity>`](#dtemplate-persistence-idbcontext-add)
- [`AddAsync<TEntity>`](#dtemplate-persistence-idbcontext-addasync)
- [`AddRange(IEnumerable<object>)`](#dtemplate-persistence-idbcontext-addrange-enumerable)
- [`AddRange(params object[])`](#dtemplate-persistence-idbcontext-addrange-params)
- [`AddRangeAsync(IEnumerable<object>)`](#dtemplate-persistence-idbcontext-addrangeasync-enumerable)
- [`AddRangeAsync(params object[])`](#dtemplate-persistence-idbcontext-addrangeasync-params)
- [`Attach<TEntity>`](#dtemplate-persistence-idbcontext-attach)
- [`AttachRange(IEnumerable<object>)`](#dtemplate-persistence-idbcontext-attachrange-enumerable)
- [`AttachRange(params object[])`](#dtemplate-persistence-idbcontext-attachrange-params)
- [`Find<TEntity>`](#dtemplate-persistence-idbcontext-find)
- [`FindAsync<TEntity>(object[], CancellationToken)`](#dtemplate-persistence-idbcontext-findasync)
- [`FindAsync<TEntity>(params object[])`](#dtemplate-persistence-idbcontext-findasync-params)
- [`Remove<TEntity>`](#dtemplate-persistence-idbcontext-remove)
- [`RemoveRange(IEnumerable<object>)`](#dtemplate-persistence-idbcontext-removerange-enumerable)
- [`RemoveRange(params object[])`](#dtemplate-persistence-idbcontext-removerange-params)
- [`Update<TEntity>`](#dtemplate-persistence-idbcontext-update)
- [`UpdateRange(params object[])`](#dtemplate-persistence-idbcontext-updaterange-params)
- [`UpdateRange(IEnumerable<object>)`](#dtemplate-persistence-idbcontext-updaterange-enumerable)
- [`Entry(object)`](#dtemplate-persistence-idbcontext-entry)
- [`Entry<TEntity>`](#dtemplate-persistence-idbcontext-entry-generic)
- [`Set<TEntity>`](#dtemplate-persistence-idbcontext-set)
- [`SaveChanges()`](#dtemplate-persistence-idbcontext-savechanges)
- [`SaveChanges(bool)`](#dtemplate-persistence-idbcontext-savechanges-bool)
- [`SaveChangesAsync(CancellationToken)`](#dtemplate-persistence-idbcontext-savechangesasync)
- [`SaveChangesAsync(bool, CancellationToken)`](#dtemplate-persistence-idbcontext-savechangesasync-bool)
- [`Equals(object)`](#dtemplate-persistence-idbcontext-equals)
- [`GetHashCode()`](#dtemplate-persistence-idbcontext-gethashcode)
- [`ToString()`](#dtemplate-persistence-idbcontext-tostring)

**Métodos**
#### <a id="dtemplate-persistence-idbcontext-add"></a>`Add<TEntity>(TEntity entity)`
- **Descripción:** Agrega una entidad al contexto.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `entity` | `TEntity` | Entidad a agregar. |
- **Devuelve:** `EntityEntry<TEntity>` ([`EntityEntry<TEntity>`](https://learn.microsoft.com/dotnet/api/microsoft.entityframeworkcore.changetracking.entityentry-1)) con la entrada asociada.
- **Excepciones:** `ArgumentNullException` si `entity` es nulo.

#### <a id="dtemplate-persistence-idbcontext-addasync"></a>`AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)`
- **Descripción:** Agrega una entidad al contexto de forma asíncrona.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `entity` | `TEntity` | Entidad a agregar. |
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación. |
- **Devuelve:** `ValueTask<EntityEntry<TEntity>>` ([`EntityEntry<TEntity>`](https://learn.microsoft.com/dotnet/api/microsoft.entityframeworkcore.changetracking.entityentry-1)).
- **Excepciones:** `ArgumentNullException` si `entity` es nulo.

#### <a id="dtemplate-persistence-idbcontext-addrange-enumerable"></a>`AddRange(IEnumerable<object> entities)`
- **Descripción:** Agrega un conjunto de entidades al contexto.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `entities` | [`IEnumerable<object>`](https://learn.microsoft.com/dotnet/api/system.collections.generic.ienumerable-1) | Entidades a agregar. |
- **Devuelve:** No aplica.
- **Excepciones:** `ArgumentNullException` si `entities` es nulo.

#### <a id="dtemplate-persistence-idbcontext-addrange-params"></a>`AddRange(params object[] entities)`
- **Descripción:** Agrega un conjunto de entidades al contexto.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `entities` | `object[]` | Entidades a agregar. |
- **Devuelve:** No aplica.
- **Excepciones:** `ArgumentNullException` si `entities` es nulo.

#### <a id="dtemplate-persistence-idbcontext-addrangeasync-enumerable"></a>`AddRangeAsync(IEnumerable<object> entities, CancellationToken cancellationToken = default)`
- **Descripción:** Agrega un conjunto de entidades de forma asíncrona.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `entities` | [`IEnumerable<object>`](https://learn.microsoft.com/dotnet/api/system.collections.generic.ienumerable-1) | Entidades a agregar. |
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación. |
- **Devuelve:** `Task`.
- **Excepciones:** `ArgumentNullException` si `entities` es nulo.

#### <a id="dtemplate-persistence-idbcontext-addrangeasync-params"></a>`AddRangeAsync(params object[] entities)`
- **Descripción:** Agrega un conjunto de entidades de forma asíncrona.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `entities` | `object[]` | Entidades a agregar. |
- **Devuelve:** `Task`.
- **Excepciones:** `ArgumentNullException` si `entities` es nulo.

#### <a id="dtemplate-persistence-idbcontext-attach"></a>`Attach<TEntity>(TEntity entity)`
- **Descripción:** Adjunta una entidad al contexto.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `entity` | `TEntity` | Entidad a adjuntar. |
- **Devuelve:** `EntityEntry<TEntity>` ([`EntityEntry<TEntity>`](https://learn.microsoft.com/dotnet/api/microsoft.entityframeworkcore.changetracking.entityentry-1)).
- **Excepciones:** `ArgumentNullException` si `entity` es nulo.

#### <a id="dtemplate-persistence-idbcontext-attachrange-enumerable"></a>`AttachRange(IEnumerable<object> entities)`
- **Descripción:** Adjunta un conjunto de entidades al contexto.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `entities` | [`IEnumerable<object>`](https://learn.microsoft.com/dotnet/api/system.collections.generic.ienumerable-1) | Entidades a adjuntar. |
- **Devuelve:** No aplica.
- **Excepciones:** `ArgumentNullException` si `entities` es nulo.

#### <a id="dtemplate-persistence-idbcontext-attachrange-params"></a>`AttachRange(params object[] entities)`
- **Descripción:** Adjunta un conjunto de entidades al contexto.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `entities` | `object[]` | Entidades a adjuntar. |
- **Devuelve:** No aplica.
- **Excepciones:** `ArgumentNullException` si `entities` es nulo.

#### <a id="dtemplate-persistence-idbcontext-find"></a>`Find<TEntity>(params object[] keyValues)`
- **Descripción:** Busca una entidad por clave primaria.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `keyValues` | `object[]` | Valores de clave. |
- **Devuelve:** `TEntity?` con la entidad encontrada.
- **Excepciones:** `ArgumentNullException` si `keyValues` es nulo.

#### <a id="dtemplate-persistence-idbcontext-findasync"></a>`FindAsync<TEntity>(object[] keyValues, CancellationToken cancellationToken = default)`
- **Descripción:** Busca una entidad por clave primaria de forma asíncrona.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `keyValues` | `object[]` | Valores de clave. |
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación. |
- **Devuelve:** `ValueTask<TEntity?>`.
- **Excepciones:** `ArgumentNullException` si `keyValues` es nulo.

#### <a id="dtemplate-persistence-idbcontext-findasync-params"></a>`FindAsync<TEntity>(params object[] keyValues)`
- **Descripción:** Busca una entidad por clave primaria de forma asíncrona.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `keyValues` | `object[]` | Valores de clave. |
- **Devuelve:** `ValueTask<TEntity?>`.
- **Excepciones:** `ArgumentNullException` si `keyValues` es nulo.

#### <a id="dtemplate-persistence-idbcontext-remove"></a>`Remove<TEntity>(TEntity entity)`
- **Descripción:** Marca una entidad para eliminación.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `entity` | `TEntity` | Entidad a eliminar. |
- **Devuelve:** `EntityEntry<TEntity>` ([`EntityEntry<TEntity>`](https://learn.microsoft.com/dotnet/api/microsoft.entityframeworkcore.changetracking.entityentry-1)).
- **Excepciones:** `ArgumentNullException` si `entity` es nulo.

#### <a id="dtemplate-persistence-idbcontext-removerange-enumerable"></a>`RemoveRange(IEnumerable<object> entities)`
- **Descripción:** Marca un conjunto de entidades para eliminación.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `entities` | [`IEnumerable<object>`](https://learn.microsoft.com/dotnet/api/system.collections.generic.ienumerable-1) | Entidades a eliminar. |
- **Devuelve:** No aplica.
- **Excepciones:** `ArgumentNullException` si `entities` es nulo.

#### <a id="dtemplate-persistence-idbcontext-removerange-params"></a>`RemoveRange(params object[] entities)`
- **Descripción:** Marca un conjunto de entidades para eliminación.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `entities` | `object[]` | Entidades a eliminar. |
- **Devuelve:** No aplica.
- **Excepciones:** `ArgumentNullException` si `entities` es nulo.

#### <a id="dtemplate-persistence-idbcontext-update"></a>`Update<TEntity>(TEntity entity)`
- **Descripción:** Marca una entidad como modificada.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `entity` | `TEntity` | Entidad a actualizar. |
- **Devuelve:** `EntityEntry<TEntity>` ([`EntityEntry<TEntity>`](https://learn.microsoft.com/dotnet/api/microsoft.entityframeworkcore.changetracking.entityentry-1)).
- **Excepciones:** `ArgumentNullException` si `entity` es nulo.

#### <a id="dtemplate-persistence-idbcontext-updaterange-params"></a>`UpdateRange(params object[] entities)`
- **Descripción:** Marca un conjunto de entidades como modificadas.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `entities` | `object[]` | Entidades a actualizar. |
- **Devuelve:** No aplica.
- **Excepciones:** `ArgumentNullException` si `entities` es nulo.

#### <a id="dtemplate-persistence-idbcontext-updaterange-enumerable"></a>`UpdateRange(IEnumerable<object> entities)`
- **Descripción:** Marca un conjunto de entidades como modificadas.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `entities` | [`IEnumerable<object>`](https://learn.microsoft.com/dotnet/api/system.collections.generic.ienumerable-1) | Entidades a actualizar. |
- **Devuelve:** No aplica.
- **Excepciones:** `ArgumentNullException` si `entities` es nulo.

#### <a id="dtemplate-persistence-idbcontext-entry"></a>`Entry(object entity)`
- **Descripción:** Obtiene la entrada de la entidad.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `entity` | `object` | Entidad de referencia. |
- **Devuelve:** `EntityEntry` ([`EntityEntry`](https://learn.microsoft.com/dotnet/api/microsoft.entityframeworkcore.changetracking.entityentry)).
- **Excepciones:** `ArgumentNullException` si `entity` es nulo.

#### <a id="dtemplate-persistence-idbcontext-entry-generic"></a>`Entry<TEntity>(TEntity entity) where TEntity : class`
- **Descripción:** Obtiene la entrada de la entidad tipada.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `entity` | `TEntity` | Entidad de referencia. |
- **Devuelve:** `EntityEntry<TEntity>` ([`EntityEntry<TEntity>`](https://learn.microsoft.com/dotnet/api/microsoft.entityframeworkcore.changetracking.entityentry-1)).
- **Excepciones:** `ArgumentNullException` si `entity` es nulo.

#### <a id="dtemplate-persistence-idbcontext-set"></a>`Set<TEntity>() where TEntity : class`
- **Descripción:** Obtiene el conjunto de entidades para el tipo indicado.
- **Parámetros:** No aplica.
- **Devuelve:** `DbSet<TEntity>` ([`DbSet<TEntity>`](https://learn.microsoft.com/dotnet/api/microsoft.entityframeworkcore.dbset-1)).
- **Excepciones:** No aplica.

#### <a id="dtemplate-persistence-idbcontext-savechanges"></a>`SaveChanges()`
- **Descripción:** Guarda los cambios en la base de datos.
- **Parámetros:** No aplica.
- **Devuelve:** `int` con el número de entidades afectadas.
- **Excepciones:** Puede lanzar `DbUpdateException`.

#### <a id="dtemplate-persistence-idbcontext-savechanges-bool"></a>`SaveChanges(bool acceptAllChangesOnSuccess)`
- **Descripción:** Guarda cambios con opción de aceptar cambios.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `acceptAllChangesOnSuccess` | `bool` | Indica si acepta cambios. |
- **Devuelve:** `int`.
- **Excepciones:** Puede lanzar `DbUpdateException`.

#### <a id="dtemplate-persistence-idbcontext-savechangesasync"></a>`SaveChangesAsync(CancellationToken cancellationToken = default)`
- **Descripción:** Guarda cambios de forma asíncrona.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación. |
- **Devuelve:** `Task<int>`.
- **Excepciones:** Puede lanzar `DbUpdateException`.

#### <a id="dtemplate-persistence-idbcontext-savechangesasync-bool"></a>`SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)`
- **Descripción:** Guarda cambios de forma asíncrona con opción de aceptar cambios.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `acceptAllChangesOnSuccess` | `bool` | Indica si acepta cambios. |
  | `cancellationToken` | [`CancellationToken`](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) | Token de cancelación. |
- **Devuelve:** `Task<int>`.
- **Excepciones:** Puede lanzar `DbUpdateException`.

#### <a id="dtemplate-persistence-idbcontext-equals"></a>`Equals(object obj)`
- **Descripción:** Compara el contexto con otro objeto.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
  | `obj` | `object` | Objeto a comparar. |
- **Devuelve:** `bool`.
- **Excepciones:** No aplica.

#### <a id="dtemplate-persistence-idbcontext-gethashcode"></a>`GetHashCode()`
- **Descripción:** Obtiene el hash del contexto.
- **Parámetros:** No aplica.
- **Devuelve:** `int`.
- **Excepciones:** No aplica.

#### <a id="dtemplate-persistence-idbcontext-tostring"></a>`ToString()`
- **Descripción:** Devuelve una representación del contexto.
- **Parámetros:** No aplica.
- **Devuelve:** `string?`.
- **Excepciones:** No aplica.

---

### DTemplate.Persistence AppDbContext
**Descripción:** DbContext concreto que aplica configuraciones y convertidores [`CId`](#dtemplatedomain-cid).

**Índice de métodos**
- [`AppDbContext`](#dtemplate-persistence-appdbcontext-ctor)
- [`OnModelCreating`](#dtemplate-persistence-appdbcontext-onmodelcreating)

**Métodos**
#### <a id="dtemplate-persistence-appdbcontext-ctor"></a>`AppDbContext(DbContextOptions<AppDbContext> options)`
- **Descripción:** Inicializa el contexto con opciones de configuración.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
| `options` | [`DbContextOptions<AppDbContext>`](https://learn.microsoft.com/dotnet/api/microsoft.entityframeworkcore.dbcontextoptions-1) | Opciones del contexto. |
- **Devuelve:** No aplica.
- **Excepciones:** `ArgumentNullException` si `options` es nulo.

#### <a id="dtemplate-persistence-appdbcontext-onmodelcreating"></a>`OnModelCreating(ModelBuilder builder)`
- **Descripción:** Configura el modelo y aplica configuraciones del ensamblado.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
| `builder` | [`ModelBuilder`](https://learn.microsoft.com/dotnet/api/microsoft.entityframeworkcore.modelbuilder) | Constructor de modelo. |
- **Devuelve:** No aplica.
- **Excepciones:** Puede propagar excepciones de configuración.

---

### DTemplate.Persistence BaseEntityConfiguration
**Descripción:** Configuración base para entidades con [`CId`](#dtemplatedomain-cid).

**Índice de métodos**
- [`Configure`](#dtemplate-persistence-baseentityconfiguration-configure)

**Métodos**
#### <a id="dtemplate-persistence-baseentityconfiguration-configure"></a>`Configure(EntityTypeBuilder<TEntity> builder)`
- **Descripción:** Configura claves, generadores y convertidores para entidades con `CId`.
- **Parámetros**
  | Nombre | Tipo | Descripción |
  | --- | --- | --- |
| `builder` | [`EntityTypeBuilder<TEntity>`](https://learn.microsoft.com/dotnet/api/microsoft.entityframeworkcore.metadata.builders.entitytypebuilder-1) | Constructor de entidad. |
- **Devuelve:** No aplica.
- **Excepciones:** No aplica.

---

# DTemplate.Tests

## DTemplate.Tests visión general
Proyecto de pruebas sin componentes de negocio definidos en el código fuente actual. Está configurado para usar `Microsoft.NET.Test.Sdk` y puede ampliarse para pruebas unitarias o de integración.
