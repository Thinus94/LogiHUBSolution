using LogiHUB.Shared.DTOs;
using System.Net.Http.Json;
using LogiHUB.Shared.Enums;

namespace LogiHUB.UI.Services
{
    public class InvoiceService
    {
        private readonly HttpClient _http;

        public InvoiceService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<InvoiceResponseDto>> GetAllAsync(Guid? customerId = null, Guid? shipmentId = null)
        {
            var url = "api/invoices";
            var queryParams = new List<string>();

            if (customerId.HasValue)
                queryParams.Add($"customerId={customerId}");

            if (shipmentId.HasValue)
                queryParams.Add($"shipmentId={shipmentId}");

            if (queryParams.Any())
                url += "?" + string.Join("&", queryParams);

            var invoices = await _http.GetFromJsonAsync<List<InvoiceResponseDto>>(url)
                           ?? new List<InvoiceResponseDto>();

            // Mark overdue invoices
            foreach (var invoice in invoices)
            {
                if (invoice.DueDate.HasValue && invoice.DueDate.Value < DateTime.Today && invoice.Status != InvoiceStatus.Paid)
                {
                    invoice.Status = InvoiceStatus.Overdue;
                }
            }

            return invoices;
        }

        public async Task<InvoiceResponseDto?> GetByIdAsync(Guid id)
        {
            return await _http.GetFromJsonAsync<InvoiceResponseDto>($"api/invoices/{id}");
        }

        public async Task CreateAsync(CreateInvoiceDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/invoices", dto);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateAsync(UpdateInvoiceDto dto)
        {
            var response = await _http.PutAsJsonAsync($"api/invoices/{dto.Id}", dto);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(Guid id)
        {
            var response = await _http.DeleteAsync($"api/invoices/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}