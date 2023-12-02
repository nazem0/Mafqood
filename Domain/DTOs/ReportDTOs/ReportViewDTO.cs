using Domain.DTOs.AttachmentDTOs;
using Domain.DTOs.CityDTOs;

namespace Domain.DTOs.ReportDTOs
{
    public class ReportViewDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public bool Gender { get; set; }
        public int SkinTone { get; set; }
        public int HairType { get; set; }
        public int HairColor { get; set; }
        public int EyeColor { get; set; }
        public int HeightLevel { get; set; }
        public int WeightLevel { get; set; }
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
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
