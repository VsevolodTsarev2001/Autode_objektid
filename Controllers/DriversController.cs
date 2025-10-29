using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Autode_objektid.Data;
using Autode_objektid.Models;

namespace Autode_objektid.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DriversController : ControllerBase
    {
        private readonly AppDbContext _db;
        public DriversController(AppDbContext db) => _db = db;

        [HttpGet]
        public async Task<IEnumerable<Driver>> Get() => await _db.Drivers.Include(d => d.ContactInfo).AsNoTracking().ToListAsync();
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Driver>> Get(int id) => await _db.Drivers.Include(d => d.ContactInfo).FirstOrDefaultAsync(d => d.Id == id) is Driver dr ? dr : NotFound();
        
        [HttpPost]
        public async Task<ActionResult<Driver>> Post(Driver d) 
        { 
            _db.Drivers.Add(d);
            await _db.SaveChangesAsync(); 
            return CreatedAtAction(nameof(Get), new { id = d.Id }, d); 
        }
        
        [HttpPut("{id}")] 
        public async Task<IActionResult> Put(int id, Driver d) 
        { 
            if (id != d.Id) return BadRequest();
            _db.Entry(d).State = EntityState.Modified; 
            await _db.SaveChangesAsync(); 
            return NoContent(); 
        }
        
        [HttpDelete("{id}")] 
        public async Task<IActionResult> Delete(int id) 
        {
            var d = await _db.Drivers.FindAsync(id);
            if (d == null) return NotFound();
            _db.Drivers.Remove(d);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
