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

        public async Task<PagedResult<CustomerResponseDto>> GetAllAsync(
            string search,
            bool? isActive,
            int page,
            int pageSize)
        {
            var parameters = new List<string>();

            if (!string.IsNullOrWhiteSpace(search))
                parameters.Add($"search={search}");

            if (isActive.HasValue)
                parameters.Add($"isActive={isActive.Value}");

            parameters.Add($"pageNumber={page}");
            parameters.Add($"pageSize={pageSize}");

            var url = "api/customers?" + string.Join("&", parameters);

            return await _httpClient.GetFromJsonAsync<PagedResult<CustomerResponseDto>>(url)
                   ?? new PagedResult<CustomerResponseDto>();
        }

        // Overload for simpler calls without filters
        public async Task<PagedResult<CustomerResponseDto>> GetAllAsync()
        {
            return await GetAllAsync(
                "",
                null,
                1,
                1000
            );
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
