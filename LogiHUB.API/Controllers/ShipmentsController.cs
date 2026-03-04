using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LogiHUB.Shared.Models;

namespace LogiHUB.API.Controllers
{
    [ApiController]
    [Route("api/shipments")]
    public class ShipmentsController : ControllerBase
    {
        private readonly ShipmentDbContext _context;

        public ShipmentsController(ShipmentDbContext context)
        {
            _context = context;
        }

        // GET: api/shipments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Shipment>>> GetAll()
        {
            return await _context.Shipments.ToListAsync();
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
        public async Task<ActionResult<Shipment>> Create(Shipment shipment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (shipment == null)
                return BadRequest("Shipment cannot be null.");

            shipment.Id = Guid.NewGuid();
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

            return CreatedAtAction(
                actionName: nameof(GetById),
                routeValues: new { id = shipment.Id },
                value: shipment
            );
        }

        // PUT: api/shipments/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, Shipment shipment)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            if (id != shipment.Id) 
                return BadRequest("Shipment ID mismatch!");

            var existingShipment = await _context.Shipments.FindAsync(id);

            if (existingShipment == null) 
                return NotFound();

            // Update only allowed properties
            existingShipment.ShipmentNumber = shipment.ShipmentNumber;
            existingShipment.Origin = shipment.Origin;
            existingShipment.Destination = shipment.Destination;
            existingShipment.Status = shipment.Status;
            existingShipment.PickupDate = shipment.PickupDate;
            existingShipment.DeliveryDate = shipment.DeliveryDate;
            existingShipment.WeightKg = shipment.WeightKg;
            existingShipment.CustomerCode = shipment.CustomerCode;

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

