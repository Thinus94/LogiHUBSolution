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

        public async Task<PagedResult<InvoiceResponseDto>> GetAllAsync(
            string search,
            string status,
            Guid? customerId,
            Guid? shipmentId,
            bool? isActive,
            int page,
            int pageSize)
        {
            var parameters = new List<string>();

            if (!string.IsNullOrWhiteSpace(search))
                parameters.Add($"search={search}");

            if (!string.IsNullOrWhiteSpace(status))
                parameters.Add($"status={status}");

            if (customerId.HasValue)
                parameters.Add($"customerId={customerId}");

            if (shipmentId.HasValue)
                parameters.Add($"shipmentId={shipmentId}");

            if (isActive.HasValue)
                parameters.Add($"isActive={isActive.Value}");

            parameters.Add($"pageNumber={page}");
            parameters.Add($"pageSize={pageSize}");

            var url = "api/invoices?" + string.Join("&", parameters);

            return await _http.GetFromJsonAsync<PagedResult<InvoiceResponseDto>>(url)
                   ?? new PagedResult<InvoiceResponseDto>();
        }

        //Overload for simpler calls without filters
        public async Task<PagedResult<InvoiceResponseDto>> GetAllAsync(
            Guid? customerId = null,
            Guid? shipmentId = null)
        {
            return await GetAllAsync(
                "",              // search
                "",              // status
                customerId,
                shipmentId,
                null,            // isActive
                1,               // page
                1000             // pageSize (big enough)
            );
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