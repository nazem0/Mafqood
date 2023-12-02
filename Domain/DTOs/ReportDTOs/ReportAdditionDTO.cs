using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.ReportDTOs
{
    public class ReportAdditionDTO
    {
        [MaxLength(255)]
        public string? Name { get; set; }
        [Required]
        public bool Gender { get; set; }
        [Required]
        public int SkinTone { get; set; }
        [Required]
        public int HairType { get; set; }
        [Required]
        public int HairColor { get; set; }
        [Required]
        public int EyeColor { get; set; }
        [Required]
        public int HeightLevel { get; set; }
        [Required]
        public int WeightLevel { get; set; }
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
        public string? Street { get; set; }
        public string? District { get; set; }
        [Required]
        public int CityId { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        [Required, RegularExpression("^[\\+]?[(]?[0-9]{3}[)]?[-\\s\\.]?[0-9]{3}[-\\s\\.]?[0-9]{4,6}")]
        public required string ContactNumber { get; set; }
        [MaxLength(1500)]
        public string? AdditionalInfo { get; set; }
        [MaxLength(32)]
        public required string DeletionCode { get; set; }
        public required IFormFileCollection ReportAttachments { get; set; }
    }
}
