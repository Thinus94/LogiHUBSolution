using LogiHUB.Shared.DTOs;
using System.Net.Http.Json;

namespace LogiHUB.UI.Services
{
    public class ShipmentService
    {
        private readonly HttpClient _httpClient;

        public ShipmentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PagedResult<ShipmentResponseDto>> GetAllAsync(
            string search,
            string status,
            Guid? customerId,
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

            parameters.Add($"pageNumber={page}");
            parameters.Add($"pageSize={pageSize}");

            var url = "api/shipments?" + string.Join("&", parameters);

            return await _httpClient.GetFromJsonAsync<PagedResult<ShipmentResponseDto>>(url)
                   ?? new PagedResult<ShipmentResponseDto>();
        }

        public async Task<ShipmentResponseDto?> GetByIdAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<ShipmentResponseDto>($"api/shipments/{id}");
        }

        public async Task CreateAsync(CreateShipmentDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/shipments", dto);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateAsync(UpdateShipmentDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/shipments/{dto.Id}", dto);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/shipments/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}