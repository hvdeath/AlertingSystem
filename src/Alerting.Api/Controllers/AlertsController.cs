using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alerting.Api.DTOs;
using Alerting.Domain.Entities;
using Alerting.Infrastructure;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alerting.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlertsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public AlertsController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlertRuleDto>>> GetAll()
        {
            var items = await _db.AlertRules.AsNoTracking().ToListAsync();
            return Ok(_mapper.Map<IEnumerable<AlertRuleDto>>(items));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AlertRuleDto>> Get(Guid id)
        {
            var item = await _db.AlertRules.FindAsync(id);
            if (item == null) return NotFound();
            return Ok(_mapper.Map<AlertRuleDto>(item));
        }

        [HttpPost]
        public async Task<ActionResult<AlertRuleDto>> Create([FromBody] CreateAlertRuleDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Construct domain entity explicitly to avoid constructor-mapping edge cases
            var entity = new AlertRule(Guid.NewGuid(), dto.Name, dto.IsActive);
            _db.AlertRules.Add(entity);
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log to console so test runner captures the full exception text
                System.Console.WriteLine("SaveChanges Exception: " + ex.ToString());
                return Problem(detail: ex.ToString());
            }

            var result = _mapper.Map<AlertRuleDto>(entity);
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAlertRuleDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var entity = await _db.AlertRules.FindAsync(id);
            if (entity == null) return NotFound();

            entity.Rename(dto.Name);
            if (entity.IsActive != dto.IsActive) entity = new AlertRule(entity.Id, entity.Name, dto.IsActive);

            _db.AlertRules.Update(entity);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var entity = await _db.AlertRules.FindAsync(id);
            if (entity == null) return NotFound();
            _db.AlertRules.Remove(entity);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
