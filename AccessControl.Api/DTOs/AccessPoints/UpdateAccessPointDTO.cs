using AccessControl.Api.DTOs.Common;
using AccessControl.Domain.Entities.AccessPoints.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace AccessControl.Api.DTOs.AccessPoints
{
    public class UpdateAccessPointDTO
    {
        [NotEmptyGuid]
        public Guid Id { get; set; }

        [Required]
        [StringLength(64, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;

        [StringLength(256)]
        public string? Description { get; set; }

        [EnumDataType(typeof(AccessPointType))]
        public AccessPointType Type { get; set; }

        [Required]
        public bool IsEnabled { get; set; }
    }
}
