using LogiHUB.Shared.DTOs;
using System.Net.Http.Json;
using Blazored.LocalStorage;

namespace LogiHUB.UI.Services
{
    public class ClientService
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;

        public ClientService(HttpClient http, ILocalStorageService localStorage)
        {
            _http = http;
            _localStorage = localStorage;
        }

        public async Task<bool> Register(ClientRegistrationDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/clients/register", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Login(ClientLoginDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/clients/login", dto);
            if (!response.IsSuccessStatusCode) return false;

            var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
            if (result == null) return false;

            await _localStorage.SetItemAsync("authToken", result.Token);
            await _localStorage.SetItemAsync("clientId", result.ClientId);

            return true;
        }

        public async Task<ClientResponseDto?> Get(Guid id)
            => await _http.GetFromJsonAsync<ClientResponseDto>($"api/clients/{id}");

        public async Task Update(Guid id, UpdateClientDto dto)
        {
            var response = await _http.PutAsJsonAsync($"api/clients/{id}", dto);
            response.EnsureSuccessStatusCode();
        }
    }
}
