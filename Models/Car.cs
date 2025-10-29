// Models/Car.cs
using System.ComponentModel.DataAnnotations;

namespace Autode_objektid.Models
{
    public enum CarType
    {
        Soiduauto = 0,
        Veoauto = 1
    }

    public class Car
    {
        public int Id { get; set; }

        // pikkus in meters
        public double Length { get; set; }

        // mass in tonnes
        public double Mass { get; set; }

        [Required]
        public string Mark { get; set; } = string.Empty;

        // Type (Soiduauto or Veoauto)
        public CarType Type { get; set; }

        // X coordinate for LiikuvAuto logic
        public double X { get; set; } = 0.0;
    }
}
