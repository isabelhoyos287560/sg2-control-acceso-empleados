using AccessControl.Api.DTOs.AccessRecords;
using AccessControl.Application.UseCases.AccessRecords.Commands.RegisterAccess;
using AccessControl.Application.UseCases.AccessRecords.Queries.GetAccessHistory;
using AccessControl.Application.Utilities.Mediator;
using AccessControl.Application.Utilities.Pagination;
using AccessControl.Application.Utilities.Results;
using AccessControl.Domain.Entities.AccessRecords.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace AccessControl.Api.Controllers
{
    [Route("api/[controller]")]
    public class AccessRecordsController : ApiController
    {
        private readonly IMediator _mediator;

        public AccessRecordsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// RF-03: registra un intento de entrada o salida. Responde 201 tanto si es autorizado como si es rechazado:
        /// el movimiento queda registrado y el resultado viene en el cuerpo (isAuthorized / rejectionReason).
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterAccessDTO request)
        {
            RegisterAccessCommand command = new()
            {
                CredentialCode = request.CredentialCode,
                AccessPointId = request.AccessPointId,
                MovementType = request.MovementType
            };

            Result<AccessRecordResultDTO> result = await _mediator.Send(command);

            return ToActionResult(result, StatusCodes.Status201Created);
        }

        /// <summary>
        /// RF-04: historial paginado de accesos con filtros por empleado, rango de fechas (UTC), punto de acceso y resultado.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetHistory([FromQuery] int pageNumber = 1,
                                                    [FromQuery] int pageSize = PaginationRequest.DEFAULT_PAGE_SIZE,
                                                    [FromQuery] Guid? employeeId = null,
                                                    [FromQuery] DateTime? from = null,
                                                    [FromQuery] DateTime? to = null,
                                                    [FromQuery] Guid? accessPointId = null,
                                                    [FromQuery] AccessResult? result = null)
        {
            GetAccessHistoryQuery query = new()
            {
                Pagination = new PaginationRequest(pageNumber, pageSize),
                EmployeeId = employeeId,
                From = from,
                To = to,
                AccessPointId = accessPointId,
                Result = result
            };

            Result<PaginationResponse<AccessRecordListItemDTO>> response = await _mediator.Send(query);

            return ToActionResult(response);
        }
    }
}
