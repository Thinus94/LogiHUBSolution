using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LogiHUB.Shared.Models;

namespace LogiHUB.API.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomersController : ControllerBase
    {
        private readonly ShipmentDbContext _context;

        public CustomersController(ShipmentDbContext context)
        {
            _context = context;
        }

        // GET: api/customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetAll()
        {
            return await _context.Customers.ToListAsync();
        }

        // GET: api/customers/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetById(Guid id)
        {
            var customer = await _context.Customers.FindAsync(id);
            return customer == null ? NotFound() : customer;
        }

        // POST: api/customers
        [HttpPost]
        public async Task<ActionResult<Customer>> Create(Customer customer)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (customer == null)
                return BadRequest("Customer cannot be null.");

            customer.Id = Guid.NewGuid();
            _context.Customers.Add(customer);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Error saving customer: {ex.Message}");
            }

            return CreatedAtAction(
                nameof(GetById),
                new { id = customer.Id },
                customer
            );
        }

        // PUT: api/customers/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, Customer customer)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != customer.Id)
                return BadRequest("Customer ID mismatch!");

            var existingCustomer = await _context.Customers.FindAsync(id);
            if (existingCustomer == null) return NotFound();

            // Update properties
            existingCustomer.Name = customer.Name;
            existingCustomer.Email = customer.Email;
            existingCustomer.Phone = customer.Phone;
            existingCustomer.Address = customer.Address;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                    return NotFound();
                throw;
            }

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

        private bool CustomerExists(Guid id)
        {
            return _context.Customers.Any(c => c.Id == id);
        }
    }
}
