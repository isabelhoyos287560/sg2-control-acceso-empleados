using AccessControl.Application.UseCases.Areas.Queries.GetAreasList;
using AccessControl.Application.Utilities.Mediator;
using AccessControl.Application.Utilities.Results;
using Microsoft.AspNetCore.Mvc;

namespace AccessControl.Api.Controllers
{
    [Route("api/[controller]")]
    public class AreasController : ApiController
    {
        private readonly IMediator _mediator;

        public AreasController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            Result<List<AreaDTO>> result = await _mediator.Send(new GetAreasListQuery());

            return ToActionResult(result);
        }
    }
}
