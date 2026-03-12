using AutoMapper;
using LogiHUB.API.Helpers;
using LogiHUB.Shared.DTOs;
using LogiHUB.Shared.Enums;
using LogiHUB.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LogiHUB.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/invoices")]
    public class InvoicesController : ControllerBase
    {
        private readonly ShipmentDbContext _context;
        private readonly IMapper _mapper;

        public InvoicesController(ShipmentDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InvoiceResponseDto>>> GetAll(Guid? customerId)
        {
            var clientId = GetClient.GetClientId(User);

            var query = _context.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Shipment)
                .Where(i => i.Customer!.ClientId == clientId)
                .AsQueryable();

            if (customerId.HasValue)
            {
                query = query.Where(i => i.CustomerId == customerId.Value);
            }

            var invoices = await query.ToListAsync();

            return Ok(_mapper.Map<List<InvoiceResponseDto>>(invoices));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InvoiceResponseDto>> GetById(Guid id)
        {
            var clientId = GetClient.GetClientId(User);

            var invoice = await _context.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Shipment)
                .FirstOrDefaultAsync(i => i.Id == id && i.Customer!.ClientId == clientId);

            if (invoice == null) return NotFound();

            return Ok(_mapper.Map<InvoiceResponseDto>(invoice));
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateInvoiceDto dto)
        {
            var clientId = GetClient.GetClientId(User);

            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Id == dto.CustomerId && c.ClientId == clientId);

            if (customer == null)
                return BadRequest("Invalid customer.");

            var invoice = _mapper.Map<Invoice>(dto);

            invoice.Id = Guid.NewGuid();
            invoice.InvoiceNumber = $"INV-{Guid.NewGuid().ToString()[..8].ToUpper()}";
            invoice.Status = dto.Status;

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateInvoiceDto dto)
        {
            if (id != dto.Id) return BadRequest();

            var clientId = GetClient.GetClientId(User);

            var invoice = await _context.Invoices
                .Include(i => i.Customer)
                .FirstOrDefaultAsync(i => i.Id == id && i.Customer!.ClientId == clientId);

            if (invoice == null) return NotFound();

            _mapper.Map(dto, invoice);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var clientId = GetClient.GetClientId(User);

            var invoice = await _context.Invoices
                .Include(i => i.Customer)
                .FirstOrDefaultAsync(i => i.Id == id && i.Customer!.ClientId == clientId);

            if (invoice == null) return NotFound();

            _context.Invoices.Remove(invoice);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}