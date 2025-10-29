using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Autode_objektid.Data;
using Autode_objektid.Models;

namespace Autode_objektid.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AssignmentsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public AssignmentsController(AppDbContext db) => _db = db;

        [HttpGet]
        public async Task<IEnumerable<Assignment>> Get([FromQuery] int? carId, [FromQuery] int? driverId)
        {
            var q = _db.Assignments.AsNoTracking().Include(a => a.Driver).Include(a => a.Car).AsQueryable();
            if (carId.HasValue) q = q.Where(a => a.CarId == carId.Value);
            if (driverId.HasValue) q = q.Where(a => a.DriverId == driverId.Value);
            return await q.OrderByDescending(a => a.From).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Assignment>> Post(Assignment a)
        {
            try
            {
                _db.Assignments.Add(a);
                await _db.SaveChangesAsync();
                return CreatedAtAction(nameof(Get), new { id = a.Id }, a);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new { message = ex.InnerException?.Message ?? ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Assignment a) 
        {
            if (id != a.Id) return BadRequest();
            _db.Entry(a).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return NoContent(); 
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) 
        {
            var a = await _db.Assignments.FindAsync(id);
            if (a == null) return NotFound();
            _db.Assignments.Remove(a);
            await _db.SaveChangesAsync();
            return NoContent(); 
        }
    }
}
