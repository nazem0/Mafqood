using Domain.Entities;
using Domain.Enums;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using System.Linq.Expressions;

namespace Application.DTOs.ReportDTOs
{
    public class ReportFiltrationDTO
    {
        public string? Keyword { get; set; }
        public string? Name { get; set; }
        public Gender? Gender { get; set; }
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
        public int? GovernorateId { get; set; }
        public int? CityId { get; set; }
        public bool? Valid { get; set; }
        public bool? Missing { get; set; }
        public DateTime? DateTime { get; set; }
    }
}
