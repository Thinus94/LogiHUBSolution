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

        public async Task<List<InvoiceResponseDto>> GetAllAsync()
        {
            return await _http.GetFromJsonAsync<List<InvoiceResponseDto>>("api/invoices")
                   ?? new();
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