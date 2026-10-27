using AccessControl.Api.DTOs.Credentials;
using AccessControl.Application.UseCases.Credentials.Commands.AssignCredential;
using AccessControl.Application.UseCases.Credentials.Commands.BlockCredential;
using AccessControl.Application.UseCases.Credentials.Commands.RevokeCredential;
using AccessControl.Application.UseCases.Credentials.Commands.UnblockCredential;
using AccessControl.Application.UseCases.Credentials.Queries.GetCredentialByCode;
using AccessControl.Application.Utilities.Mediator;
using AccessControl.Application.Utilities.Results;
using Microsoft.AspNetCore.Mvc;

namespace AccessControl.Api.Controllers
{
    [Route("api/[controller]")]
    public class CredentialsController : ApiController
    {
        private readonly IMediator _mediator;

        public CredentialsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> GetByCode([FromRoute] string code)
        {
            GetCredentialByCodeQuery query = new()
            {
                Code = code
            };

            Result<CredentialDetailsDTO> result = await _mediator.Send(query);

            return ToActionResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Assign([FromBody] AssignCredentialDTO request)
        {
            AssignCredentialCommand command = new()
            {
                EmployeeId = request.EmployeeId,
                Code = request.Code
            };

            Result<Guid> result = await _mediator.Send(command);

            return ToActionResult(result, StatusCodes.Status201Created);
        }

        [HttpPut("{code}/block")]
        public async Task<IActionResult> Block([FromRoute] string code)
        {
            Result<Guid> result = await _mediator.Send(new BlockCredentialCommand { Code = code });

            return ToActionResult(result);
        }

        [HttpPut("{code}/unblock")]
        public async Task<IActionResult> Unblock([FromRoute] string code)
        {
            Result<Guid> result = await _mediator.Send(new UnblockCredentialCommand { Code = code });

            return ToActionResult(result);
        }

        [HttpPut("{code}/revoke")]
        public async Task<IActionResult> Revoke([FromRoute] string code)
        {
            Result<Guid> result = await _mediator.Send(new RevokeCredentialCommand { Code = code });

            return ToActionResult(result);
        }
    }
}
