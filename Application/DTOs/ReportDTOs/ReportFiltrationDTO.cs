using Domain.Enums;

namespace Domain.DTOs.ReportDTOs
{
    public class ReportFiltrationDTO
    {
        public string? Name { get; set; }
        public bool? Gender { get; set; }
        public SkinTone? SkinTone { get; set; }
        public HairType? HairType { get; set; }
        public bool? FacialHair { get; set; }
        public HairColor? HairColor { get; set; }
        public EyeColor? EyeColor { get; set; }
        public HeightLevel? HeightLevel { get; set; }
        public WeightLevel? WeightLevel { get; set; }
        public byte? MinAge { get; set; }
        public byte? MaxAge { get; set; }
        public string? Street { get; set; }
        public string? District { get; set; }
        public int? CityId { get; set; }
        public DateTime? DateTime { get; set; }
    }
}
