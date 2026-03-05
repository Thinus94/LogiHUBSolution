using AutoMapper;
using LogiHUB.Shared.Models;
using LogiHUB.Shared.DTOs;

namespace LogiHUB.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Create
            CreateMap<CreateShipmentDto, Shipment>();

            // Update
            CreateMap<UpdateShipmentDto, Shipment>();

            // Entity -> Response DTO
            CreateMap<Shipment, ShipmentResponseDto>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer!.Name));
        }
    }
}