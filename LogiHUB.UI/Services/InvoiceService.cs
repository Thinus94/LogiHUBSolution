using LogiHUB.Shared.DTOs;
using System.Net.Http.Json;

namespace LogiHUB.UI.Services
{
    public class InvoiceService
    {
        private readonly HttpClient _http;

        public InvoiceService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<InvoiceResponseDto>> GetAllAsync(Guid? customerId)
        {
            var url = "api/invoices";

            if (customerId.HasValue)
            {
                url += $"?customerId={customerId}";
            }

            var invoices = await _http.GetFromJsonAsync<List<InvoiceResponseDto>>(url)
                           ?? new();

            foreach (var invoice in invoices)
            {
                if (invoice.DueDate < DateTime.Today && invoice.Status != "Paid")
                {
                    invoice.Status = "Overdue";
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