using AccessControl.Api.DTOs.Employees;
using AccessControl.Application.UseCases.Employees.Commands.CreateEmployee;
using AccessControl.Application.UseCases.Employees.Commands.DeactivateEmployee;
using AccessControl.Application.UseCases.Employees.Commands.UpdateEmployee;
using AccessControl.Application.UseCases.Employees.Queries.GetEmployeeById;
using AccessControl.Application.UseCases.Employees.Queries.GetEmployeesList;
using AccessControl.Application.Utilities.Mediator;
using AccessControl.Application.Utilities.Pagination;
using AccessControl.Application.Utilities.Results;
using AccessControl.Domain.Entities.Employees.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace AccessControl.Api.Controllers
{
    [Route("api/[controller]")]
    public class EmployeesController : ApiController
    {
        private readonly IMediator _mediator;

        public EmployeesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] int pageNumber = 1,
                                                 [FromQuery] int pageSize = PaginationRequest.DEFAULT_PAGE_SIZE,
                                                 [FromQuery] EmploymentStatus? status = null,
                                                 [FromQuery] string? areaCode = null)
        {
            GetEmployeesListQuery query = new()
            {
                Pagination = new PaginationRequest(pageNumber, pageSize),
                Status = status,
                AreaCode = areaCode
            };

            Result<PaginationResponse<EmployeeListItemDTO>> result = await _mediator.Send(query);

            return ToActionResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            GetEmployeeByIdQuery query = new()
            {
                Id = id
            };

            Result<EmployeeDetailsDTO> result = await _mediator.Send(query);

            return ToActionResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeDTO request)
        {
            CreateEmployeeCommand command = new()
            {
                IdentificationNumber = request.IdentificationNumber,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Position = request.Position,
                AreaCode = request.AreaCode
            };

            Result<Guid> result = await _mediator.Send(command);

            return ToActionResult(result, StatusCodes.Status201Created);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateEmployeeDTO request)
        {
            UpdateEmployeeCommand command = new()
            {
                Id = request.Id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Position = request.Position,
                AreaCode = request.AreaCode
            };

            Result<Guid> result = await _mediator.Send(command);

            return ToActionResult(result);
        }

        [HttpPut("{id}/deactivate")]
        public async Task<IActionResult> Deactivate([FromRoute] Guid id)
        {
            DeactivateEmployeeCommand command = new()
            {
                Id = id
            };

            Result<Guid> result = await _mediator.Send(command);

            return ToActionResult(result);
        }
    }
}
