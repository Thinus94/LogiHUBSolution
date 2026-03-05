using LogiHUB.Shared.DTOs;
using LogiHUB.Shared.Models;
using System.Net.Http;
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

        public async Task<List<Shipment>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Shipment>>("api/shipments")
                   ?? new List<Shipment>();
        }

        public async Task<Shipment?> GetByIdAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<Shipment>($"api/shipments/{id}");
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