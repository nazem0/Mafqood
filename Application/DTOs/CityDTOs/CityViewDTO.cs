using Application.DTOs.GovernorateDTOs;

namespace Application.DTOs.CityDTOs
{
    public class CityViewDTO
    {
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }

        // Navigation property
        public virtual GovernorateViewDTO Governorate { get; set; }
    }
}
