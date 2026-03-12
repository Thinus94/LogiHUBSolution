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
                .ForMember(dest => dest.InvoiceId,
                    opt => opt.MapFrom(src => src.Invoice != null ? src.Invoice.Id : (Guid?)null))
                .ForMember(dest => dest.InvoiceNumber,
                    opt => opt.MapFrom(src => src.Invoice != null ? src.Invoice.InvoiceNumber : null));


            // -----------------------------
            // CUSTOMERS
            // -----------------------------

            CreateMap<CreateCustomerDto, Customer>();

            CreateMap<UpdateCustomerDto, Customer>();

            CreateMap<Customer, CustomerResponseDto>()
                .ForMember(dest => dest.ShipmentCount,
                    opt => opt.MapFrom(src => src.Shipments.Count))
                .ForMember(dest => dest.InvoiceCount,
                    opt => opt.MapFrom(src => src.Shipments.Count(s => s.Invoice != null)))
                .ForMember(dest => dest.ClientId,
                    opt => opt.MapFrom(src => src.ClientId));


            // -----------------------------
            // INVOICES
            // -----------------------------

            CreateMap<CreateInvoiceDto, Invoice>();

            CreateMap<UpdateInvoiceDto, Invoice>();

            CreateMap<Invoice, InvoiceResponseDto>()
                .ForMember(dest => dest.CustomerName,
                    opt => opt.MapFrom(src => src.Customer!.Name))
                .ForMember(dest => dest.ShipmentNumber,
                    opt => opt.MapFrom(src => src.Shipment != null ? src.Shipment.ShipmentNumber : null));


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