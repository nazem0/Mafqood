using Application.DTOs.AttachmentDTOs;
using Application.DTOs.CityDTOs;
using Domain.Enums;

namespace Application.DTOs.ReportDTOs
{
    public class ReportViewDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public bool Gender { get; set; }
        public SkinTone SkinTone { get; set; }
        public HairType HairType { get; set; }
        public bool FacialHair { get; set; }
        public HairColor HairColor { get; set; }
        public EyeColor EyeColor { get; set; }
        public HeightLevel HeightLevel { get; set; }
        public WeightLevel WeightLevel { get; set; }
        public byte? MinAge { get; set; }
        public byte? MaxAge { get; set; }
        public string? Street { get; set; }
        public string? District { get; set; }
        public DateTime DateTime { get; set; }
        public required string ContactNumber { get; set; }
        public string? AdditionalInfo { get; set; }
        public bool Valid { get; set; } = false; // false by default
        public bool Missing { get; set; } = false; // false by default
        public virtual ICollection<AttachmentViewDTO> Attachments { get; set; }
        public virtual CityViewDTO City { get; set; }
    }
}
