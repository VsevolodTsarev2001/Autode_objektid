using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Autode_objektid.Data;
using Autode_objektid.Models;

namespace Autode_objektid.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GaragesController : ControllerBase
    {
        private readonly AppDbContext _db;
        public GaragesController(AppDbContext db) => _db = db;

        [HttpGet] public async Task<IEnumerable<Garage>> Get() => await _db.Garages.Include(g => g.Address).AsNoTracking().ToListAsync();
        [HttpGet("{id}")] public async Task<ActionResult<Garage>> Get(int id) => await _db.Garages.Include(g => g.Address).FirstOrDefaultAsync(g => g.Id == id) is Garage gar ? gar : NotFound();
        
        [HttpPost] 
        public async Task<ActionResult<Garage>> Post(Garage g) 
        { 
            _db.Garages.Add(g);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = g.Id }, g); 
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Garage g) 
        {
            if (id != g.Id) return BadRequest();
            _db.Entry(g).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return NoContent(); 
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) 
        { 
            var g = await _db.Garages.FindAsync(id);
            if (g == null) return NotFound(); _db.Garages.Remove(g);
            await _db.SaveChangesAsync();
            return NoContent(); 
        }
    }
}
