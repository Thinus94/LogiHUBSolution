using LogiHUB.Shared.DTOs;
using System.Net.Http.Json;

namespace LogiHUB.UI.Services
{
    public class CustomerService
    {
        private readonly HttpClient _httpClient;

        public CustomerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CustomerResponseDto>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<CustomerResponseDto>>("api/customers")
                   ?? new List<CustomerResponseDto>();
        }

        public async Task<CustomerResponseDto?> GetByIdAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<CustomerResponseDto>($"api/customers/{id}");
        }

        public async Task CreateAsync(CreateCustomerDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/customers", dto);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateAsync(UpdateCustomerDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/customers/{dto.Id}", dto);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/customers/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
