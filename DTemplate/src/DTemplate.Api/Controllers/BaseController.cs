using Microsoft.AspNetCore.Mvc;
using Pelican.Mediator;

namespace DTemplate.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    /// <summary>
    /// Provides a base API controller with access to the mediator.
    /// </summary>
    public class BaseController : ControllerBase
    {
        private IMediator _mediator;

        /// <summary>
        /// Gets the mediator resolved from the current request services.
        /// </summary>
        public IMediator Mediator => 
            _mediator ??= HttpContext.RequestServices.GetService<IMediator>() 
            ?? throw new InvalidOperationException("Mediator service is not available.");
    }
}
