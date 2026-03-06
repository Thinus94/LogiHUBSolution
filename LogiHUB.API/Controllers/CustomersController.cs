using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LogiHUB.Shared.Models;
using LogiHUB.Shared.DTOs;
using AutoMapper;

namespace LogiHUB.API.Controllers
{
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
        public async Task<ActionResult<IEnumerable<CustomerResponseDto>>> GetAll()
        {
            var customers = await _context.Customers
                .Include(c => c.Shipments)
                .ToListAsync();

            var response = _mapper.Map<List<CustomerResponseDto>>(customers);
            return Ok(response);
        }

        // GET: api/customers/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponseDto>> GetById(Guid id)
        {
            var customer = await _context.Customers
                .Include(c => c.Shipments)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null) return NotFound();

            var response = _mapper.Map<CustomerResponseDto>(customer);
            return Ok(response);
        }

        // POST: api/customers
        [HttpPost]
        public async Task<ActionResult<CustomerResponseDto>> Create(CreateCustomerDto dto)
        {
            var customer = _mapper.Map<Customer>(dto);

            _context.Customers.Add(customer);

            await _context.SaveChangesAsync();

            var response = _mapper.Map<CustomerResponseDto>(customer);

            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, response);
        }

        // PUT: api/customers/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateCustomerDto dto)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return NotFound();

            _mapper.Map(dto, customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/customers/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return NotFound();

            // Optional: Check if this customer has shipments
            var hasShipments = await _context.Shipments.AnyAsync(s => s.CustomerId == id);
            if (hasShipments)
                return BadRequest("Cannot delete customer with existing shipments.");

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
