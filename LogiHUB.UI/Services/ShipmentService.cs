using System.Net.Http;
using System.Net.Http.Json;
using LogiHUB.Shared.Models;

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

        public async Task<Shipment> CreateAsync(Shipment shipment)
        {
            var response = await _httpClient.PostAsJsonAsync("api/shipments", shipment);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Shipment>()
                   ?? throw new Exception("Failed to deserialize shipment.");
        }

        public async Task UpdateAsync(Shipment shipment)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/shipments/{shipment.Id}", shipment);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/shipments/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}