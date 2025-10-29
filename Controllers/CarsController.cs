// Controllers/CarsController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Autode_objektid.Data;
using Autode_objektid.Models;

namespace Autode_objektid.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CarsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public CarsController(AppDbContext db) => _db = db;

        [HttpGet]
        public async Task<IEnumerable<Car>> GetAll() => await _db.Cars.AsNoTracking().ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Car>> Get(int id)
        {
            var c = await _db.Cars.FindAsync(id);
            return c is null ? NotFound() : c;
        }

        [HttpPost]
        public async Task<ActionResult<Car>> Create(Car input)
        {
            // validation: soiduauto mass <= 3.5
            if (input.Type == CarType.Soiduauto && input.Mass > 3.5)
                return BadRequest("Soiduauto mass ei tohi ületada 3.5 tonni.");

            _db.Cars.Add(input);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = input.Id }, input);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Car input)
        {
            if (id != input.Id) return BadRequest("ID mismatch");
            if (input.Type == CarType.Soiduauto && input.Mass > 3.5)
                return BadRequest("Soiduauto mass ei tohi ületada 3.5 tonni.");

            _db.Entry(input).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var c = await _db.Cars.FindAsync(id);
            if (c is null) return NotFound();
            _db.Cars.Remove(c);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        // POST /cars/{id}/move?direction=edasi|tagasi&km=10
        [HttpPost("{id}/move")]
        public async Task<IActionResult> Move(int id, [FromQuery] string direction, [FromQuery] double km)
        {
            var car = await _db.Cars.FindAsync(id);
            if (car is null) return NotFound();
            if (km <= 0) return BadRequest("Kaugus peab olema positiivne.");

            if (string.Equals(direction, "edasi", StringComparison.OrdinalIgnoreCase))
            {
                car.X += km;
                if (car.Type == CarType.Veoauto)
                {
                    _db.TripLogs.Add(new TripLog { CarId = car.Id, Timestamp = DateTime.UtcNow, DistanceKm = Math.Abs(km) });
                }
            }
            else if (string.Equals(direction, "tagasi", StringComparison.OrdinalIgnoreCase))
            {
                car.X -= km;
                if (car.Type == CarType.Veoauto)
                {
                    _db.TripLogs.Add(new TripLog { CarId = car.Id, Timestamp = DateTime.UtcNow, DistanceKm = -Math.Abs(km) });
                }
            }
            else return BadRequest("Suund peab olema 'edasi' või 'tagasi'.");

            await _db.SaveChangesAsync();
            return Ok(car);
        }
    }
}
