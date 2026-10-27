using AccessControl.Api.DTOs.Common;
using AccessControl.Domain.Entities.AccessRecords.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace AccessControl.Api.DTOs.AccessRecords
{
    public class RegisterAccessDTO
    {
        [Required]
        [StringLength(32, MinimumLength = 6)]
        public string CredentialCode { get; set; } = string.Empty;

        [NotEmptyGuid]
        public Guid AccessPointId { get; set; }

        /// <summary>1 = Entry (entrada), 2 = Exit (salida)</summary>
        [EnumDataType(typeof(MovementType))]
        public MovementType MovementType { get; set; }
    }
}
