using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LogiHUB.Shared.Models;
using LogiHUB.Shared.DTOs;
using AutoMapper;

namespace LogiHUB.API.Controllers
{
    [ApiController]
    [Route("api/shipments")]
    public class ShipmentsController : ControllerBase
    {
        private readonly ShipmentDbContext _context;
        private readonly IMapper _mapper;

        public ShipmentsController(ShipmentDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/shipments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Shipment>>> GetAll()
        {
            return await _context.Shipments
                .Include(s => s.Customer) // <--- include the customer
                .ToListAsync();
        }

        // GET: api/shipments/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Shipment>> GetById(Guid id)
        {
            var shipment = await _context.Shipments.FindAsync(id);
            return shipment == null ? NotFound() : shipment;
        }

        // POST: api/shipments
        [HttpPost]
        public async Task<ActionResult<Shipment>> Create(CreateShipmentDto dto)
        {
            var shipment = _mapper.Map<Shipment>(dto);
            shipment.Id = Guid.NewGuid();
            shipment.ShipmentNumber = $"SN-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
            shipment.Status = "Created"; // auto

            _context.Shipments.Add(shipment);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Log the exception
                return StatusCode(500, $"Error saving shipment: {ex.Message}");
            }

            return CreatedAtAction(nameof(GetById), new { id = shipment.Id }, shipment);
        }

        // PUT: api/shipments/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateShipmentDto dto)
        {
            var shipment = await _context.Shipments.FindAsync(id);
            if (shipment == null) return NotFound();

            _mapper.Map(dto, shipment);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShipmentExists(id)) 
                    return NotFound();

                throw;
            }
            return NoContent();
        }

        // DELETE: api/shipments/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var shipment = await _context.Shipments.FindAsync(id);
            if (shipment == null) return NotFound();
            _context.Shipments.Remove(shipment);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool ShipmentExists(Guid id)
        {
            return _context.Shipments.Any(e => e.Id == id);
        }

    }
}

