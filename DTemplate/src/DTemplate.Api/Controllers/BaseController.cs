using Microsoft.AspNetCore.Mvc;
using Pelican.Mediator;

namespace DTemplate.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private IMediator _mediator;

        public IMediator Mediator => 
            _mediator ??= HttpContext.RequestServices.GetService<IMediator>() 
            ?? throw new InvalidOperationException("Mediator service is not available.");
    }
}
