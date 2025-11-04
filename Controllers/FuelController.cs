using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Autode_objektid.Data;
using Autode_objektid.Models;

namespace Autode_objektid.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FuelController : ControllerBase
    {
        private readonly AppDbContext _db;
        public FuelController(AppDbContext db) => _db = db;

        [HttpGet]
        public async Task<IEnumerable<FuelLog>> Get([FromQuery] int? carId, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var q = _db.FuelLogs.AsNoTracking().AsQueryable();
            if (carId.HasValue) q = q.Where(x => x.CarId == carId.Value);
            if (from.HasValue) q = q.Where(x => x.Date >= from.Value);
            if (to.HasValue) q = q.Where(x => x.Date <= to.Value);
            return await q.OrderByDescending(x => x.Date).ToListAsync();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, FuelLog f)
        {
            if (id != f.Id) return BadRequest();
            _db.Entry(f).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return NoContent();
        }


        [HttpPost] 
        public async Task<ActionResult<FuelLog>> Post(FuelLog f) { _db.FuelLogs.Add(f); await _db.SaveChangesAsync(); return CreatedAtAction(nameof(Get), new { carId = f.CarId }, f); }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) { var f = await _db.FuelLogs.FindAsync(id); if (f == null) return NotFound(); _db.FuelLogs.Remove(f); await _db.SaveChangesAsync(); return NoContent(); }

        // summary liters & cost for car in interval
        [HttpGet("summary")]
        public async Task<ActionResult<object>> Summary([FromQuery] int carId, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var q = _db.FuelLogs.AsNoTracking().Where(x => x.CarId == carId);
            if (from.HasValue) q = q.Where(x => x.Date >= from.Value);
            if (to.HasValue) q = q.Where(x => x.Date <= to.Value);

            var totalLiters = await q.SumAsync(x => (double?)x.Liters) ?? 0.0;
            var totalCost = await q.SumAsync(x => (double?)(x.Liters * x.PricePerLiter)) ?? 0.0;
            return new { carId, totalLiters, totalCost };
        }
    }
}
