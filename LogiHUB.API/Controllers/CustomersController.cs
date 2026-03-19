using AutoMapper;
using LogiHUB.API.Helpers;
using LogiHUB.API.Middleware;
using LogiHUB.Shared.DTOs;
using LogiHUB.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LogiHUB.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/customers")]
    public class CustomersController : ControllerBase
    {
        private readonly ShipmentDbContext _context;
        private readonly IMapper _mapper;

        public CustomersController(ShipmentDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/customers
        [HttpGet]
        public async Task<ActionResult<PagedResult<CustomerResponseDto>>> GetAll([FromQuery] CustomerQueryDto query)
        {
            var clientId = GetClient.GetClientId(User);

            var customersQuery = _context.Customers
                .Where(c => c.ClientId == clientId)
                .Include(c => c.Shipments)
                .Include(c => c.Invoices)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                customersQuery = customersQuery.Where(c =>
                    c.Name.Contains(query.Search) ||
                    c.Email.Contains(query.Search) ||
                    c.Phone.Contains(query.Search));
            }

            if (query.IsActive.HasValue)
            {
                customersQuery = customersQuery.Where(c => c.IsActive == query.IsActive.Value);
            }

            var totalCount = await customersQuery.CountAsync();

            var customers = await customersQuery
                .OrderByDescending(c => c.CreatedDate)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return Ok(new PagedResult<CustomerResponseDto>
            {
                Items = _mapper.Map<List<CustomerResponseDto>>(customers),
                TotalCount = totalCount,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            });
        }

        // GET: api/customers/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponseDto>> GetById(Guid id)
        {
            var clientId = GetClient.GetClientId(User);

            var customer = await _context.Customers
                .Include(c => c.Shipments)
                .Include(c => c.Invoices)
                .FirstOrDefaultAsync(c => c.Id == id && c.ClientId == clientId);

            if (customer == null)
                throw new NotFoundException("Customer not found.");

            var response = _mapper.Map<CustomerResponseDto>(customer);
            return Ok(response);
        }

        // POST: api/customers
        [HttpPost]
        public async Task<ActionResult<CustomerResponseDto>> Create(CreateCustomerDto dto)
        {
            var clientId = GetClient.GetClientId(User);

            var customer = _mapper.Map<Customer>(dto);
            customer.ClientId = clientId;
            customer.CreatedDate = DateTime.UtcNow;
            _context.Customers.Add(customer);

            await _context.SaveChangesAsync();

            var response = _mapper.Map<CustomerResponseDto>(customer);

            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, response);
        }

        // PUT: api/customers/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateCustomerDto dto)
        {
            var clientId = GetClient.GetClientId(User);

            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Id == id && c.ClientId == clientId);

            if (customer == null)
                throw new NotFoundException("Customer not found.");

            _mapper.Map(dto, customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/customers/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var clientId = GetClient.GetClientId(User);

            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Id == id && c.ClientId == clientId);

            if (customer == null)
                throw new NotFoundException("Customer not found.");

            // Optional: Check if this customer has shipments
            var hasShipments = await _context.Shipments.AnyAsync(s => s.CustomerId == id);
            if (hasShipments)
                throw new BadRequestException("Cannot delete customer with existing shipments.");

            //_context.Customers.Remove(customer);
            // SOFT DELETE
            customer.IsActive = false;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
