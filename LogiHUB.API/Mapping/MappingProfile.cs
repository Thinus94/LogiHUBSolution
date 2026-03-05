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

            // Entity -> DTO (optional but recommended)
            CreateMap<Shipment, UpdateShipmentDto>();
        }
    }
}