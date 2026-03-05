using System.Net.Http;
using System.Net.Http.Json;
using LogiHUB.Shared.Models;

namespace LogiHUB.UI.Services
{
    public class CustomerService
    {
        private readonly HttpClient _httpClient;

        public CustomerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Customer>>("api/customers")
                   ?? new List<Customer>();
        }

        public async Task<Customer?> GetByIdAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<Customer>($"api/customers/{id}");
        }

        public async Task<Customer> CreateAsync(Customer customer)
        {
            var response = await _httpClient.PostAsJsonAsync("api/customers", customer);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Customer>()
                   ?? throw new Exception("Failed to deserialize customer.");
        }

        public async Task UpdateAsync(Customer customer)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/customers/{customer.Id}", customer);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/customers/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
