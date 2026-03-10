using AutoMapper;
using LogiHUB.Shared.Models;
using LogiHUB.Shared.DTOs;

namespace LogiHUB.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Create Shipment
            CreateMap<CreateShipmentDto, Shipment>();

            // Update Shipment
            CreateMap<UpdateShipmentDto, Shipment>();

            // Entity -> Shipment Response DTO
            CreateMap<Shipment, ShipmentResponseDto>()
                .ForMember(dest => dest.CustomerName,
                    opt => opt.MapFrom(src => src.Customer!.Name))
                .ForMember(dest => dest.InvoiceId,
                    opt => opt.MapFrom(src => src.Invoice != null ? src.Invoice.Id : (Guid?)null))
                .ForMember(dest => dest.InvoiceNumber,
                    opt => opt.MapFrom(src => src.Invoice != null ? src.Invoice.InvoiceNumber : null));

            // Create Customer
            CreateMap<CreateCustomerDto, Customer>();

            // Update Customer
            CreateMap<UpdateCustomerDto, Customer>();

            // Entity -> Customer Response DTO
            CreateMap<Customer, CustomerResponseDto>()
                .ForMember(dest => dest.ShipmentCount,
                    opt => opt.MapFrom(src => src.Shipments.Count))
                .ForMember(dest => dest.InvoiceCount,
                    opt => opt.MapFrom(src => src.Shipments.Count(s => s.Invoice != null)));

            // Create Invoice
            CreateMap<CreateInvoiceDto, Invoice>();

            // Update Invoice
            CreateMap<UpdateInvoiceDto, Invoice>();

            // Entity -> Invoice Response DTO
            CreateMap<Invoice, InvoiceResponseDto>()
                .ForMember(dest => dest.CustomerName,
                    opt => opt.MapFrom(src => src.Customer!.Name))
                .ForMember(dest => dest.ShipmentNumber,
                    opt => opt.MapFrom(src => src.Shipment != null ? src.Shipment.ShipmentNumber : null));
        }
    }
}