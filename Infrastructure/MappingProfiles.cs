using AutoMapper;
using Domain.DTOs.AttachmentDTOs;
using Domain.DTOs.CityDTOs;
using Domain.DTOs.GovernorateDTOs;
using Domain.DTOs.ReportDTOs;
using Domain.Entities;

namespace Infrastructure
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            //Use ReverseMap to make two way mapping.
            CreateMap<Attachment, AttachmentViewDTO>();
            CreateMap<Governorate, GovernorateViewDTO>();
            CreateMap<City, CityViewDTO>();
            CreateMap<Report, ReportViewDTO>();
            CreateMap<ReportAdditionDTO, Report>();
        }
    }
}
