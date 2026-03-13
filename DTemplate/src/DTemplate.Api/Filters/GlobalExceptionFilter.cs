using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.Net;
using DTemplate.Business.Core.Exceptions;

namespace DTemplate.Api.Filters
{
    /// <summary>
    /// Handles unhandled exceptions and produces a standardized error response.
    /// </summary>
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;
        private readonly ApiBehaviorOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalExceptionFilter"/> class.
        /// </summary>
        /// <param name="logger">The logger to use for error reporting.</param>
        /// <param name="options">The API behavior options.</param>
        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger, IOptions<ApiBehaviorOptions> options)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options?.Value;
        }

        /// <summary>
        /// Handles an exception and sets the appropriate response.
        /// </summary>
        /// <param name="context">The exception context.</param>
        public void OnException(ExceptionContext context)
        {
            var errors = Array.Empty<string>();
            var code = HttpStatusCode.InternalServerError;

            switch (context.Exception)
            {
                case ValidationException vEx:
                    errors = vEx.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}").ToArray();
                    code = HttpStatusCode.BadRequest;
                    break;

                case HttpException httpEx:
                    errors = [ httpEx.Message ];
                    code = httpEx.StatusCode;
                    break;

                default:
                    errors = [ context.Exception.Message ];
                    break;
            }

            _logger.LogError(context.Exception, context.Exception.Message);

            var result = new ProblemDetails();

            // Use 'Problem Details' feature added in 2.1 and 2.2 so it follows the standardized error messages in API controllers. It's based on the RFC 7807 specification.
            // ASP.NET Core 2.1 Problem Details support: https://devblogs.microsoft.com/aspnet/asp-net-core-2-1-web-apis/#problem-details
            // ASP.NET Core 2.2 release notes on Problem Details: https://blogs.msdn.microsoft.com/webdev/2018/09/12/asp-net-core-2-2-0-preview2-now-available/
            if (_options.ClientErrorMapping.TryGetValue((int)code, out var errorData))
            {
                result.Type = errorData.Link;
                result.Title = errorData.Title;
            }
            else
            {
                var reason = ReasonPhrases.GetReasonPhrase((int)code);
                if (!string.IsNullOrWhiteSpace(reason))
                    result.Title = reason;
            }

            result.Title ??= "An unexpected error occurred.";
            result.Detail = string.Join(" | ", errors);
            result.Status = (int)code;
            result.Instance = context.HttpContext.TraceIdentifier;

            context.HttpContext.Response.StatusCode = (int)code;

            context.Result = new ObjectResult(result)
            {
                StatusCode = (int)code,
                DeclaredType = typeof(ProblemDetails)
            };
        }
    }
}
