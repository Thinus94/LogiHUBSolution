using AutoMapper;
using LogiHUB.API.Constants;
using LogiHUB.Shared.DTOs;
using LogiHUB.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult<PagedResult<ShipmentResponseDto>>> GetAll([FromQuery] ShipmentQueryDto query)
        {
            var shipmentsQuery = _context.Shipments
                .Include(s => s.Customer)
                .Include(s => s.Invoice)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                shipmentsQuery = shipmentsQuery.Where(s =>
                    s.ShipmentNumber.Contains(query.Search) ||
                    s.Origin.Contains(query.Search) ||
                    s.Destination.Contains(query.Search));
            }

            if (!string.IsNullOrWhiteSpace(query.Status))
            {
                shipmentsQuery = shipmentsQuery.Where(s => s.Status == query.Status);
            }

            if (query.CustomerId.HasValue)
            {
                shipmentsQuery = shipmentsQuery.Where(s => s.CustomerId == query.CustomerId);
            }

            var totalCount = await shipmentsQuery.CountAsync();

            var shipments = await shipmentsQuery
                .OrderByDescending(s => s.CreatedDate)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            var result = new PagedResult<ShipmentResponseDto>
            {
                Items = _mapper.Map<List<ShipmentResponseDto>>(shipments),
                TotalCount = totalCount,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            };

            return Ok(result);
        }

        // GET: api/shipments/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ShipmentResponseDto>> GetById(Guid id)
        {
            var shipment = await _context.Shipments
                .Include(s => s.Customer)
                .Include(s => s.Invoice)
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
            shipment.Status = ShipmentStatuses.Created;
            shipment.CreatedDate = DateTime.UtcNow;

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
            if (id != dto.Id)
                return BadRequest();

            if (!ShipmentStatuses.All.Contains(dto.Status))
            {
                return BadRequest("Invalid shipment status.");
            }

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

