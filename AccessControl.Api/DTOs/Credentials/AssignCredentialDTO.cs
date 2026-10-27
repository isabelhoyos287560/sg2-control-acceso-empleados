using AccessControl.Api.DTOs.Common;
using System.ComponentModel.DataAnnotations;

namespace AccessControl.Api.DTOs.Credentials
{
    public class AssignCredentialDTO
    {
        [NotEmptyGuid]
        public Guid EmployeeId { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 6)]
        public string Code { get; set; } = string.Empty;
    }
}
