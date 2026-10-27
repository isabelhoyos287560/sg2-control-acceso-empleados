using AccessControl.Application.UseCases.AccessPoints.Commands.CreateAccessPoint;
using AccessControl.Application.UseCases.AccessPoints.Commands.UpdateAccessPoint;
using AccessControl.Application.UseCases.AccessPoints.Queries.GetAccessPointById;
using AccessControl.Application.UseCases.AccessPoints.Queries.GetAccessPointsList;
using AccessControl.Application.UseCases.AccessRecords.Commands.RegisterAccess;
using AccessControl.Application.UseCases.AccessRecords.Queries.GetAccessHistory;
using AccessControl.Application.UseCases.Areas.Queries.GetAreasList;
using AccessControl.Application.UseCases.Credentials.Commands.AssignCredential;
using AccessControl.Application.UseCases.Credentials.Commands.BlockCredential;
using AccessControl.Application.UseCases.Credentials.Commands.RevokeCredential;
using AccessControl.Application.UseCases.Credentials.Commands.UnblockCredential;
using AccessControl.Application.UseCases.Credentials.Queries.GetCredentialByCode;
using AccessControl.Application.UseCases.Employees.Commands.CreateEmployee;
using AccessControl.Application.UseCases.Employees.Commands.DeactivateEmployee;
using AccessControl.Application.UseCases.Employees.Commands.UpdateEmployee;
using AccessControl.Application.UseCases.Employees.Queries.GetEmployeeById;
using AccessControl.Application.UseCases.Employees.Queries.GetEmployeesList;
using AccessControl.Application.Utilities.Mediator;
using AccessControl.Application.Utilities.Pagination;
using AccessControl.Application.Utilities.Results;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace AccessControl.Application
{
    public static class ApplicationServicesRegistry
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Mediator
            services.AddScoped<IMediator, SimpleMediator>();

            // Use Cases - Employees (RF-01)
            services.AddScoped<IRequestHandler<CreateEmployeeCommand, Result<Guid>>, CreateEmployeeUseCase>();
            services.AddScoped<IRequestHandler<UpdateEmployeeCommand, Result<Guid>>, UpdateEmployeeUseCase>();
            services.AddScoped<IRequestHandler<DeactivateEmployeeCommand, Result<Guid>>, DeactivateEmployeeUseCase>();
            services.AddScoped<IRequestHandler<GetEmployeesListQuery, Result<PaginationResponse<EmployeeListItemDTO>>>, GetEmployeesListUseCase>();
            services.AddScoped<IRequestHandler<GetEmployeeByIdQuery, Result<EmployeeDetailsDTO>>, GetEmployeeByIdUseCase>();
            services.AddScoped<IRequestHandler<GetAreasListQuery, Result<List<AreaDTO>>>, GetAreasListUseCase>();

            // Use Cases - Credentials (RF-02)
            services.AddScoped<IRequestHandler<AssignCredentialCommand, Result<Guid>>, AssignCredentialUseCase>();
            services.AddScoped<IRequestHandler<BlockCredentialCommand, Result<Guid>>, BlockCredentialUseCase>();
            services.AddScoped<IRequestHandler<UnblockCredentialCommand, Result<Guid>>, UnblockCredentialUseCase>();
            services.AddScoped<IRequestHandler<RevokeCredentialCommand, Result<Guid>>, RevokeCredentialUseCase>();
            services.AddScoped<IRequestHandler<GetCredentialByCodeQuery, Result<CredentialDetailsDTO>>, GetCredentialByCodeUseCase>();

            // Use Cases - Access records (RF-03, RF-04)
            services.AddScoped<IRequestHandler<RegisterAccessCommand, Result<AccessRecordResultDTO>>, RegisterAccessUseCase>();
            services.AddScoped<IRequestHandler<GetAccessHistoryQuery, Result<PaginationResponse<AccessRecordListItemDTO>>>, GetAccessHistoryUseCase>();

            // Use Cases - Access points (RF-06)
            services.AddScoped<IRequestHandler<CreateAccessPointCommand, Result<Guid>>, CreateAccessPointUseCase>();
            services.AddScoped<IRequestHandler<UpdateAccessPointCommand, Result<Guid>>, UpdateAccessPointUseCase>();
            services.AddScoped<IRequestHandler<GetAccessPointsListQuery, Result<PaginationResponse<AccessPointDTO>>>, GetAccessPointsListUseCase>();
            services.AddScoped<IRequestHandler<GetAccessPointByIdQuery, Result<AccessPointDTO>>, GetAccessPointByIdUseCase>();

            // Validations
            services.AddValidatorsFromAssemblyContaining<CreateEmployeeCommandValidator>();

            return services;
        }
    }
}
