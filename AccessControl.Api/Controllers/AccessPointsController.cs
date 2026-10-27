using AccessControl.Api.DTOs.AccessPoints;
using AccessControl.Application.UseCases.AccessPoints.Commands.CreateAccessPoint;
using AccessControl.Application.UseCases.AccessPoints.Commands.UpdateAccessPoint;
using AccessControl.Application.UseCases.AccessPoints.Queries.GetAccessPointById;
using AccessControl.Application.UseCases.AccessPoints.Queries.GetAccessPointsList;
using AccessControl.Application.Utilities.Mediator;
using AccessControl.Application.Utilities.Pagination;
using AccessControl.Application.Utilities.Results;
using AccessControl.Domain.Entities.AccessPoints.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace AccessControl.Api.Controllers
{
    [Route("api/[controller]")]
    public class AccessPointsController : ApiController
    {
        private readonly IMediator _mediator;

        public AccessPointsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] int pageNumber = 1,
                                                 [FromQuery] int pageSize = PaginationRequest.DEFAULT_PAGE_SIZE,
                                                 [FromQuery] bool? isEnabled = null,
                                                 [FromQuery] AccessPointType? type = null)
        {
            GetAccessPointsListQuery query = new()
            {
                Pagination = new PaginationRequest(pageNumber, pageSize),
                IsEnabled = isEnabled,
                Type = type
            };

            Result<PaginationResponse<AccessPointDTO>> result = await _mediator.Send(query);

            return ToActionResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            Result<AccessPointDTO> result = await _mediator.Send(new GetAccessPointByIdQuery { Id = id });

            return ToActionResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAccessPointDTO request)
        {
            CreateAccessPointCommand command = new()
            {
                Name = request.Name,
                Description = request.Description,
                Type = request.Type,
                IsEnabled = request.IsEnabled
            };

            Result<Guid> result = await _mediator.Send(command);

            return ToActionResult(result, StatusCodes.Status201Created);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateAccessPointDTO request)
        {
            UpdateAccessPointCommand command = new()
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
                Type = request.Type,
                IsEnabled = request.IsEnabled
            };

            Result<Guid> result = await _mediator.Send(command);

            return ToActionResult(result);
        }
    }
}
