using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LogiHUB.Shared.Models;
using LogiHUB.Shared.DTOs;
using LogiHUB.API.Middleware;

namespace LogiHUB.API.Controllers
{
    [ApiController]
    [Route("api/clients")]
    public class ClientsController : ControllerBase
    {
        private readonly ShipmentDbContext _context;
        private readonly JwtService _jwtService;

        public ClientsController(ShipmentDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        // REGISTER CLIENT
        [HttpPost("register")]
        public async Task<IActionResult> Register(ClientRegistrationDto dto)
        {
            var existing = await _context.Clients
                .FirstOrDefaultAsync(c => c.Email == dto.Email);

            if (existing != null)
                throw new BadRequestException("Email already registered.");

            var client = new Client
            {
                Id = Guid.NewGuid(),
                CompanyName = dto.CompanyName,
                ContactName = dto.ContactName,
                Email = dto.Email,
                Phone = dto.Phone,
                Address = dto.Address,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                CreatedDate = DateTime.UtcNow
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // LOGIN
        [HttpPost("login")]
        public async Task<ActionResult<object>> Login(ClientLoginDto dto)
        {
            var client = await _context.Clients
                .FirstOrDefaultAsync(c => c.Email == dto.Email);

            if (client == null || !BCrypt.Net.BCrypt.Verify(dto.Password, client.PasswordHash))
                throw new UnauthorizedException("Invalid credentials");

            var token = _jwtService.GenerateToken(client);

            return Ok(new
            {
                token,
                clientId = client.Id,
                companyName = client.CompanyName
            });
        }

        // GET CLIENT PROFILE
        [HttpGet("{id}")]
        public async Task<ActionResult<ClientResponseDto>> Get(Guid id)
        {
            var client = await _context.Clients
                .Include(c => c.Customers)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (client == null)
                throw new NotFoundException("Client not found.");

            var response = new ClientResponseDto
            {
                Id = client.Id,
                CompanyName = client.CompanyName,
                ContactName = client.ContactName,
                Email = client.Email,
                Phone = client.Phone,
                Address = client.Address,
                CustomerCount = client.Customers.Count
            };

            return Ok(response);
        }

        // UPDATE CLIENT PROFILE
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateClientDto dto)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
                throw new NotFoundException("Client not found.");

            client.CompanyName = dto.CompanyName;
            client.ContactName = dto.ContactName;
            client.Email = dto.Email;
            client.Phone = dto.Phone;
            client.Address = dto.Address;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}