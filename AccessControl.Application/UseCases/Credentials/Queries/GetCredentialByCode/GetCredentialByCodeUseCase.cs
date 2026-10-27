using AccessControl.Application.Contracts.Repositories;
using AccessControl.Application.Exceptions;
using AccessControl.Application.UseCases.Common;
using AccessControl.Application.Utilities.Mediator;
using AccessControl.Application.Utilities.Results;
using AccessControl.Domain.Entities.Employees;
using AccessControl.Domain.Entities.Employees.ValueObjects;

namespace AccessControl.Application.UseCases.Credentials.Queries.GetCredentialByCode
{
    public sealed class GetCredentialByCodeUseCase : BaseHandler, IRequestHandler<GetCredentialByCodeQuery, Result<CredentialDetailsDTO>>
    {
        private readonly IEmployeesRepository _employeesRepository;

        public GetCredentialByCodeUseCase(IEmployeesRepository employeesRepository)
        {
            _employeesRepository = employeesRepository;
        }

        public async Task<Result<CredentialDetailsDTO>> Handle(GetCredentialByCodeQuery query)
        {
            return await ExecuteAsync(async () =>
            {
                CredentialCode code = CredentialCode.Create(query.Code);

                Employee? employee = await _employeesRepository.GetByCredentialCodeAsync(code);
                Credential? credential = employee?.FindCredential(code);

                if (employee is null || credential is null)
                {
                    throw new NotFoundException($"No existe una credencial con el código {code}.");
                }

                return credential.ToDetailsDTO(employee);
            });
        }
    }
}
