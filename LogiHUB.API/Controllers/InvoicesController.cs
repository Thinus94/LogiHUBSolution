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

        // GET: api/invoices
        [HttpGet]
        public async Task<ActionResult<PagedResult<InvoiceResponseDto>>> GetAll([FromQuery] InvoiceQueryDto query)
        {
            var clientId = GetClient.GetClientId(User);

            var invoicesQuery = _context.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Shipment)
                .Where(i => i.Customer!.ClientId == clientId)
                .AsQueryable();

            if (query.CustomerId.HasValue)
                invoicesQuery = invoicesQuery.Where(i => i.CustomerId == query.CustomerId);

            if (query.ShipmentId.HasValue)
                invoicesQuery = invoicesQuery.Where(i => i.ShipmentId == query.ShipmentId);

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                invoicesQuery = invoicesQuery.Where(i =>
                    i.InvoiceNumber.Contains(query.Search));
            }

            if (query.Status.HasValue)
            {
                invoicesQuery = invoicesQuery.Where(i => i.Status == query.Status.Value);
            }

            if (query.IsActive.HasValue)
            {
                invoicesQuery = invoicesQuery.Where(i => i.IsActive == query.IsActive.Value);
            }

            var totalCount = await invoicesQuery.CountAsync();

            var invoices = await invoicesQuery
                .OrderByDescending(i => i.IssueDate)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return Ok(new PagedResult<InvoiceResponseDto>
            {
                Items = _mapper.Map<List<InvoiceResponseDto>>(invoices),
                TotalCount = totalCount,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            });
        }

        // GET: api/invoices/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<InvoiceResponseDto>> GetById(Guid id)
        {
            var clientId = GetClient.GetClientId(User);

            var invoice = await _context.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Shipment)
                .FirstOrDefaultAsync(i => i.Id == id && i.Customer!.ClientId == clientId);

            if (invoice == null)
                throw new NotFoundException("Invoice not found.");

            return Ok(_mapper.Map<InvoiceResponseDto>(invoice));
        }

        // POST: api/invoices
        [HttpPost]
        public async Task<ActionResult> Create(CreateInvoiceDto dto)
        {
            var clientId = GetClient.GetClientId(User);

            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Id == dto.CustomerId && c.ClientId == clientId);

            if (customer == null)
                throw new BadRequestException("Invalid customer.");

            if (dto.ShipmentId != null)
            {
                var shipment = await _context.Shipments
                    .Include(s => s.Customer)
                    .FirstOrDefaultAsync(s => s.Id == dto.ShipmentId);

                if (shipment == null || shipment.Customer!.ClientId != clientId)
                    throw new BadRequestException("Invalid shipment.");

                if (shipment.CustomerId != dto.CustomerId)
                    throw new BadRequestException("Shipment does not belong to the selected customer.");
            }

            var invoice = _mapper.Map<Invoice>(dto);

            invoice.Id = Guid.NewGuid();
            invoice.InvoiceNumber = $"INV-{Guid.NewGuid().ToString()[..8].ToUpper()}";
            invoice.Status = dto.Status;

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = invoice.Id }, _mapper.Map<InvoiceResponseDto>(invoice));
        }

        // PUT: api/invoices/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateInvoiceDto dto)
        {
            if (id != dto.Id) return BadRequest();

            var clientId = GetClient.GetClientId(User);

            var invoice = await _context.Invoices
                .Include(i => i.Customer)
                .FirstOrDefaultAsync(i => i.Id == id && i.Customer!.ClientId == clientId);

            if (invoice == null)
                throw new NotFoundException("Invoice not found.");

            _mapper.Map(dto, invoice);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/invoices/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var clientId = GetClient.GetClientId(User);

            var invoice = await _context.Invoices
                .Include(i => i.Customer)
                .FirstOrDefaultAsync(i => i.Id == id && i.Customer!.ClientId == clientId);

            if (invoice == null)
                throw new NotFoundException("Invoice not found.");

            _context.Invoices.Remove(invoice);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}