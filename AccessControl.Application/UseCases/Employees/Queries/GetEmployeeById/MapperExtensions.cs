using AccessControl.Domain.Entities.Employees;

namespace AccessControl.Application.UseCases.Employees.Queries.GetEmployeeById
{
    internal static class MapperExtensions
    {
        public static EmployeeDetailsDTO ToDetailsDTO(this Employee employee)
        {
            return new EmployeeDetailsDTO
            {
                Id = employee.Id,
                IdentificationNumber = employee.IdentificationNumber,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email.Value,
                Position = employee.Position,
                AreaCode = employee.Area.Code,
                AreaName = employee.Area.Name,
                Status = employee.Status.ToString(),
                CreatedAt = employee.CreatedAt,
                DeactivatedAt = employee.DeactivatedAt,
                Credentials = employee.Credentials.OrderByDescending(c => c.IssuedAt)
                                                  .Select(c => new EmployeeCredentialDTO
                                                  {
                                                      Id = c.Id,
                                                      Code = c.Code.Value,
                                                      Status = c.Status.ToString(),
                                                      IssuedAt = c.IssuedAt,
                                                      StatusChangedAt = c.StatusChangedAt,
                                                  })
                                                  .ToList(),
            };
        }
    }
}
