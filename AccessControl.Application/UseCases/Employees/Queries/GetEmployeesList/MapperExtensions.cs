using AccessControl.Domain.Entities.Employees;

namespace AccessControl.Application.UseCases.Employees.Queries.GetEmployeesList
{
    internal static class MapperExtensions
    {
        public static EmployeeListItemDTO ToListItemDTO(this Employee employee)
        {
            return new EmployeeListItemDTO
            {
                Id = employee.Id,
                IdentificationNumber = employee.IdentificationNumber,
                FullName = employee.FullName,
                Email = employee.Email.Value,
                Position = employee.Position,
                AreaCode = employee.Area.Code,
                AreaName = employee.Area.Name,
                Status = employee.Status.ToString(),
            };
        }
    }
}
