using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Autode_objektid.Data;
using Autode_objektid.Models;

namespace Autode_objektid.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TripLogsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public TripLogsController(AppDbContext db) => _db = db;

        // GET /triplogs?carId=1&from=2025-10-01&to=2025-10-31
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TripLog>>> Get([FromQuery] int? carId, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var q = _db.TripLogs.AsNoTracking().Include(t => t.Car).AsQueryable();
            if (carId.HasValue) q = q.Where(t => t.CarId == carId.Value);
            if (from.HasValue) q = q.Where(t => t.Timestamp >= from.Value);
            if (to.HasValue) q = q.Where(t => t.Timestamp <= to.Value);
            return await q.OrderBy(t => t.Timestamp).ToListAsync();
        }

        // ✅ POST /triplogs
        [HttpPost]
        public async Task<ActionResult<TripLog>> Post(TripLog log)
        {
            if (!_db.Cars.Any(c => c.Id == log.CarId))
                return BadRequest("Invalid CarId.");

            log.Timestamp = log.Timestamp == default ? DateTime.UtcNow : log.Timestamp;
            _db.TripLogs.Add(log);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { carId = log.CarId }, log);
        }

        // ✅ PUT /triplogs/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, TripLog log)
        {
            if (id != log.Id) return BadRequest("ID mismatch");
            _db.Entry(log).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        // ✅ DELETE /triplogs/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var t = await _db.TripLogs.FindAsync(id);
            if (t == null) return NotFound();
            _db.TripLogs.Remove(t);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        // GET /triplogs/summary?carId=1
        [HttpGet("summary")]
        public async Task<ActionResult<object>> Summary([FromQuery] int carId, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var q = _db.TripLogs.AsNoTracking().Where(t => t.CarId == carId);
            if (from.HasValue) q = q.Where(t => t.Timestamp >= from.Value);
            if (to.HasValue) q = q.Where(t => t.Timestamp <= to.Value);
            var total = await q.SumAsync(t => (double?)t.DistanceKm) ?? 0.0;
            var absTotal = await q.SumAsync(t => (double?)Math.Abs(t.DistanceKm)) ?? 0.0;
            return new { carId, total, absTotal };
        }
    }
}
