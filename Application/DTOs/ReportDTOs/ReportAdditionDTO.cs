using Domain.Enums;
using Domain.Validators;
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
        public SkinTone SkinTone { get; set; }
        [Required]
        public HairType HairType { get; set; }
        [Required]
        public bool FacialHair { get; set; }
        [Required]
        public HairColor HairColor { get; set; }
        [Required]
        public EyeColor EyeColor { get; set; }
        [Required]
        public HeightLevel HeightLevel { get; set; }
        [Required]
        public WeightLevel WeightLevel { get; set; }
        public byte? MinAge { get; set; }
        public byte? MaxAge { get; set; }
        public string? Street { get; set; }
        public string? District { get; set; }
        [Required]
        public int CityId { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        [Required, DataType(DataType.PhoneNumber), RegularExpression("^[\\+]?[(]?[0-9]{3}[)]?[-\\s\\.]?[0-9]{3}[-\\s\\.]?[0-9]{4,6}")]
        public required string ContactNumber { get; set; }
        [MaxLength(1500)]
        public string? AdditionalInfo { get; set; }
        [Required, DataType(DataType.Password), MaxLength(32)]
        public required string DeletionCode { get; set; }
        [Required, DataType(DataType.Upload), OnlyImageFormFileType, MaxFormFileCollectionCount(3), MaxFormFileCollectionSize(10)]
        public required IFormFileCollection ReportAttachments { get; set; }
    }
}
