using System.ComponentModel.DataAnnotations;

namespace AccessControl.Api.DTOs.Employees
{
    public class CreateEmployeeDTO
    {
        [Required]
        [StringLength(20, MinimumLength = 5)]
        public string IdentificationNumber { get; set; } = string.Empty;

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

        /// <summary>Código del área organizacional (ver GET api/areas). Ej: WAREHOUSE</summary>
        [Required]
        public string AreaCode { get; set; } = string.Empty;
    }
}
