using AutoMapper;
using Application.DTOs.AttachmentDTOs;
using Application.DTOs.CityDTOs;
using Application.DTOs.GovernorateDTOs;
using Application.DTOs.ReportDTOs;
using Domain.Entities;

namespace Infrastructure.Mapping
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
