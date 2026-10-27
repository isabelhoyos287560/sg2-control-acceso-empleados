using AccessControl.Api.DTOs.Common;
using System.ComponentModel.DataAnnotations;

namespace AccessControl.Api.DTOs.Employees
{
    public class UpdateEmployeeDTO
    {
        [NotEmptyGuid]
        public Guid Id { get; set; }

        [Required]
        [StringLength(64, MinimumLength = 2)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(64, MinimumLength = 2)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(128)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(64, MinimumLength = 2)]
        public string Position { get; set; } = string.Empty;

        [Required]
        public string AreaCode { get; set; } = string.Empty;
    }
}
