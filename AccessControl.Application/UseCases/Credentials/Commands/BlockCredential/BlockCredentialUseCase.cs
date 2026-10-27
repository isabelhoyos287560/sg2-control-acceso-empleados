using AccessControl.Application.Contracts.Persistence;
using AccessControl.Application.Contracts.Repositories;
using AccessControl.Application.Exceptions;
using AccessControl.Application.UseCases.Common;
using AccessControl.Application.Utilities.Mediator;
using AccessControl.Application.Utilities.Results;
using AccessControl.Domain.Entities.Employees;
using AccessControl.Domain.Entities.Employees.ValueObjects;

namespace AccessControl.Application.UseCases.Credentials.Commands.BlockCredential
{
    public sealed class BlockCredentialUseCase : BaseHandler, IRequestHandler<BlockCredentialCommand, Result<Guid>>
    {
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BlockCredentialUseCase(IEmployeesRepository employeesRepository, IUnitOfWork unitOfWork)
        {
            _employeesRepository = employeesRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(BlockCredentialCommand command)
        {
            return await ExecuteAsync(async () =>
            {
                CredentialCode code = CredentialCode.Create(command.Code);

                Employee? employee = await _employeesRepository.GetByCredentialCodeAsync(code);

                if (employee is null)
                {
                    throw new NotFoundException($"No existe una credencial con el código {code}.");
                }

                // La regla (no bloquear una credencial revocada) vive en el dominio
                employee.BlockCredential(code);

                await _unitOfWork.CommitAsync();

                return employee.FindCredential(code)!.Id;
            });
        }
    }
}
