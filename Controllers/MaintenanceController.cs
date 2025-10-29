using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Autode_objektid.Data;
using Autode_objektid.Models;

namespace Autode_objektid.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MaintenanceController : ControllerBase
    {
        private readonly AppDbContext _db;
        public MaintenanceController(AppDbContext db) => _db = db;

        [HttpGet]
        public async Task<IEnumerable<MaintenanceRecord>> Get([FromQuery] int? carId, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var q = _db.MaintenanceRecords.AsNoTracking().AsQueryable();
            if (carId.HasValue) q = q.Where(x => x.CarId == carId.Value);
            if (from.HasValue) q = q.Where(x => x.Date >= from.Value);
            if (to.HasValue) q = q.Where(x => x.Date <= to.Value);
            return await q.OrderByDescending(x => x.Date).ToListAsync();
        }

        [HttpPost] public async Task<ActionResult<MaintenanceRecord>> Post(MaintenanceRecord m) { _db.MaintenanceRecords.Add(m); await _db.SaveChangesAsync(); return CreatedAtAction(nameof(Get), new { carId = m.CarId }, m); }
        [HttpDelete("{id}")] public async Task<IActionResult> Delete(int id) { var m = await _db.MaintenanceRecords.FindAsync(id); if (m == null) return NotFound(); _db.MaintenanceRecords.Remove(m); await _db.SaveChangesAsync(); return NoContent(); }

        // summary cost for car in interval
        [HttpGet("summary")]
        public async Task<ActionResult<object>> Summary([FromQuery] int carId, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var q = _db.MaintenanceRecords.AsNoTracking().Where(x => x.CarId == carId);
            if (from.HasValue) q = q.Where(x => x.Date >= from.Value);
            if (to.HasValue) q = q.Where(x => x.Date <= to.Value);
            var total = await q.SumAsync(x => (double?)x.Cost) ?? 0.0;
            return new { carId, totalCost = total };
        }
    }
}
