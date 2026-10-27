using AccessControl.Domain.Entities.AccessPoints.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace AccessControl.Api.DTOs.AccessPoints
{
    public class CreateAccessPointDTO
    {
        [Required]
        [StringLength(64, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;

        [StringLength(256)]
        public string? Description { get; set; }

        /// <summary>1 = Reception, 2 = Parking, 3 = Warehouse, 4 = RestrictedArea, 99 = Other</summary>
        [EnumDataType(typeof(AccessPointType))]
        public AccessPointType Type { get; set; }

        public bool IsEnabled { get; set; } = true;
    }
}
