// Controllers/TripLogsController.cs
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
        public async Task<ActionResult<IEnumerable<TripLog>>> Get([FromQuery] int carId, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var q = _db.TripLogs.AsNoTracking().Where(t => t.CarId == carId);
            if (from.HasValue) q = q.Where(t => t.Timestamp >= from.Value);
            if (to.HasValue) q = q.Where(t => t.Timestamp <= to.Value);
            return await q.OrderBy(t => t.Timestamp).ToListAsync();
        }

        // GET /triplogs/summary?carId=1&from=...&to=...
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
