using Domain.Enums;

namespace Domain.Entities
{
    public class Report : BaseModel
    {
        public string? Name { get; set; }
        public Gender Gender { get; set; }
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
        public int CityId { get; set; }
        public DateTime DateTime { get; set; }
        public required string ContactNumber { get; set; }
        public string? AdditionalInfo { get; set; }
        public bool Valid { get; set; } = false; // false by default
        public bool Missing { get; set; } = true; // false by default
        public required string DeletionCode { get; set; } // pass to delete report
        public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
        public virtual City City { get; set; }

    }
}
