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