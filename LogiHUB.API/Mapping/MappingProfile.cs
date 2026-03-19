using AutoMapper;
using LogiHUB.Shared.Models;
using LogiHUB.Shared.DTOs;

namespace LogiHUB.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // -----------------------------
            // SHIPMENTS
            // -----------------------------

            CreateMap<CreateShipmentDto, Shipment>();

            CreateMap<UpdateShipmentDto, Shipment>();

            CreateMap<Shipment, ShipmentResponseDto>()
                .ForMember(dest => dest.CustomerName,
                    opt => opt.MapFrom(src => src.Customer!.Name))
                .ForMember(dest => dest.InvoiceCount,
                    opt => opt.MapFrom(src => src.Invoices.Count))
                .ForMember(dest => dest.IsActive,
                    opt => opt.MapFrom(src => src.IsActive));


            // -----------------------------
            // CUSTOMERS
            // -----------------------------

            CreateMap<CreateCustomerDto, Customer>();

            CreateMap<UpdateCustomerDto, Customer>();

            CreateMap<Customer, CustomerResponseDto>()
                .ForMember(dest => dest.ShipmentCount,
                    opt => opt.MapFrom(src => src.Shipments.Count))
                .ForMember(dest => dest.InvoiceCount,
                    opt => opt.MapFrom(src => src.Invoices.Count))
                .ForMember(dest => dest.ClientId,
                    opt => opt.MapFrom(src => src.ClientId))
                .ForMember(dest => dest.IsActive,
                    opt => opt.MapFrom(src => src.IsActive));


            // -----------------------------
            // INVOICES
            // -----------------------------

            CreateMap<CreateInvoiceDto, Invoice>();

            CreateMap<UpdateInvoiceDto, Invoice>();

            CreateMap<Invoice, InvoiceResponseDto>()
                .ForMember(dest => dest.CustomerName,
                    opt => opt.MapFrom(src => src.Customer!.Name))
                .ForMember(dest => dest.ShipmentNumber,
                    opt => opt.MapFrom(src => src.Shipment != null ? src.Shipment.ShipmentNumber : null))
                .ForMember(dest => dest.IsActive,
                    opt => opt.MapFrom(src => src.IsActive));


            // -----------------------------
            // CLIENTS
            // -----------------------------

            CreateMap<ClientRegistrationDto, Client>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            CreateMap<UpdateClientDto, Client>();

            CreateMap<Client, ClientResponseDto>()
                .ForMember(dest => dest.CustomerCount,
                    opt => opt.MapFrom(src => src.Customers.Count));
        }
    }
}