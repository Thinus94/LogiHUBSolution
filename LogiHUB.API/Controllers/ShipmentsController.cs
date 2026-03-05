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
        public async Task<ActionResult<IEnumerable<ShipmentResponseDto>>> GetAll()
        {
            var shipments = await _context.Shipments
                .Include(s => s.Customer)
                .ToListAsync();

            var response = _mapper.Map<List<ShipmentResponseDto>>(shipments);
            return Ok(response);
        }

        // GET: api/shipments/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ShipmentResponseDto>> GetById(Guid id)
        {
            var shipment = await _context.Shipments
                .Include(s => s.Customer)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (shipment == null) return NotFound();

            var response = _mapper.Map<ShipmentResponseDto>(shipment);
            return Ok(response);
        }

        // POST: api/shipments
        [HttpPost]
        public async Task<ActionResult<ShipmentResponseDto>> Create(CreateShipmentDto dto)
        {
            var shipment = _mapper.Map<Shipment>(dto);
            shipment.Id = Guid.NewGuid();
            shipment.ShipmentNumber = $"SN-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
            shipment.Status = "Created";

            _context.Shipments.Add(shipment);
            await _context.SaveChangesAsync();

            await _context.Entry(shipment).Reference(s => s.Customer).LoadAsync(); // ensure customer is loaded
            var response = _mapper.Map<ShipmentResponseDto>(shipment);

            return CreatedAtAction(nameof(GetById), new { id = shipment.Id }, response);
        }

        // PUT: api/shipments/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateShipmentDto dto)
        {
            var shipment = await _context.Shipments.FindAsync(id);
            if (shipment == null) return NotFound();

            _mapper.Map(dto, shipment);
            await _context.SaveChangesAsync();

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
    }
}

